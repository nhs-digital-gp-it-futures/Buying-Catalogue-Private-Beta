using System;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
  public interface ILinkManagerDatastore
  {
    void FrameworkSolutionAssociate(Guid frameworkId, Guid solutionId);
  }
#pragma warning restore CS1591
}
