using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SharePoint.Client.NetCore;
using Microsoft.SharePoint.Client.NetCore.Runtime;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public sealed class EvidenceBlobStoreDatastore : IEvidenceBlobStoreDatastore
  {
    private readonly IHostingEnvironment _env;
    private readonly IAuthenticationManager _authMgr;

    private readonly IOrganisationsDatastore _organisationsDatastore;
    private readonly ISolutionsDatastore _solutionsDatastore;
    private readonly ICapabilitiesImplementedDatastore _capabilitiesImplementedDatastore;
    private readonly IStandardsApplicableDatastore _standardsApplicableDatastore;
    private readonly ICapabilitiesDatastore _capabilitiesDatastore;
    private readonly IStandardsDatastore _standardsDatastore;

    private readonly bool _logSharePoint;
    private readonly bool _isFakeSharePoint;
    private readonly ILogger<IEvidenceBlobStoreDatastore> _logger;
    private readonly ISyncPolicy _policy;

    private readonly string SharePoint_BaseUrl;
    private readonly string SharePoint_OrganisationsRelativeUrl;
    private readonly string SharePoint_ClientId;
    private readonly string SharePoint_ClientSecret;
    private readonly string SharePoint_Login;
    private readonly string SharePoint_Password;

    public EvidenceBlobStoreDatastore(
      IHostingEnvironment env,
      IConfiguration config,
      IAuthenticationManager authMgr,
      IOrganisationsDatastore organisationsDatastore,
      ISolutionsDatastore solutionsDatastore,
      ICapabilitiesImplementedDatastore capabilitiesImplementedDatastore,
      IStandardsApplicableDatastore standardsApplicableDatastore,
      ICapabilitiesDatastore capabilitiesDatastore,
      IStandardsDatastore standardsDatastore,
      ILogger<IEvidenceBlobStoreDatastore> logger,
      ISyncPolicyFactory policy
      )
    {
      _env = env;
      _authMgr = authMgr;

      _solutionsDatastore = solutionsDatastore;
      _organisationsDatastore = organisationsDatastore;
      _capabilitiesImplementedDatastore = capabilitiesImplementedDatastore;
      _standardsApplicableDatastore = standardsApplicableDatastore;
      _capabilitiesDatastore = capabilitiesDatastore;
      _standardsDatastore = standardsDatastore;

      _logSharePoint = Settings.LOG_SHAREPOINT(config);
      _isFakeSharePoint = Settings.SHAREPOINT_PROVIDER_FAKE(config);
      _logger = logger;
      _policy = policy.Build(_logger);

      SharePoint_BaseUrl = Settings.SHAREPOINT_BASEURL(config);
      SharePoint_OrganisationsRelativeUrl = Settings.SHAREPOINT_ORGANISATIONSRELATIVEURL(config);
      SharePoint_ClientId = Settings.SHAREPOINT_CLIENT_ID(config);
      SharePoint_ClientSecret = Settings.SHAREPOINT_CLIENT_SECRET(config);
      SharePoint_Login = Settings.SHAREPOINT_LOGIN(config);
      SharePoint_Password = Settings.SHAREPOINT_PASSWORD(config);

      if (string.IsNullOrWhiteSpace(SharePoint_BaseUrl) ||
        string.IsNullOrWhiteSpace(SharePoint_OrganisationsRelativeUrl) ||
        string.IsNullOrWhiteSpace(SharePoint_ClientId) ||
        string.IsNullOrWhiteSpace(SharePoint_ClientSecret) ||
        string.IsNullOrWhiteSpace(SharePoint_Login) ||
        string.IsNullOrWhiteSpace(SharePoint_Password)
        )
      {
        throw new ConfigurationErrorsException("Missing SharePoint configuration - check UserSecrets or environment variables");
      }
    }

    public string AddEvidenceForClaim(IClaimsInfoProvider claimsInfoProvider, string claimId, Stream file, string fileName, string subFolder = null)
    {
      return GetInternal(() =>
      {
        LogInformation($"AddEvidenceForClaim: claimId: {claimId} | fileName: {CleanupFileName(fileName)} | subFolder: {CleanupFileName(subFolder)}");
        var blobId = UploadFileSlicePerSlice(claimsInfoProvider, claimId, file, CleanupFileName(fileName), CleanupFileName(subFolder));
        LogInformation($"AddEvidenceForClaim: claimId: {claimId} | fileName: {CleanupFileName(fileName)} | subFolder: {CleanupFileName(subFolder)} --> {blobId}");

        return blobId;
      });
    }

    private string UploadFileSlicePerSlice(
      IClaimsInfoProvider claimsInfoProvider,
      string claimId,
      Stream file,
      string fileName,
      string subFolder,
      int fileChunkSizeInMB = 3)
    {
      // Each sliced upload requires a unique id
      var uploadId = Guid.NewGuid();

      // Get to folder to upload into
      var claim = claimsInfoProvider.GetClaimById(claimId);
      var soln = _solutionsDatastore.ById(claim.SolutionId);
      var org = _organisationsDatastore.ById(soln.OrganisationId);
      var solnVer = GetSolutionVersionFolderName(soln);
      var claimFolderRelUrl = Url.Combine(
        SharePoint_OrganisationsRelativeUrl,
        CleanupFileName(org.Name),
        solnVer,
        CleanupFileName(claimsInfoProvider.GetFolderName()),
        CleanupFileName(claimsInfoProvider.GetFolderClaimName(claim)));

      // create subFolder if not exists
      if (!string.IsNullOrEmpty(subFolder))
      {
        CreateSubFolder(claimFolderRelUrl, subFolder);
        claimFolderRelUrl = Url.Combine(claimFolderRelUrl, subFolder);
      }

      var context = GetClientContext();
      var docClaimFolder = context.Web.GetFolderByServerRelativeUrl(claimFolderRelUrl);
      context.ExecuteQuery();

      // Get the information about the folder that will hold the file
      LogInformation($"UploadFileSlicePerSlice: enumerating {Url.Combine(context.Url, claimFolderRelUrl)}...");
      context.Load(docClaimFolder.Files);
      context.Load(docClaimFolder, folder => folder.ServerRelativeUrl);
      context.ExecuteQuery();

      using (var br = new BinaryReader(file))
      {
        var fileSize = file.Length;
        ClientResult<long> bytesUploaded = null;
        Microsoft.SharePoint.Client.NetCore.File uploadFile = null;

        // Calculate block size in bytes
        var blockSize = fileChunkSizeInMB * 1024 * 1024;

        byte[] buffer = new byte[blockSize];
        byte[] lastBuffer = null;
        long fileoffset = 0;
        long totalBytesRead = 0;
        int bytesRead;
        bool first = true;
        bool last = false;

        // Read data from stream in blocks 
        while ((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
        {
          totalBytesRead += bytesRead;

          // We've reached the end of the file
          if (totalBytesRead == fileSize)
          {
            last = true;

            // Copy to a new buffer that has the correct size
            lastBuffer = new byte[bytesRead];
            Array.Copy(buffer, 0, lastBuffer, 0, bytesRead);
          }

          if (first)
          {
            using (var contentStream = new MemoryStream())
            {
              // Add an empty file.
              var fileInfo = new FileCreationInformation
              {
                ContentStream = contentStream,
                Url = fileName,
                Overwrite = true
              };
              uploadFile = docClaimFolder.Files.Add(fileInfo);

              // Start upload by uploading the first slice
              // NOTE:  small files will be contained in the lastBuffer, so use this to upload in one call
              using (var strm = new MemoryStream(last ? lastBuffer : buffer))
              {
                // Call the start upload method on the first slice
                bytesUploaded = uploadFile.StartUpload(uploadId, strm);

                LogInformation($"UploadFileSlicePerSlice: uploading first slice...");
                context.ExecuteQuery();

                // fileoffset is the pointer where the next slice will be added
                fileoffset = bytesUploaded.Value;
              }

              // NOTE:  small files have already been uploaded from lastBuffer, so reset it
              lastBuffer = new byte[0];
            }
          }

          // Get a reference to our file
          LogInformation($"UploadFileSlicePerSlice: getting reference to file...");
          uploadFile = context.Web.GetFileByServerRelativeUrl(Url.Combine(docClaimFolder.ServerRelativeUrl, fileName));

          if (last)
          {
            // Is this the last slice of data?
            using (var strm = new MemoryStream(lastBuffer))
            {
              // End sliced upload by calling FinishUpload
              LogInformation($"UploadFileSlicePerSlice: uploading last slice...");
              uploadFile = uploadFile.FinishUpload(uploadId, fileoffset, strm);
              context.Load(uploadFile);
              context.ExecuteQuery();

              return uploadFile.UniqueId.ToString();
            }
          }

          if (first)
          {
            // we can only start the upload once
            first = false;

            continue;
          }

          using (var strm = new MemoryStream(buffer))
          {
            // Continue sliced upload
            LogInformation($"UploadFileSlicePerSlice: uploading intermediate slice...");
            bytesUploaded = uploadFile.ContinueUpload(uploadId, fileoffset, strm);
            context.ExecuteQuery();

            // update fileoffset for the next slice
            fileoffset = bytesUploaded.Value;
          }
        }
      }

      return string.Empty;
    }

    public IEnumerable<BlobInfo> EnumerateFolder(IClaimsInfoProvider claimsInfoProvider, string claimId, string subFolder = null)
    {
      return GetInternal(() =>
      {
        LogInformation($"EnumerateFolder: claimId: {claimId} | subFolder: {CleanupFileName(subFolder)}");
        var claim = claimsInfoProvider.GetClaimById(claimId);
        var soln = _solutionsDatastore.ById(claim.SolutionId);
        var org = _organisationsDatastore.ById(soln.OrganisationId);

        var context = GetClientContext();
        var claimFolderUrl = Url.Combine(
          SharePoint_OrganisationsRelativeUrl,
          CleanupFileName(org.Name),
          GetSolutionVersionFolderName(soln),
          CleanupFileName(claimsInfoProvider.GetFolderName()),
          CleanupFileName(claimsInfoProvider.GetFolderClaimName(claim)));
        if (!string.IsNullOrEmpty(subFolder))
        {
          claimFolderUrl = Url.Combine(claimFolderUrl, subFolder);
        }
        var claimFolder = context.Web.GetFolderByServerRelativeUrl(claimFolderUrl);

        context.Load(claimFolder);
        context.Load(claimFolder.Files);
        context.Load(claimFolder.Folders);

        LogInformation($"EnumerateFolder: enumerating {Url.Combine(context.Url, claimFolderUrl)}...");
        context.ExecuteQuery();

        var claimFolderInfo = new BlobInfo
        {
          Id = claimFolder.UniqueId.ToString(),
          Name = claimFolder.Name,
          IsFolder = true,
          Url = new Uri(new Uri(context.Url), claimFolder.ServerRelativeUrl).AbsoluteUri,
          TimeLastModified = claimFolder.TimeLastModified
        };
        var claimSubFolderInfos = claimFolder
          .Folders
          .Select(x =>
            new BlobInfo
            {
              Id = x.UniqueId.ToString(),
              ParentId = claimFolderInfo.Id,
              Name = x.Name,
              IsFolder = true,
              Length = 0,
              Url = new Uri(new Uri(context.Url), x.ServerRelativeUrl).AbsoluteUri,
              TimeLastModified = x.TimeLastModified
            });
        var claimFileInfos = claimFolder
          .Files
          .Select(x =>
            new BlobInfo
            {
              Id = x.UniqueId.ToString(),
              ParentId = claimFolderInfo.Id,
              Name = x.Name,
              IsFolder = false,
              Length = x.Length,
              Url = new Uri(new Uri(context.Url), x.ServerRelativeUrl).AbsoluteUri,
              TimeLastModified = x.TimeLastModified,
              BlobId = x.UniqueId.ToString()
            });
        var retVal = new List<BlobInfo>();

        retVal.Add(claimFolderInfo);
        retVal.AddRange(claimSubFolderInfos);
        retVal.AddRange(claimFileInfos);

        return retVal;
      });
    }

    public IEnumerable<ClaimBlobInfoMap> EnumerateClaimFolderTree(IClaimsInfoProvider claimsInfoProvider, string solutionId)
    {
      return GetInternal(() =>
      {
        var soln = _solutionsDatastore.ById(solutionId);
        var claims = claimsInfoProvider.GetClaimBySolution(solutionId);
        var org = _organisationsDatastore.ById(soln.OrganisationId);
        var claimFolder = CleanupFileName(claimsInfoProvider.GetFolderName());
        var solutionUrl = Url.Combine(
          SharePoint_OrganisationsRelativeUrl,
          CleanupFileName(org.Name),
          GetSolutionVersionFolderName(soln));
        var allBlobInfos = EnumerateFolderRecursively(solutionUrl, claimFolder);
        var quals = claimsInfoProvider.GetAllQualities();

        var retval = new List<ClaimBlobInfoMap>();
        foreach (var qual in quals)
        {
          // specific claim folder eg 'Standards Evidence/Patient Management'
          var qualFolder = allBlobInfos.SingleOrDefault(bi => bi.Name == CleanupFileName(qual.Name) && bi.IsFolder);
          if (qualFolder == null)
          {
            continue;
          }

          // look for claim corresponding to this Capability/Standard
          var claim = claims.SingleOrDefault(c => c.QualityId == qual.Id);
          if (claim == null)
          {
            continue;
          }

          var map = new ClaimBlobInfoMap { ClaimId = claim.Id };
          var blobInfos = new List<BlobInfo>(new[] { qualFolder });

          // files and sub-folders in specific claim folder
          var subBlobInfos = allBlobInfos.Where(abi => abi.ParentId == qualFolder.Id);
          blobInfos.AddRange(subBlobInfos);

          // files in a specific claim sub folder eg 'Standards Evidence/Patient Management/Video Evidence'
          var subBlobInfosFolders = subBlobInfos.Where(sbi => sbi.IsFolder);
          var subBlobInfosFiles = subBlobInfosFolders.SelectMany(sbif => allBlobInfos.Where(abi => abi.ParentId == sbif.Id));
          blobInfos.AddRange(subBlobInfosFiles);

          map.BlobInfos = blobInfos;
          retval.Add(map);
        }

        return retval;
      });
    }

    private IEnumerable<BlobInfo> EnumerateFolderRecursively(string solutionUrl, string claimFolder)
    {
      var context = GetClientContext();

      // top level folder eg 'Standards Evidence'
      var claimFolderUrl = Url.Combine(solutionUrl, claimFolder);
      var contextFolder = context.Web.GetFolderByServerRelativeUrl(claimFolderUrl);
      context.Load(contextFolder);
      context.Load(contextFolder.Folders);
      context.Load(contextFolder.Files);
      context.ExecuteQuery();

      // each specific claim folder eg 'Standards Evidence/Patient Management'
      contextFolder.Folders.ToList().ForEach(x => context.Load(x.Files));
      contextFolder.Folders.ToList().ForEach(x => context.Load(x.Folders));
      context.ExecuteQuery();

      // each specific claim sub folder eg 'Standards Evidence/Patient Management/Video Evidence'
      contextFolder.Folders.ToList().SelectMany(x => x.Folders).ToList().ForEach(x => context.Load(x.Files));
      contextFolder.Folders.ToList().SelectMany(x => x.Folders).ToList().ForEach(x => context.Load(x.Folders));
      context.ExecuteQuery();


      var retval = new List<BlobInfo>();

      retval.Add(new BlobInfo
      {
        Id = contextFolder.UniqueId.ToString(),
        ParentId = null,
        Name = contextFolder.Name,
        IsFolder = true,
        Url = new Uri(new Uri(context.Url), contextFolder.ServerRelativeUrl).AbsoluteUri,
        TimeLastModified = contextFolder.TimeLastModified
      });
      foreach (var folder in contextFolder.Folders)
      {
        retval.Add(new BlobInfo
        {
          Id = folder.UniqueId.ToString(),
          ParentId = contextFolder.UniqueId.ToString(),
          Name = folder.Name,
          IsFolder = true,
          Url = new Uri(new Uri(context.Url), folder.ServerRelativeUrl).AbsoluteUri,
          TimeLastModified = folder.TimeLastModified
        });
        foreach (var subFolder in folder.Folders)
        {
          retval.Add(new BlobInfo
          {
            Id = subFolder.UniqueId.ToString(),
            ParentId = folder.UniqueId.ToString(),
            Name = subFolder.Name,
            IsFolder = true,
            Url = new Uri(new Uri(context.Url), subFolder.ServerRelativeUrl).AbsoluteUri,
            TimeLastModified = subFolder.TimeLastModified
          });
          foreach (var subFile in subFolder.Files)
          {
            retval.Add(new BlobInfo
            {
              Id = subFile.UniqueId.ToString(),
              ParentId = subFolder.UniqueId.ToString(),
              Name = subFile.Name,
              IsFolder = false,
              Url = new Uri(new Uri(context.Url), subFile.ServerRelativeUrl).AbsoluteUri,
              TimeLastModified = subFile.TimeLastModified,
              BlobId = subFile.UniqueId.ToString()
            });
          }
        }
        foreach (var file in folder.Files)
        {
          retval.Add(new BlobInfo
          {
            Id = file.UniqueId.ToString(),
            ParentId = folder.UniqueId.ToString(),
            Name = file.Name,
            IsFolder = false,
            Url = new Uri(new Uri(context.Url), file.ServerRelativeUrl).AbsoluteUri,
            TimeLastModified = file.TimeLastModified,
            BlobId = file.UniqueId.ToString()
          });
        }
      }

      return retval;
    }

    public FileStreamResult GetFileStream(IClaimsInfoProvider claimsInfoProvider, string claimId, string uniqueId)
    {
      return GetInternal(() =>
      {
        LogInformation($"GetFileStream: claimId: {claimId} | uniqueId: {uniqueId}");

        var context = CreateClientContextByUserNamePassword();
        var file = context.Web.GetFileById(Guid.Parse(uniqueId));
        context.Load(file);
        context.ExecuteQuery();
        LogInformation($"GetFileStream: retrieved info for {file.Name}");

        return
          // File.OpenBinaryDirect will only work with a username/password context as CSOM uses basic authentication.
          // For an add-in context, CSOM does not set the authentication.
          // This probably because SP uses WebDAV as the underlying protocol and this will allow SP
          // to determine the calling user's permissions.
          //
          // An add-in should use:
          //    File.OpenBinaryStream
          // but this is broken for Dot Net Core CSOM (but works for .NET CSOM).
          // It may be possible to directly call the SP restful API:
          //    https://docs.microsoft.com/en-us/previous-versions/office/developer/sharepoint-rest-reference/dn450841%28v%3doffice.15%29
          // but I (TDE) couldn't get it working.
          new FileStreamResult(Microsoft.SharePoint.Client.NetCore.File.OpenBinaryDirect(context, file.ServerRelativeUrl)?.Stream, GetContentType(file.Name))
          {
            FileDownloadName = Path.GetFileName(file.Name)
          };
      });
    }

    public void PrepareForSolution(IClaimsInfoProvider claimsInfoProvider, string solutionId)
    {
      GetInternal(() =>
      {
        if (_env.IsDevelopment() && _isFakeSharePoint)
        {
          LogInformation($"PrepareForSolution disabled in 'test' Development environment");
          return 0;
        }

        LogInformation($"PrepareForSolution: solutionId: {solutionId}");
        var soln = _solutionsDatastore.ById(solutionId);
        if (soln == null)
        {
          throw new KeyNotFoundException($"Could not find solution: {solutionId}");
        }
        var org = _organisationsDatastore.ById(soln.OrganisationId);
        var claimedCapNames = _capabilitiesImplementedDatastore
          .BySolution(solutionId)
          .Select(x => CleanupFileName(_capabilitiesDatastore.ById(x.CapabilityId).Name));
        var claimedNameStds = _standardsApplicableDatastore
          .BySolution(solutionId)
          .Select(x => CleanupFileName(_standardsDatastore.ById(x.StandardId).Name));

        var capsTask = Task.Factory.StartNew(() => CreateClaimSubFolders(CreateClientContext(), SharePoint_OrganisationsRelativeUrl, CleanupFileName(org.Name), CleanupFileName(soln.Name), CleanupFileName(soln.Version), CleanupFileName(claimsInfoProvider.GetCapabilityFolderName()), claimedCapNames));
        var stdsTask = Task.Factory.StartNew(() => CreateClaimSubFolders(CreateClientContext(), SharePoint_OrganisationsRelativeUrl, CleanupFileName(org.Name), CleanupFileName(soln.Name), CleanupFileName(soln.Version), CleanupFileName(claimsInfoProvider.GetStandardsFolderName()), claimedNameStds));
        Task.WaitAll(capsTask, stdsTask);

        return 0;
      });
    }

    private ClientContext CreateClientContext()
    {
      return _authMgr.GetAppOnlyAuthenticatedContext(SharePoint_BaseUrl, SharePoint_ClientId, SharePoint_ClientSecret);
    }

    private ClientContext CreateClientContextByUserNamePassword()
    {
      var securePassword = new SecureString();
      foreach (char item in SharePoint_Password)
      {
        securePassword.AppendChar(item);
      }

      var encodedUrl = Uri.EscapeUriString(SharePoint_BaseUrl);
      var onlineCredentials = new SharePointOnlineCredentials(SharePoint_Login, securePassword);
      var context = new ClientContext(encodedUrl)
      {
        Credentials = onlineCredentials
      };

      return context;
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }

    private const int SPServerFolderAlreadyExistsExceptionErrorCode = -2130245363;

    // can only create sub-folder immediately under baseUrl
    private void CreateSubFolder(string baseUrl, string subFolder)
    {
      LogInformation($"CreateSubFolder: baseUrl: {baseUrl} | subFolder: {subFolder}");
      var context = GetClientContext();
      var baseFolder = context.Web.GetFolderByServerRelativeUrl(baseUrl);
      baseFolder.AddSubFolder(subFolder);
      context.Load(baseFolder);
      try
      {
        LogInformation($"CreateSubFolder: creating sub-folder [{subFolder}]");
        context.ExecuteQuery();
      }
      catch (ServerException sex) when (sex.ServerErrorCode == SPServerFolderAlreadyExistsExceptionErrorCode)
      {
        LogInformation($"CreateSubFolder: sub-folder [{subFolder}] exists");
      }
    }

    private void CreateClaimSubFolders(
      ClientContext context,
      string baseUrl,
      string organisation,
      string solution,
      string version,
      string claimType,
      IEnumerable<string> claimNames)
    {
      var baseFolder = context.Web.GetFolderByServerRelativeUrl(baseUrl);
      context.Load(baseFolder);
      context.ExecuteQuery();
      LogInformation($"Created BaseFolder: {baseUrl}");

      var orgFolder = baseFolder.Folders.Add(organisation);
      context.Load(orgFolder);
      context.ExecuteQuery();
      LogInformation($"Created OrgFolder: {organisation}");

      var solnVer = GetSolutionVersionFolderName(solution, version);
      var solnFolder = orgFolder.Folders.Add(solnVer);
      context.Load(solnFolder);
      context.ExecuteQuery();
      LogInformation($"Created SolnVerFolder: {solnVer}");

      var claimTypeFolder = solnFolder.Folders.Add(claimType);
      context.Load(claimTypeFolder);
      context.ExecuteQuery();
      LogInformation($"Created ClaimTypeFolder: {claimType}");

      foreach (var claimName in claimNames)
      {
        var claimFolder = claimTypeFolder.Folders.Add(claimName);
        context.Load(claimFolder);
      }
      context.ExecuteQuery();
      LogInformation($"Created claims folders: [{string.Join(',', claimNames)}]");
    }

    private static string GetSolutionVersionFolderName(Solutions soln)
    {
      return GetSolutionVersionFolderName(soln.Name, soln.Version);
    }

    private static string GetSolutionVersionFolderName(string solution, string version)
    {
      return $"{CleanupFileName(solution)} - [{CleanupFileName(version) ?? string.Empty}]";
    }

    private static string CleanupFileName(string fileName)
    {
      // Special Characters Not Allowed: ~ " # % & * : < > ? / \ { | }
      if (!string.IsNullOrEmpty(fileName))
      {
        // Regex to Replace the Special Character
        fileName = Regex.Replace(fileName, @"[~#'%&*:<>?/\{|}\n]", "");

        if (fileName.Contains("\""))
        {
          fileName = fileName.Replace("\"", "");
        }

        if (fileName.StartsWith(".", StringComparison.OrdinalIgnoreCase) ||
          fileName.EndsWith(".", StringComparison.OrdinalIgnoreCase))
        {
          fileName = fileName.TrimStart(new char[] { '.' });
        }

        if (fileName.IndexOf("..", StringComparison.OrdinalIgnoreCase) > -1)
        {
          fileName = fileName.Replace("..", "");
        }

        fileName = fileName.Replace("/n", string.Empty);
      }

      return fileName;
    }

    private static string GetContentType(string path)
    {
      var types = GetMimeTypes();
      var ext = Path.GetExtension(path).ToLowerInvariant();

      return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
    }

    private static Dictionary<string, string> GetMimeTypes()
    {
      return new Dictionary<string, string>
      {
          {".txt", "text/plain"},
          {".pdf", "application/pdf"},
          {".doc", "application/vnd.ms-word"},
          {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
          {".xls", "application/vnd.ms-excel"},
          {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
          {".ppt", "application/vnd.ms-powerpoint" },
          {".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
          {".png", "image/png"},
          {".jpg", "image/jpeg"},
          {".jpeg", "image/jpeg"},
          {".gif", "image/gif"},
          {".csv", "text/csv"},
          {".mp4", "video/mp4"}
      };
    }

    private void LogInformation(string msg)
    {
      if (_logSharePoint)
      {
        _logger.LogInformation(msg);
      }
    }

    private ClientContext GetClientContext()
    {
      return _authMgr.GetAppOnlyAuthenticatedContext(SharePoint_BaseUrl, SharePoint_ClientId, SharePoint_ClientSecret);
    }
  }
}
