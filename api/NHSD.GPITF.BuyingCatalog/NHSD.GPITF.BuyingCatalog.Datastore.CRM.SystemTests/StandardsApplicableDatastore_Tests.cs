using FluentAssertions;
using NHSD.GPITF.BuyingCatalog.Logic;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class StandardsApplicableDatastore_Tests : DatastoreBase_Tests<StandardsApplicableDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void BySolution_ReturnsData()
    {
      var allSolns = Retriever.GetAllSolutions(_policy);
      var ids = allSolns.Select(soln => soln.Id).Distinct().ToList();
      var datastore = new StandardsApplicableDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var datas = ids.SelectMany(id => datastore.BySolution(id)).ToList();

      datas.Should().NotBeEmpty();
      datas.ForEach(data => data.Should().NotBeNull());
      datas.ForEach(data => Verifier.Verify(data));
    }

    [Test]
    public void CRUD_Succeeds()
    {
      var contact = Retriever.GetAllContacts(_policy).First();
      var soln = Retriever.GetAllSolutions(_policy).First();
      var std = Retriever.GetAllStandards(_policy).First();
      var datastore = new StandardsApplicableDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      // create
      var newEnt = Creator.GetStandardsApplicable(solnId:soln.Id, claimId:std.Id, ownerId: contact.Id);
      var createdEnt = datastore.Create(newEnt);

      try
      {
        createdEnt.Should().BeEquivalentTo(newEnt);

        // update
        createdEnt.Status = StandardsApplicableStatus.Submitted;
        datastore.Update(createdEnt);

        // retrieve
        datastore.ById(createdEnt.Id)
          .Should().BeEquivalentTo(createdEnt);
      }
      finally
      {
        // delete
        datastore.Delete(createdEnt);
      }

      // delete
      datastore.ById(createdEnt.Id)
        .Should().BeNull();
    }
  }
}
