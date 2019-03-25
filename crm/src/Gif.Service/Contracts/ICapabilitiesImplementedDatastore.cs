using Gif.Service.Models;
using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
    public interface ICapabilitiesImplementedDatastore : IDatastoreBase<CapabilityImplemented>
    {
        CapabilityImplemented ById(string id);
        IEnumerable<CapabilityImplemented> BySolution(string solutionId);
        CapabilityImplemented Create(CapabilityImplemented capabilityImplemented);
        void Update(CapabilityImplemented capabilityImplemented);
        void Delete(CapabilityImplemented capabilityImplemented);
    }
#pragma warning restore CS1591
}
