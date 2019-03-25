using Gif.Plugins.Business_Logic;
using Gif.Plugins.Repositories;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Gif.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = CrmHelper.GetService();

            var sol = svc.Retrieve("cc_standardapplicable", Guid.Parse("460779EE-DBF3-E811-A96D-0022480130E2"), new ColumnSet(true));
            var logic = new CascadeDeleteLogic(new SolutionRepository(svc), "");
            logic.OnStandardApplicableDelete(sol.ToEntityReference());
        }
    }
}
