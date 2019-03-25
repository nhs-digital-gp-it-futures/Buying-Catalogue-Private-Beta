using FluentValidation;
using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class TechnicalContactsLogic : LogicBase, ITechnicalContactsLogic
  {
    private readonly ITechnicalContactsDatastore _datastore;
    private readonly ITechnicalContactsValidator _validator;
    private readonly ITechnicalContactsFilter _filter;

    public TechnicalContactsLogic(
      ITechnicalContactsDatastore datastore,
      IHttpContextAccessor context,
      ITechnicalContactsValidator validator,
      ITechnicalContactsFilter filter) :
      base(context)
    {
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public IEnumerable<TechnicalContacts> BySolution(string solutionId)
    {
      return _filter.Filter(_datastore.BySolution(solutionId));
    }

    public TechnicalContacts Create(TechnicalContacts techCont)
    {
      _validator.ValidateAndThrowEx(techCont, ruleSet: nameof(ITechnicalContactsLogic.Create));
      return _datastore.Create(techCont);
    }

    public void Delete(TechnicalContacts techCont)
    {
      _validator.ValidateAndThrowEx(techCont, ruleSet: nameof(ITechnicalContactsLogic.Delete));
      _datastore.Delete(techCont);
    }

    public void Update(TechnicalContacts techCont)
    {
      _validator.ValidateAndThrowEx(techCont, ruleSet: nameof(ITechnicalContactsLogic.Update));
      _datastore.Update(techCont);
    }
  }
}
