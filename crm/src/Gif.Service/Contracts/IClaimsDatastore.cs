using System.Collections.Generic;

namespace Gif.Service.Contracts
{
#pragma warning disable CS1591
    public interface IClaimsDatastore<T> where T : ClaimsBase
    {
        T ById(string id);
        IEnumerable<T> BySolution(string solutionId);
        T Create(T claim);
        void Update(T claim);
        void Delete(T claim);
    }
#pragma warning restore CS1591
}
