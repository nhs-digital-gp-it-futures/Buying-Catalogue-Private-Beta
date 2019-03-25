using FluentAssertions;
using NHSD.GPITF.BuyingCatalog.Logic;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class FrameworksDatastore_Tests : DatastoreBase_Tests<FrameworksDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void GetAll_ReturnsData()
    {
      var datastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var datas = datastore.GetAll().ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(fw => Verifier.Verify(fw));
    }

    [Test]
    public void ById_UnknownId_ReturnsNull()
    {
      var datastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var data = datastore.ById(Guid.NewGuid().ToString());

      data.Should().BeNull();
    }

    [Test]
    public void ById_KnownId_ReturnsData()
    {
      var datastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);
      var allData = datastore.GetAll().ToList();

      var allDataById = allData.Select(data => datastore.ById(data.Id));

      allDataById.Should().BeEquivalentTo(allData);
    }

    [Test]
    public void ByCapability_UnknownId_ReturnsEmpty()
    {
      var datastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var frameworks = datastore.ByCapability(Guid.NewGuid().ToString());

      frameworks.Should().BeEmpty();
    }
  }
}
