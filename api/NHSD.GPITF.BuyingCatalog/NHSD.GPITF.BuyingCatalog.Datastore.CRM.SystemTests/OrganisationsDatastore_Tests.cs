using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NUnit.Framework;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class OrganisationsDatastore_Tests : DatastoreBase_Tests<OrganisationsDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new OrganisationsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object));
    }

    [Test]
    public void ById_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var allSolns = frameworks.SelectMany(fw => solnDatastore.ByFramework(fw.Id));
      var ids = allSolns.Select(soln => soln.OrganisationId).Distinct().ToList();

      var datastore = new OrganisationsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object);

      var datas = ids.Select(id => datastore.ById(id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void ByContact_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var allSolns = frameworks.SelectMany(fw => solnDatastore.ByFramework(fw.Id));
      var allOrgIds = allSolns.Select(soln => soln.OrganisationId).Distinct().ToList();
      var contactsDatastore = new ContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<ContactsDatastore>>().Object, _policy, _config, new Mock<IDatastoreCache>().Object);
      var allContacts = allOrgIds.SelectMany(orgId => contactsDatastore.ByOrganisation(orgId)).ToList();
      var datastore = new OrganisationsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config, new Mock<IDatastoreCache>().Object);

      var datas = allContacts.Select(contact => datastore.ByContact(contact.Id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }
  }
}
