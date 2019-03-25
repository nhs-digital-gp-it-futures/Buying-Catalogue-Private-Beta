using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace Gif.Plugins.Contracts
{
    /// <summary>
    /// Base Repository interface
    /// </summary>
    public interface IRepository
    {
        string GetOptionsetText(string entityName, string attributeName, int optionSetValue);
        OptionMetadata[] GetOptionSetList();
        void Update(Entity entity);
        void Upsert(Entity entity);
        Entity Create(Entity entity);
        void SvcCreate(Entity entity);
        void Delete(Entity entity);
        string GetTraceInformation();
        void Trace(string value);
        IOrganizationService GetService();
    }
}
