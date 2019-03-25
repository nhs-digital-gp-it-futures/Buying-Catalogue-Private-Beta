using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Interfaces
{
#pragma warning disable CS1591
  public interface IKeywordSearchHistoryLogic
  {
    IEnumerable<KeywordCount> Get(DateTime startDate, DateTime endDate);
  }
#pragma warning restore CS1591
}
