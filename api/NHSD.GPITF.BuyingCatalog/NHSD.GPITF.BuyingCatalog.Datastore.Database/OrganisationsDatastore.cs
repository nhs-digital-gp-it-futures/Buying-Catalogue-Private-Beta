using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database
{
  public sealed class OrganisationsDatastore : DatastoreBase<Organisations>, IOrganisationsDatastore
  {
    public OrganisationsDatastore(IDbConnectionFactory dbConnectionFactory, ILogger<OrganisationsDatastore> logger, ISyncPolicyFactory policy) :
      base(dbConnectionFactory, logger, policy)
    {
    }

    public Organisations ByContact(string contactId)
    {
      return GetInternal(() =>
      {
        const string sql = @"
select org.* from Organisations org
join Contacts cont on cont.OrganisationId = org.Id
where cont.Id = @contactId";
        var retval = _dbConnection.Value.Query<Organisations>(sql, new { contactId }).SingleOrDefault();
        return retval;
      });
    }

    public Organisations ById(string organisationId)
    {
      return GetInternal(() =>
      {
        return _dbConnection.Value.Get<Organisations>(organisationId);
      });
    }
  }
}
