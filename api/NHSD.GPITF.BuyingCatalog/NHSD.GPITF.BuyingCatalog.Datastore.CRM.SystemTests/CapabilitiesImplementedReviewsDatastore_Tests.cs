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
  public sealed class CapabilitiesImplementedReviewsDatastore_Tests : DatastoreBase_Tests<CapabilitiesImplementedReviewsDatastore>
  {
    [Test]
    public void Constructor_Completes()
    {
      Assert.DoesNotThrow(() => new CapabilitiesImplementedReviewsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config));
    }

    [Test]
    public void CRUD_Succeeds()
    {
      var contact = Retriever.GetAllContacts(_policy).First();
      var orgDatastore = new OrganisationsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<OrganisationsDatastore>>().Object, _policy, _config, new Mock<IDatastoreCache>().Object);
      var org = orgDatastore.ById(contact.OrganisationId);
      var solnDatastore = new SolutionsDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<SolutionsDatastore>>().Object, _policy, _config);
      var soln = solnDatastore.ByOrganisation(org.Id).First();
      var cap = Retriever.GetAllCapabilities(_policy).First();
      var claimDatastore = new CapabilitiesImplementedDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<CapabilitiesImplementedDatastore>>().Object, _policy, _config);
      var evidenceDatastore = new CapabilitiesImplementedEvidenceDatastore(DatastoreBaseSetup.CrmConnectionFactory, new Mock<ILogger<CapabilitiesImplementedEvidenceDatastore>>().Object, _policy, _config);
      var datastore = new CapabilitiesImplementedReviewsDatastore(DatastoreBaseSetup.CrmConnectionFactory, _logger, _policy, _config);

      var newClaim = Creator.GetCapabilitiesImplemented(solnId: soln.Id, claimId: cap.Id, ownerId: contact.Id);
      var createdClaim = claimDatastore.Create(newClaim);
      CapabilitiesImplementedReviews createdReview = null;

      try
      {
        var newEvidence = Creator.GetCapabilitiesImplementedEvidence(claimId: createdClaim.Id, createdById: contact.Id);
        var createdEvidence = evidenceDatastore.Create(newEvidence);

        // create
        var newReview = Creator.GetCapabilitiesImplementedReviews(evidenceId: createdEvidence.Id, createdById: contact.Id);
        createdReview = datastore.Create(newReview);

        createdReview.Should().BeEquivalentTo(newReview,
          opts => opts
            .Excluding(ent => ent.CreatedOn));

        // retrieve ById
        datastore.ById(createdReview.Id)
          .Should().NotBeNull()
          .And.Subject
          .Should().BeEquivalentTo(createdReview,
            opts => opts
              .Excluding(ent => ent.CreatedOn));

        // retrieve ByEvidence
        var retrievedReviews = datastore.ByEvidence(createdEvidence.Id)
          .SelectMany(x => x).ToList();
        retrievedReviews.Should().ContainSingle()
          .And.Subject.Single()
          .Should().BeEquivalentTo(createdReview,
            opts => opts
              .Excluding(ent => ent.CreatedOn));
      }
      finally
      {
        claimDatastore.Delete(createdClaim);
      }

      // delete
      datastore.ById(createdReview.Id)
        .Should().BeNull();
    }
  }
}
