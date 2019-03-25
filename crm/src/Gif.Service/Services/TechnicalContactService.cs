using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class TechnicalContactService : ServiceBase<TechnicalContact>, ITechnicalContactsDatastore
  {
    public TechnicalContactService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<TechnicalContact> BySolution(string solutionId)
    {
      var contacts = new List<TechnicalContact>();

      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("Framework") {FilterName = "_cc_solution_value", FilterValue = solutionId},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new TechnicalContact().GetQueryString(null, filterAttributes, true, true), out Count
      );

      foreach (var contact in appJson.Children())
      {
        contacts.Add(new TechnicalContact(contact));
      }

      Count = contacts.Count();

      return contacts;
    }

    public TechnicalContact ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
            {
                new CrmFilterAttribute("SolutionId") {FilterName = "cc_technicalcontactid", FilterValue = id},
                new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
            };

      var appJson = Repository.RetrieveMultiple(new TechnicalContact().GetQueryString(null, filterAttributes), out int? count);
      var tcJson = appJson?.FirstOrDefault();

      return (tcJson == null) ? null : new TechnicalContact(tcJson);
    }

    public TechnicalContact Create(TechnicalContact techCont)
    {
      Repository.CreateEntity(techCont.EntityName, techCont.SerializeToODataPost());
      return techCont;
    }

    public void Update(TechnicalContact techCont)
    {
      Repository.UpdateEntity(techCont.EntityName, techCont.Id, techCont.SerializeToODataPut("cc_technicalcontactid"));
    }

    public void Delete(TechnicalContact techCont)
    {
      Repository.Delete(techCont.EntityName, techCont.Id);
    }

  }
}
