using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class SolutionsDatastore_Tests : DatastoreBase_Tests<SolutionsDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void ByFramework_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var datas = frameworks.SelectMany(fw => datastore.ByFramework(fw.Id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void ById_UnknownId_ReturnsNull()
    {
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var data = datastore.ById(Guid.NewGuid().ToString());

      data.Should().BeNull();
    }

    [Test]
    public void ById_KnownId_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);
      var allData = frameworks.SelectMany(fw => datastore.ByFramework(fw.Id));

      var allDataById = allData.Select(data => datastore.ById(data.Id));

      allDataById.Should().BeEquivalentTo(allData);
    }

    [Test]
    public void ByOrganisation_UnknownId_ReturnsEmpty()
    {
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var data = datastore.ByOrganisation(Guid.NewGuid().ToString());

      data.Should().BeEmpty();
    }

    [Test]
    public void ByOrganisation_KnownId_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);
      var orgIds = frameworks
        .SelectMany(fw => datastore.ByFramework(fw.Id))
        .Select(soln => soln.OrganisationId)
        .Distinct();

      var allDataByOrg = orgIds.SelectMany(orgId => datastore.ByOrganisation(orgId)).ToList();

      allDataByOrg.Should().NotBeEmpty();
      allDataByOrg.ForEach(soln => Verifier.Verify(soln));
    }

    [Test]
    public void CRUD_Succeeds()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var datastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);
      var orgId = frameworks
        .SelectMany(fw => datastore.ByFramework(fw.Id))
        .Select(soln => soln.OrganisationId)
        .First();
      var contactsDatastore = new ContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<ContactsDatastore>>().Object, _policy, _config, new Mock<IDatastoreCache>().Object);
      var contactId = contactsDatastore.ByOrganisation(orgId).First().Id;

      // create
      var newEnt = Creator.GetSolution(orgId: orgId, createdById: contactId, modifiedById: contactId);
      newEnt.Name = "My New Solution";
      var createdEnt = datastore.Create(newEnt);

      try
      {
        createdEnt.Should().BeEquivalentTo(newEnt,
          opts => opts
            .Excluding(ent => ent.CreatedOn)
            .Excluding(ent => ent.ModifiedOn));

        // retrieve
        var retrievedEnt = datastore.ById(createdEnt.Id);
        retrievedEnt.Should().BeEquivalentTo(createdEnt,
          opts => opts
            .Excluding(ent => ent.CreatedOn)
            .Excluding(ent => ent.ModifiedOn));

        // update
        createdEnt.Name = "My Other New Solution";
        datastore.Update(createdEnt);
        var updatedEnt = datastore.ById(createdEnt.Id);
        updatedEnt.Should().BeEquivalentTo(createdEnt,
          opts => opts
            .Excluding(ent => ent.CreatedOn)
            .Excluding(ent => ent.ModifiedOn));
      }
      finally
      {
        // delete
        datastore.Delete(createdEnt);
      }

      // delete
      var deletedEnt = datastore.ById(createdEnt.Id);
      deletedEnt.Should().BeNull();
    }
  }
}
