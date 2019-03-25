using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class TechnicalContactsDatastore_Tests : DatastoreBase_Tests<TechnicalContactsDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new TechnicalContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void BySolution_ReturnsData()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var allSolns = frameworks.SelectMany(fw => solnDatastore.ByFramework(fw.Id)).ToList();
      var datastore = new TechnicalContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var datas = allSolns.SelectMany(soln => datastore.BySolution(soln.Id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void CRUD_Succeeds()
    {
      var frameworksDatastore = new FrameworksDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<FrameworksDatastore>>().Object, _policy, _config);
      var frameworks = frameworksDatastore.GetAll().ToList();
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var soln = frameworks.SelectMany(fw => solnDatastore.ByFramework(fw.Id)).First();
      var datastore = new TechnicalContactsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      // create
      var newEnt = Creator.GetTechnicalContact(solutionId: soln.Id, contactType: "Lead Contact", emailAddress: "steve.gray@nhs.net.uk");
      newEnt.FirstName = "Steve";
      newEnt.LastName = "Gray";
      newEnt.PhoneNumber = "1234567890";
      var createdEnt = datastore.Create(newEnt);

      try
      {
        createdEnt.Should().BeEquivalentTo(newEnt);

        // update
        createdEnt.FirstName = "Jon";
        createdEnt.LastName = "Dough";
        datastore.Update(createdEnt);
        datastore.BySolution(soln.Id).Single(ent => ent.Id == createdEnt.Id)
          .Should().BeEquivalentTo(createdEnt);
      }
      finally
      {
        // delete
        datastore.Delete(createdEnt);
      }

      // delete
      datastore.BySolution(soln.Id)
        .Should().NotContain(ent => ent.Id == createdEnt.Id);
    }
  }
}
