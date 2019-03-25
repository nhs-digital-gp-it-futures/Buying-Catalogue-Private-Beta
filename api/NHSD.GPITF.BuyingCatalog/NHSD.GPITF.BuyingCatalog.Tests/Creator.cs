using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Primitives;
using NHSD.GPITF.BuyingCatalog.Authentications;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace NHSD.GPITF.BuyingCatalog.Tests
{
  public static class Creator
  {
    public static TokenValidatedContext GetTokenValidatedContext(string bearerToken)
    {
      var authScheme = new AuthenticationScheme("BearerAuthentication", "BearerAuthentication", typeof(DummyAuthenticationHandler));
      var options = new JwtBearerOptions();
      var ctx = new TokenValidatedContext(Creator.GetContext(bearerToken), authScheme, options)
      {
        Principal = new ClaimsPrincipal()
      };

      return ctx;
    }

    public static DummyHttpContext GetContext(string bearerToken)
    {
      var ctx = new DummyHttpContext();
      ((FrameRequestHeaders)ctx.Request.Headers).HeaderAuthorization = new StringValues(bearerToken);
      return ctx;
    }

    public static DefaultHttpContext GetContext(
      string orgId = "NHS Digital",
      string role = Roles.Admin,
      string email = "NHS-GPIT@WigglyAmps.com")
    {
      var orgClaim = new Claim(nameof(Organisations), orgId);
      var roleClaim = new Claim(ClaimTypes.Role, role);
      var emailClaim = new Claim(ClaimTypes.Email, email);
      var claimsIdentity = new ClaimsIdentity(new[] { orgClaim, roleClaim, emailClaim });
      var user = new ClaimsPrincipal(new[] { claimsIdentity });
      var ctx = new DefaultHttpContext { User = user };

      return ctx;
    }

    public static UserInfoResponse GetUserInfoResponse(
      IEnumerable<(string Type, string Value)> claims
      )
    {
      var jsonClaimsArray = claims.Select(c => $"{c.Type}:\"{c.Value}\"");
      var jsonClaims = "{" + string.Join(',', jsonClaimsArray) + "}";
      var response = new UserInfoResponse(jsonClaims);
      return response;
    }

    public static CachedUserInfoResponse GetCachedUserInfoResponseExpired(UserInfoResponse userInfoResponse)
    {
      return new CachedUserInfoResponse(userInfoResponse, new DateTime(2006, 2, 20));
    }

    public static Frameworks GetFramework(
      string id = null)
    {
      var retval = new Frameworks
      {
        Id = id ?? Guid.NewGuid().ToString()
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static Contacts GetContact(
      string id = null,
      string orgId = null,
      string emailAddress1 = null)
    {
      var retval = new Contacts
      {
        Id = id ?? Guid.NewGuid().ToString(),
        OrganisationId = orgId ?? Guid.NewGuid().ToString(),
        EmailAddress1 = emailAddress1 ?? "jon.dough@tpp.com"
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static Organisations GetOrganisation(
      string id = "NHS Digital",
      string primaryRoleId = PrimaryRole.GovernmentDepartment)
    {
      var retval = new Organisations
      {
        Id = id,
        Name = id,
        PrimaryRoleId = primaryRoleId
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static Solutions GetSolution(
      string id = null,
      string previousId = null,
      string orgId = null,
      SolutionStatus status = SolutionStatus.Draft,
      string createdById = null,
      DateTime? createdOn = null,
      string modifiedById = null,
      DateTime? modifiedOn = null)
    {
      var retval = new Solutions
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = previousId,
        OrganisationId = orgId ?? Guid.NewGuid().ToString(),
        Status = status,
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.Now,
        ModifiedById = modifiedById ?? Guid.NewGuid().ToString(),
        ModifiedOn = modifiedOn ?? DateTime.Now
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static SolutionEx GetSolutionEx(
      Solutions soln = null,

      List<CapabilitiesImplemented> claimedCap = null,
      List<CapabilitiesImplementedEvidence> claimedCapEv = null,
      List<CapabilitiesImplementedReviews> claimedCapRev = null,

      List<StandardsApplicable> claimedStd = null,
      List<StandardsApplicableEvidence> claimedStdEv = null,
      List<StandardsApplicableReviews> claimedStdRev = null,

      List<TechnicalContacts> techCont = null
      )
    {
      soln = soln ?? GetSolution();

      claimedCap = claimedCap ?? new List<CapabilitiesImplemented>
      {
        GetCapabilitiesImplemented(solnId: soln.Id)
      };
      claimedCapEv = claimedCapEv ?? new List<CapabilitiesImplementedEvidence>
      {
        GetCapabilitiesImplementedEvidence(claimId: claimedCap.First().Id)
      };
      claimedCapRev = claimedCapRev ?? new List<CapabilitiesImplementedReviews>
      {
        GetCapabilitiesImplementedReviews(evidenceId: claimedCapEv.First().Id)
      };

      claimedStd = claimedStd ?? new List<StandardsApplicable>
      {
        GetStandardsApplicable(solnId: soln.Id)
      };
      claimedStdEv = claimedStdEv ?? new List<StandardsApplicableEvidence>
      {
        GetStandardsApplicableEvidence(claimId: claimedStd.First().Id)
      };
      claimedStdRev = claimedStdRev ?? new List<StandardsApplicableReviews>
      {
        GetStandardsApplicableReviews(evidenceId: claimedStdEv.First().Id)
      };

      techCont = techCont ?? new List<TechnicalContacts>
      {
        GetTechnicalContact(solutionId: soln.Id)
      };

      var solnEx = new SolutionEx
      {
        Solution = soln,

        ClaimedCapability = claimedCap,
        ClaimedCapabilityEvidence = claimedCapEv,
        ClaimedCapabilityReview = claimedCapRev,

        ClaimedStandard = claimedStd,
        ClaimedStandardEvidence = claimedStdEv,
        ClaimedStandardReview = claimedStdRev,

        TechnicalContact = techCont
      };

      Verifier.Verify(solnEx);
      return solnEx;
    }

    public static TechnicalContacts GetTechnicalContact(
      string id = null,
      string solutionId = null,
      string contactType = "Technical Contact",
      string emailAddress = "jon.dough@tpp.com"
      )
    {
      var retval = new TechnicalContacts
      {
        Id = id ?? Guid.NewGuid().ToString(),
        SolutionId = solutionId ?? Guid.NewGuid().ToString(),
        ContactType = contactType,
        EmailAddress = emailAddress
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static Capabilities GetCapability(
      string id = null,
      string name = null,
      string description = null)
    {
      var retval = new Capabilities
      {
        Id = id ?? Guid.NewGuid().ToString(),
        Name = name ?? string.Empty,
        Description = description ?? string.Empty
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static ClaimsBase GetClaimsBase(
      string id = null,
      string solnId = null,
      string ownerId = null,
      DateTime? originalDate = null)
    {
      var retval = new DummyClaimsBase
      {
        Id = id ?? Guid.NewGuid().ToString(),
        SolutionId = solnId ?? Guid.NewGuid().ToString(),
        OwnerId = ownerId ?? Guid.NewGuid().ToString(),
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static CapabilitiesImplemented GetCapabilitiesImplemented(
      string id = null,
      string solnId = null,
      string claimId = null,
      string ownerId = null,
      CapabilitiesImplementedStatus status = CapabilitiesImplementedStatus.Draft,
      DateTime? originalDate = null)
    {
      var retval = new CapabilitiesImplemented
      {
        Id = id ?? Guid.NewGuid().ToString(),
        SolutionId = solnId ?? Guid.NewGuid().ToString(),
        CapabilityId = claimId ?? Guid.NewGuid().ToString(),
        OwnerId = ownerId ?? Guid.NewGuid().ToString(),
        Status = status,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static StandardsApplicable GetStandardsApplicable(
      string id = null,
      string solnId = null,
      string claimId = null,
      string ownerId = null,
      StandardsApplicableStatus status = StandardsApplicableStatus.Draft,
      DateTime? originalDate = null,
      DateTime? submittedOn = null,
      DateTime? assignedOn = null)
    {
      var retval = new StandardsApplicable
      {
        Id = id ?? Guid.NewGuid().ToString(),
        SolutionId = solnId ?? Guid.NewGuid().ToString(),
        StandardId = claimId ?? Guid.NewGuid().ToString(),
        OwnerId = ownerId ?? Guid.NewGuid().ToString(),
        Status = status,
        OriginalDate = originalDate ?? DateTime.UtcNow,
        SubmittedOn = submittedOn ?? DateTime.UtcNow,
        AssignedOn = assignedOn ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static DummyEvidenceBase GetEvidenceBase(
      string id = null,
      string prevId = null,
      string claimId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new DummyEvidenceBase
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        ClaimId = claimId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.Now,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static CapabilitiesImplementedEvidence GetCapabilitiesImplementedEvidence(
      string id = null,
      string prevId = null,
      string claimId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new CapabilitiesImplementedEvidence
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        ClaimId = claimId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.UtcNow,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static StandardsApplicableEvidence GetStandardsApplicableEvidence(
      string id = null,
      string prevId = null,
      string claimId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new StandardsApplicableEvidence
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        ClaimId = claimId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.UtcNow,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static DummyReviewsBase GetReviewsBase(
      string id = null,
      string prevId = null,
      string evidenceId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new DummyReviewsBase
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        EvidenceId = evidenceId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.Now,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static CapabilitiesImplementedReviews GetCapabilitiesImplementedReviews(
      string id = null,
      string prevId = null,
      string evidenceId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new CapabilitiesImplementedReviews
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        EvidenceId = evidenceId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.UtcNow,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }

    public static StandardsApplicableReviews GetStandardsApplicableReviews(
      string id = null,
      string prevId = null,
      string evidenceId = null,
      string createdById = null,
      DateTime? createdOn = null,
      DateTime? originalDate = null)
    {
      var retval = new StandardsApplicableReviews
      {
        Id = id ?? Guid.NewGuid().ToString(),
        PreviousId = prevId,
        EvidenceId = evidenceId ?? Guid.NewGuid().ToString(),
        CreatedById = createdById ?? Guid.NewGuid().ToString(),
        CreatedOn = createdOn ?? DateTime.UtcNow,
        OriginalDate = originalDate ?? DateTime.UtcNow
      };
      Verifier.Verify(retval);
      return retval;
    }
  }
}
