using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NHSD.GPITF.BuyingCatalog.OperationFilters
{
#pragma warning disable CS1591
  public sealed class EvidenceForClaimFileUploadOperation : IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      operation.Parameters.Clear();
      operation.Parameters.Add(new NonBodyParameter
      {
        Name = "claimId",
        In = "formData",
        Description = "Unique identifier of claim",
        Required = true,
        Type = "string"
      });
      operation.Parameters.Add(new NonBodyParameter
      {
        Name = "file",
        In = "formData",
        Description = "Client file path",
        Required = true,
        Type = "file"
      });
      operation.Parameters.Add(new NonBodyParameter
      {
        Name = "filename",
        In = "formData",
        Description = "Server file name",
        Required = true,
        Type = "string"
      });
      operation.Parameters.Add(new NonBodyParameter
      {
        Name = "subFolder",
        In = "formData",
        Description = "optional sub-folder",
        Required = false,
        Type = "string"
      });
      operation.Consumes.Add("multipart/form-data");
    }
  }
#pragma warning restore CS1591
}
