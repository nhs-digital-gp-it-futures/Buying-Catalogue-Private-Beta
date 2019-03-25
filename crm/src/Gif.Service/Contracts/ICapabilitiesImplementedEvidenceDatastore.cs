using Gif.Service.Models;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface ICapabilitiesImplementedEvidenceDatastore : IEvidenceDatastore<CapabilityEvidence>
  {
    CapabilityImplemented ByEvidenceId(string id);
  }
#pragma warning restore CS1591
}
