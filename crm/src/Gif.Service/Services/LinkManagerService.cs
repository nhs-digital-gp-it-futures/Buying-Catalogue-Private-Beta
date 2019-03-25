using Gif.Service.Const;
using Gif.Service.Contracts;
using Gif.Service.Crm;
using System;

namespace Gif.Service.Services
{
  public class LinkManagerService : ServiceBase<object>, ILinkManagerDatastore
  {
    public LinkManagerService(IRepository repository) : base(repository)
    {
    }

    public void FrameworkSolutionAssociate(Guid frameworkId, Guid solutionId)
    {
      Repository.Associate(frameworkId, "cc_frameworks", solutionId, "cc_solutions", RelationshipNames.SolutionFramework);
    }
  }
}
