using FluentAssertions;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NUnit.Framework;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class ContactsDatastore_Tests : DatastoreBase_Tests<ContactsDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new ContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object));
    }

    [Test]
    public void ByEmail_ReturnsData()
    {
      var emails = Retriever.GetAllContacts(_policy).Select(ent => ent.EmailAddress1).ToList();
      var datastore = new ContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object);

      var datas = emails.Select(email => datastore.ByEmail(email)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void ById_ReturnsData()
    {
      var ids = Retriever.GetAllContacts(_policy).Select(ent => ent.Id);
      var datastore = new ContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object);

      var datas = ids.Select(id => datastore.ById(id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void ByOrganisation_ReturnsData()
    {
      var datas = Retriever.GetAllContacts(_policy);

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }
  }
}
