using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ClaimsLogicBase<T> : LogicBase, IClaimsLogic<T> where T : ClaimsBase
  {
    protected readonly IClaimsBaseModifier<T> _modifier;
    protected readonly IClaimsDatastore<T> _datastore;
    protected readonly IClaimsValidator<T> _validator;
    protected readonly IClaimsFilter<T> _filter;

    public ClaimsLogicBase(
      IClaimsBaseModifier<T> modifier,
      IClaimsDatastore<T> datastore,
      IClaimsValidator<T> validator,
      IClaimsFilter<T> filter,
      IHttpContextAccessor context) :
      base(context)
    {
      _modifier = modifier;
      _datastore = datastore;
      _validator = validator;
      _filter = filter;
    }

    public T ById(string id)
    {
      return _filter.Filter(new[] { _datastore.ById(id) }).SingleOrDefault();
    }

    public IEnumerable<T> BySolution(string solutionId)
    {
      return _filter.Filter(_datastore.BySolution(solutionId));
    }

    public T Create(T claim)
    {
      _validator.ValidateAndThrowEx(claim, ruleSet: nameof(IClaimsLogic<T>.Create));

      _modifier.ForCreate(claim);

      return _datastore.Create(claim);
    }

    public virtual void Update(T claim)
    {
      _validator.ValidateAndThrowEx(claim, ruleSet: nameof(IClaimsLogic<T>.Update));

      _datastore.Update(claim);
    }

    public void Delete(T claim)
    {
      _validator.ValidateAndThrowEx(claim, ruleSet: nameof(IClaimsLogic<T>.Delete));

      _datastore.Delete(claim);
    }
  }
}
