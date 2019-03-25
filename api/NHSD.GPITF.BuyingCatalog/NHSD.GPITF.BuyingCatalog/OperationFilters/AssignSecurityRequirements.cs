using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.OperationFilters
{
#pragma warning disable CS1591
  public sealed class AssignSecurityRequirements : IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      // check if the operation has the Authorize attribute
      if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
      {
        var authorizeAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true);
        if (!authorizeAttributes.Any())
        {
          // check if parent controller has the Authorize attribute
          var controllerAuthorizeAttributes = controllerActionDescriptor.ControllerTypeInfo.CustomAttributes.Where(x => x.AttributeType == typeof(AuthorizeAttribute));
          if (!controllerAuthorizeAttributes.Any())
          {
            return;
          }
        }

        // initialize the operation.Security property
        if (operation.Security == null)
        {
          operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
        }

        // add the appropriate security definition to the operation
        var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
        {
          { "oauth2", Enumerable.Empty<string>() }
        };
        operation.Security.Add(oAuthRequirements);

        // add the appropriate security definition to the operation
        var basicRequirements = new Dictionary<string, IEnumerable<string>>
        {
          { "basic", Enumerable.Empty<string>() }
        };
        operation.Security.Add(basicRequirements);
      }
    }
  }
#pragma warning restore CS1591
}
