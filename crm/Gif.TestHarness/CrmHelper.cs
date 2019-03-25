using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Configuration;
using System.Net;

namespace Gif.TestHarness
{
    public static class CrmHelper
    {
        public static IOrganizationService GetService()
        {
            IOrganizationService service = null;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var connectionString = ConfigurationManager.ConnectionStrings["Crm"].ConnectionString;
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            var connection = new CrmServiceClient(connectionString);
            var proxy = connection.OrganizationServiceProxy;

            proxy.Timeout = new TimeSpan(0, 0, 2, 0);

            return proxy;
        }
    }
}
