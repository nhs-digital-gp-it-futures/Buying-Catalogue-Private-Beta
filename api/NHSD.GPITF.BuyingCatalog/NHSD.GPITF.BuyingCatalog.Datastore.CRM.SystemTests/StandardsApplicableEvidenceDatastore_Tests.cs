using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NHSD.GPITF.BuyingCatalog.Interfaces;
using NHSD.GPITF.BuyingCatalog.Models;
using NHSD.GPITF.BuyingCatalog.Tests;
using NUnit.Framework;
using System.Linq;

namespace NHSD.GPITF.BuyingCatalog.Datastore.CRM.SystemTests
{
  [TestFixture]
  public sealed class StandardsApplicableEvidenceDatastore_Tests : DatastoreBase_Tests<StandardsApplicableEvidenceDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new StandardsApplicableEvidenceDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void CRUD_Succeeds()
    {
      var contact = Retriever.GetAllContacts(_policy).First();
      var orgDatastore = new OrganisationsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<OrganisationsDatastore>>().Object, _policy, _config, new Mock<IDatastoreCache>().Object);
      var org = orgDatastore.ById(contact.OrganisationId);
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var soln = solnDatastore.ByOrganisation(org.Id).First();
      var std = Retriever.GetAllStandards(_policy).First();
      var claimDatastore = new StandardsApplicableDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<StandardsApplicableDatastore>>().Object, _policy, _config);
      var datastore = new StandardsApplicableEvidenceDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var newClaim = Creator.GetStandardsApplicable(solnId: soln.Id, claimId:std.Id, ownerId: contact.Id);
      var createdClaim = claimDatastore.Create(newClaim);
      StandardsApplicableEvidence createdEvidence = null;

      try
      {
        // create
        var newEvidence = Creator.GetStandardsApplicableEvidence(claimId:createdClaim.Id, createdById: contact.Id);
        createdEvidence = datastore.Create(newEvidence);

        createdEvidence.Should().BeEquivalentTo(newEvidence,
          opts => opts
            .Excluding(ent => ent.CreatedOn));

        // retrieve ById
        datastore.ById(createdEvidence.Id)
          .Should().NotBeNull()
          .And.Subject
          .Should().BeEquivalentTo(createdEvidence,
            opts => opts
              .Excluding(ent => ent.CreatedOn));

        // retrieve ByClaim
        var retrievedClaims = datastore.ByClaim(createdClaim.Id)
          .SelectMany(x => x).ToList();
        retrievedClaims.Should().ContainSingle()
          .And.Subject.Single()
          .Should().BeEquivalentTo(createdEvidence,
            opts => opts
              .Excluding(ent => ent.CreatedOn));
      }
      finally
      {
        claimDatastore.Delete(createdClaim);
      }

      // delete
      datastore.ById(createdEvidence.Id)
        .Should().BeNull();
    }
  }
}
