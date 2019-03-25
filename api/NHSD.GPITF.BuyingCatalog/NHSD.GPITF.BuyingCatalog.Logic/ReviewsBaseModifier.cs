using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public abstract class ReviewsBaseModifier<T> : IReviewsBaseModifier<T> where T : ReviewsBase
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactsDatastore _contacts;

    public ReviewsBaseModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts)
    {
      _context = context;
      _contacts = contacts;
    }

    public void ForCreate(T input)
    {
      var email = _context.Email();
      input.CreatedById = _contacts.ByEmail(email).Id;
      input.CreatedOn = input.OriginalDate = DateTime.UtcNow;
    }

    public void ForUpdate(T input)
    {
      input.OriginalDate = (input.OriginalDate == default(DateTime)) ? DateTime.UtcNow : input.OriginalDate;
    }
  }
}
