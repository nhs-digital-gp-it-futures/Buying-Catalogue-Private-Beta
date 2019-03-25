using System.Collections.Generic;
using System.Linq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public sealed class CapabilitiesImplementedEvidenceBlobStoreLogic : EvidenceBlobStoreLogic, ICapabilitiesImplementedEvidenceBlobStoreLogic
  {
    public CapabilitiesImplementedEvidenceBlobStoreLogic(
      IEvidenceBlobStoreDatastore evidenceBlobStoreDatastore,
      ISolutionsDatastore solutionsDatastore,
      ICapabilitiesImplementedDatastore capabilitiesImplementedDatastore,
      IStandardsApplicableDatastore standardsApplicableDatastore,
      ICapabilitiesDatastore capabilitiesDatastore,
      IStandardsDatastore standardsDatastore,
      ICapabilitiesImplementedEvidenceBlobStoreValidator validator) :
      base(
        evidenceBlobStoreDatastore,
        solutionsDatastore,
        capabilitiesImplementedDatastore,
        standardsApplicableDatastore,
        capabilitiesDatastore,
        standardsDatastore,
        validator)
    {
    }

    public override string GetFolderName()
    {
      return CapabilityFolderName;
    }

    public override string GetFolderClaimName(ClaimsBase claim)
    {
      var specifiClaim = (CapabilitiesImplemented)claim;
      var cap = _capabilitiesDatastore.ById(specifiClaim.CapabilityId);

      return cap.Name;
    }

    public override IEnumerable<Quality> GetAllQualities()
    {
      return _capabilitiesDatastore.GetAll();
    }

    protected override IClaimsDatastore<ClaimsBase> ClaimsDatastore
    {
      get
      {
        return (IClaimsDatastore<ClaimsBase>)_capabilitiesImplementedDatastore;
      }
    }
  }
}
