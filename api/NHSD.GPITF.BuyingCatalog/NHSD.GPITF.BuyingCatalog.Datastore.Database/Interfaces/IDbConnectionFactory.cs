using System.Data;

namespace NHSD.GPITF.BuyingCatalog.Datastore.Database.Interfaces
{
  public interface IDbConnectionFactory
  {
    IDbConnection Get();
  }
}
