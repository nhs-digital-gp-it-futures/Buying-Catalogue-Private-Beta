using NHSD.GPITF.BuyingCatalog.Models;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class StandardsApplicableModifier : ClaimsBaseModifier<StandardsApplicable>, IStandardsApplicableModifier
  {
    public override void ForUpdate(StandardsApplicable input)
    {
      base.ForUpdate(input);

      // if we leave as default DateTime, then GIF.Service treats this as a null data and filters it out,
      // so set to an arbitrary date
      input.SubmittedOn = (input.SubmittedOn == default(DateTime)) ? DateTime.UnixEpoch : input.SubmittedOn;
      input.AssignedOn = (input.AssignedOn == default(DateTime)) ? DateTime.UnixEpoch : input.AssignedOn;

      if (input.Status == StandardsApplicableStatus.Submitted)
      {
        input.SubmittedOn = DateTime.UtcNow;
      }
    }
  }
}
