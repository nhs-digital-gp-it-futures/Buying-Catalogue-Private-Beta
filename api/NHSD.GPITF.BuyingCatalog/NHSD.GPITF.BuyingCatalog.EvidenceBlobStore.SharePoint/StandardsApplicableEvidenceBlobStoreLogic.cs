using System.Collections.Generic;
using System.Linq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.EvidenceBlobStore.SharePoint
{
  public sealed class StandardsApplicableEvidenceBlobStoreLogic : EvidenceBlobStoreLogic, IStandardsApplicableEvidenceBlobStoreLogic
  {
    public StandardsApplicableEvidenceBlobStoreLogic(
      IEvidenceBlobStoreDatastore evidenceBlobStoreDatastore,
      ISolutionsDatastore solutionsDatastore,
      ICapabilitiesImplementedDatastore capabilitiesImplementedDatastore,
      IStandardsApplicableDatastore standardsApplicableDatastore,
      ICapabilitiesDatastore capabilitiesDatastore,
      IStandardsDatastore standardsDatastore,
      IStandardsApplicableEvidenceBlobStoreValidator validator) :
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
      return StandardsFolderName;
    }

    public override string GetFolderClaimName(ClaimsBase claim)
    {
      var specifiClaim = (StandardsApplicable)claim;
      var std = _standardsDatastore.ById(specifiClaim.StandardId);

      return std.Name;
    }

    public override IEnumerable<Quality> GetAllQualities()
    {
      return _standardsDatastore.GetAll();
    }

    protected override IClaimsDatastore<ClaimsBase> ClaimsDatastore
    {
      get
      {
        return (IClaimsDatastore<ClaimsBase>)_standardsApplicableDatastore;
      }
    }
  }
}
