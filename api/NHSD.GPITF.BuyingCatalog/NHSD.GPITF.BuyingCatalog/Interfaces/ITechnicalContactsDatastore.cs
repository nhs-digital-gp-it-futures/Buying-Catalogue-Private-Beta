using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface ITechnicalContactsDatastore
  {
    IEnumerable<TechnicalContacts> BySolution(string solutionId);
    TechnicalContacts Create(TechnicalContacts techCont);
    void Update(TechnicalContacts techCont);
    void Delete(TechnicalContacts techCont);
  }
#pragma warning restore CS1591
}
