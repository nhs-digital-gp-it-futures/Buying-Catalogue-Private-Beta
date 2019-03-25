using FluentValidation;
using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class KeywordSearchHistoryLogic : LogicBase, IKeywordSearchHistoryLogic
  {
    private readonly IKeywordSearchHistoryDatastore _datastore;
    private readonly IKeywordSearchHistoryValidator _validator;

    public KeywordSearchHistoryLogic(
      IKeywordSearchHistoryDatastore datastore,
      IHttpContextAccessor context,
      IKeywordSearchHistoryValidator validator) :
      base(context)
    {
      _datastore = datastore;
      _validator = validator;
    }

    public IEnumerable<KeywordCount> Get(DateTime startDate, DateTime endDate)
    {
      _validator.ValidateAndThrowEx(Context);
      return _datastore.Get(startDate, endDate);
    }
  }
}
