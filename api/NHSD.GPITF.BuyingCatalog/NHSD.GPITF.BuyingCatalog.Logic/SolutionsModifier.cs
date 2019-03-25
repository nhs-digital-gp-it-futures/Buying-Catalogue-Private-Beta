using Microsoft.AspNetCore.Http;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System;

namespace NHSD.GPITF.BuyingCatalog.Logic
{
  public sealed class SolutionsModifier : ISolutionsModifier
  {
    private readonly IHttpContextAccessor _context;
    private readonly IContactsDatastore _contacts;

    public SolutionsModifier(
      IHttpContextAccessor context,
      IContactsDatastore contacts)
    {
      _context = context;
      _contacts = contacts;
    }

    public void ForCreate(Solutions input)
    {
      var email = _context.Email();
      input.CreatedById = input.ModifiedById = _contacts.ByEmail(email).Id;
      input.CreatedOn = input.ModifiedOn = DateTime.UtcNow;
    }

    public void ForUpdate(Solutions input)
    {
      var email = _context.Email();
      input.ModifiedById = _contacts.ByEmail(email).Id;
      input.ModifiedOn = DateTime.UtcNow;
    }
  }
}
