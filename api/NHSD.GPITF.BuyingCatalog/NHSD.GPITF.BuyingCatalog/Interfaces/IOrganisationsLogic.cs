﻿using NHSD.GPITF.BuyingCatalog.Models;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IOrganisationsLogic
  {
    Organisations ByContact(string contactId);
  }
#pragma warning restore CS1591
}
