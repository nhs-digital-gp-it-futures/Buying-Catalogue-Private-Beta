#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using Gif.Service.Attributes;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using Gif.Service.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Gif.Service.Services
{
  public class CapabilitiesImplementedReviewsService : ServiceBase<Review>, ICapabilitiesImplementedReviewsDatastore
  {
    public CapabilitiesImplementedReviewsService(IRepository repository) : base(repository)
    {
    }

    public IEnumerable<IEnumerable<Review>> ByEvidence(string evidenceId)
    {
      var reviewList = new List<Review>();
      var reviewsListList = new List<List<Review>>();

      // get all items at the end of the chain i.e. where the previous id is null
      var filterReviewParent = new List<CrmFilterAttribute>
      {
        new CrmFilterAttribute("EvidenceEntity") {FilterName = "_cc_evidence_value", FilterValue = evidenceId},
        new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
        new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
      };

      var jsonReviewParent = Repository.RetrieveMultiple(new Review().GetQueryString(null, filterReviewParent, true, true), out Count);

      // iterate through all items that are at the end of the chain
      foreach (var reviewChild in jsonReviewParent.Children())
        AddReviewChainToList(reviewChild, reviewList, reviewsListList);

      Count = reviewsListList.Count;

      return reviewsListList;
    }

    public IEnumerable<IEnumerable<Review>> ByEvidenceMultiple(List<Guid> evidenceIds)
    {
        var evidenceListList = new List<List<Review>>();

        // get all items at the end of the chain i.e. where the previous id is null
        var filterReviewParent = new List<CrmFilterAttribute>
        {
            new CrmFilterAttribute("Previous") {FilterName = "_cc_previousversion_value", FilterValue = "null"},
            new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
        };

        foreach (var evidence in evidenceIds)
        {
            filterReviewParent.Add(new CrmFilterAttribute("EvidenceId")
                { FilterName = "_cc_evidence_value", FilterValue = evidence.ToString(), MultiConditional = true });
        }

        var jsonEvidenceParent = Repository.RetrieveMultiple(new Review().GetQueryString(null, filterReviewParent, true, true), out Count);

        foreach (var reviewChild in jsonEvidenceParent.Children())
            AddReviewChainToList(reviewChild, new List<Review>(), evidenceListList);

        Count = evidenceListList.Count;

        return evidenceListList;
    }

    private void AddReviewChainToList(JToken review, List<Review> reviewList, List<List<Review>> reviewsListList)
    {
      GetReviewsChain(review, reviewList);

      var enumReviewList = Review.OrderLinkedReviews(reviewList);
      reviewsListList.Add(enumReviewList.ToList());
    }

    private void GetReviewsChain(JToken reviewChainEnd, List<Review> reviewList)
    {
      // store the end of the chain (with no previous id)
      var review = new Review(reviewChainEnd);
      reviewList.Add(review);
      var id = review.Id.ToString();

      // get the chain of reviews linked by previous id
      while (true)
      {
        var filterReview = new List<CrmFilterAttribute>
        {
          new CrmFilterAttribute("EvidenceEntity") {FilterName = "_cc_previousversion_value", FilterValue = id},
          new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
        };

        var jsonReview = Repository.RetrieveMultiple(new Review().GetQueryString(null, filterReview, true, true), out Count);
        if (jsonReview.HasValues)
        {
          review = new Review(jsonReview.FirstOrDefault());
          reviewList.Add(review);
          id = review.Id.ToString();
        }
        else
          break;
      }
    }

    public Review ById(string id)
    {
      var filterAttributes = new List<CrmFilterAttribute>
      {
        new CrmFilterAttribute("EvidenceId") {FilterName = "cc_reviewid", FilterValue = id},
        new CrmFilterAttribute("StateCode") {FilterName = "statecode", FilterValue = "0"}
      };

      var appJson = Repository.RetrieveMultiple(new Review().GetQueryString(null, filterAttributes), out Count);
      var review = appJson?.FirstOrDefault();

      return (review == null) ? null : new Review(review);
    }

    public Review Create(Review review)
    {
      Repository.CreateEntity(review.EntityName, review.SerializeToODataPost());

      return review;
    }

    public void Delete(Review review)
    {
      Repository.Delete(review.EntityName, review.Id);
    }
  }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
