using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces.Porcelain;
using NHSD.GPITF.BuyingCatalog.Models.Porcelain;
using Polly;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Meta.Porcelain
{
  public sealed class CapabilityMappingsDatastore : ICapabilityMappingsDatastore
  {
    private readonly ILogger<CapabilityMappingsDatastore> _logger;
    private readonly ISyncPolicy _policy;
    private readonly ICapabilityStandardDatastore _capabilityStandardDatastore;
    private readonly ICapabilitiesDatastore _capabilitiesDatastore;
    private readonly IStandardsDatastore _standardsDatastore;

    public CapabilityMappingsDatastore(
      ILogger<CapabilityMappingsDatastore> logger,
      ISyncPolicyFactory policy,
      ICapabilityStandardDatastore capabilityStandardDatastore,
      ICapabilitiesDatastore capabilitiesDatastore,
      IStandardsDatastore standardsDatastore)
    {
      _logger = logger;
      _policy = policy.Build(_logger);
      _capabilityStandardDatastore = capabilityStandardDatastore;
      _capabilitiesDatastore = capabilitiesDatastore;
      _standardsDatastore = standardsDatastore;
    }

    public CapabilityMappings GetAll()
    {
      return GetInternal(() =>
      {
        var retval = new CapabilityMappings();

        var capStds = _capabilityStandardDatastore.GetAll();
        var caps = _capabilitiesDatastore.GetAll();
        var stds = _standardsDatastore.GetAll();

        foreach (var cap in caps)
        {
          var thisCapMap = retval.CapabilityMapping.SingleOrDefault(x => x.Capability.Id == cap.Id);
          if (thisCapMap == null)
          {
            thisCapMap = new CapabilityMapping
            {
              Capability = cap
            };

            var optStds = capStds
              .Where(cs => cs.CapabilityId == cap.Id)
              .Select(cs =>
                new OptionalStandard
                {
                  StandardId = cs.StandardId,
                  IsOptional = cs.IsOptional
                });
            thisCapMap.OptionalStandard.AddRange(optStds);

            retval.CapabilityMapping.Add(thisCapMap);
          }
        }

        retval.Standard.AddRange(stds);

        return retval;
      });
    }

    private TOther GetInternal<TOther>(Func<TOther> get)
    {
      return _policy.Execute(get);
    }
  }
}
