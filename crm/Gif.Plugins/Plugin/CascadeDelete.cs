namespace Gif.Plugins.Plugin
{
    #region

    using Business_Logic;
    using Microsoft.Xrm.Sdk;
    using Repositories;
    using System;

    #endregion

    /// <summary>
    ///     cascade delete all associated child records when a standard applicable record is deleted.
    /// </summary>
    public class CascadeDelete : IPlugin
    {
        #region IPlugin

        /// <summary>
        /// plugin execution
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void Execute(IServiceProvider serviceProvider)
        {
            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService?.Trace($"{PluginName} started.");

            var target = (EntityReference)context.InputParameters["Target"];
            if (target == null)
                return;

            tracingService?.Trace($"Target id : {target.Id}");

            var repository = new SolutionRepository(service);
            var logic = new CascadeDeleteLogic(repository, PluginName);

            try
            {
                if (context.MessageName.Equals("Delete"))
                {
                    tracingService?.Trace($"Message Name: {context.MessageName}");
                    tracingService?.Trace($"Primary Entity Name: {context.PrimaryEntityName.Trim().ToLower()}");
                    switch (context.PrimaryEntityName.Trim().ToLower())
                    {
                        case "cc_solution":
                            logic.OnSolutionDelete(target);
                            break;
                        case "cc_standardapplicable":
                            logic.OnStandardApplicableDelete(target);
                            break;
                        case "cc_capabilityimplemented":
                            logic.OnCapabilityImplementedDelete(target);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                var traceLogException = logic.GetTraceInformation();
                throw new InvalidPluginExecutionException($"An error occurred in the {PluginName} plugin: {exception.Message} Trace: {traceLogException}", exception);
            }

            var traceLog = logic.GetTraceInformation();
            tracingService?.Trace($"Successful trace log: {traceLog}");
            tracingService?.Trace($"{PluginName} finished.");
        }

        #endregion

        #region Fields

        /// <summary>
        ///     The plugin full name.
        /// </summary>
        private const string PluginName = "Gif.Plugins.Plugin.CascadeDelete";

        #endregion
    }
}
