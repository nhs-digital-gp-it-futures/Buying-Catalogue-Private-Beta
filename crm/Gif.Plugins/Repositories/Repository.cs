using Gif.Plugins.Contracts;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gif.Plugins.Repositories
{
    /// <summary>
    /// Base Repository
    /// </summary>
    public class Repository : IRepository
    {
        /// <summary>
        /// Svc
        /// </summary>
        protected IOrganizationService Svc;

        /// <summary>
        /// Base Repository constructor
        /// </summary>
        /// <param name="svc"></param>
        public Repository(IOrganizationService svc)
        {
            Svc = svc;
        }

        /// <summary>
        /// Get Label for Optionset Value
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="attributeName"></param>
        /// <param name="optionSetValue"></param>
        /// <returns></returns>
        public string GetOptionsetText(string entityName, string attributeName, int optionSetValue)
        {
            var optionsetLabel = string.Empty;

            var retrieveDetails = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.All,
                LogicalName = entityName
            };

            var retrieveEntityResponseObj = (RetrieveEntityResponse)Svc.Execute(retrieveDetails);
            if (retrieveEntityResponseObj.EntityMetadata == null)
                return optionsetLabel;

            var metadata = retrieveEntityResponseObj.EntityMetadata;
            if (metadata.Attributes == null)
                return optionsetLabel;

            var picklistMetadata = metadata.Attributes.FirstOrDefault(attribute => String.Equals(attribute.LogicalName, attributeName, StringComparison.OrdinalIgnoreCase)) as PicklistAttributeMetadata;
            if (picklistMetadata?.OptionSet == null)
                return optionsetLabel;

            var options = picklistMetadata.OptionSet;
            IList<OptionMetadata> optionsList = (from o in options.Options where o.Value != null && o.Value.Value == optionSetValue select o).ToList();

            var label = (optionsList.First()).Label;
            if (label?.UserLocalizedLabel != null)
                optionsetLabel = label.UserLocalizedLabel.Label;

            return optionsetLabel;
        }

        /// <summary>
        /// Get Optionset List Metadata
        /// </summary>
        /// <returns></returns>
        public OptionMetadata[] GetOptionSetList()
        {
            OptionMetadata[] optionList = null;

            RetrieveOptionSetRequest retrieveOptionSetRequest = new RetrieveOptionSetRequest
            {
                Name = "processstage_category"
            };

            RetrieveOptionSetResponse retrieveOptionSetResponse = (RetrieveOptionSetResponse)Svc.Execute(retrieveOptionSetRequest);

            if (retrieveOptionSetResponse != null)
            {
                OptionSetMetadata retrievedOptionSetMetadata = (OptionSetMetadata)retrieveOptionSetResponse.OptionSetMetadata;
                if (retrievedOptionSetMetadata != null)
                    optionList = retrievedOptionSetMetadata.Options.ToArray();
            }

            return optionList;
        }

        /// <summary>
        /// Creates a new entity and returns the result
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Entity Create(Entity entity)
        {
            using (var ctx = new OrganizationServiceContext(Svc))
            {
                ctx.AddObject(entity);
                ctx.SaveChanges();
                return entity;
            }
        }

        /// <summary>
        /// Create entity using OrganisationService
        /// </summary>
        /// <param name="entity"></param>
        public void SvcCreate(Entity entity)
        {
            Svc.Create(entity);
        }

        public void Delete(Entity entity)
        {
            Svc.Delete(entity.LogicalName, entity.Id);
        }

        /// <summary>
        /// Updates an existing entity, attaching to the context if required
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Entity entity)
        {
            using (var ctx = new OrganizationServiceContext(Svc))
            {
                if (!ctx.IsAttached(entity))
                    ctx.Attach(entity);

                ctx.UpdateObject(entity);
                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Upserts a entity
        /// </summary>
        /// <param name="entity"></param>
        public void Upsert(Entity entity)
        {
            if (entity.Id == Guid.Empty)
                Create(entity);
            else
                Update(entity);
        }

        /// <summary>
        /// Trace info
        /// </summary>
        public StringBuilder TraceLog { get; protected set; }

        /// <summary>
        ///     Returns the trace log if not null
        /// </summary>
        /// <returns>The TraceLog if present</returns>
        public string GetTraceInformation()
        {
            return TraceLog.ToString();
        }

        /// <summary>
        /// Build Trace message to sue when raising an invalid plugin exception
        /// </summary>
        /// <param name="value"></param>
        public virtual void Trace(string value)
        {
            if (TraceLog == null)
            {
                TraceLog = new StringBuilder();
            }
            else
            {
                TraceLog.AppendLine();
            }

            TraceLog.Append(value);
            TraceLog.Append(" - ");
            TraceLog.Append(DateTime.Now.ToString("hh.mm.ss.ffffff"));
        }

        /// <summary>
        /// Get Service
        /// </summary>
        /// <returns></returns>
        public IOrganizationService GetService()
        {
            return Svc;
        }
    }
}
