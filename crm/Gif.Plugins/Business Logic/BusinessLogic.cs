using Gif.Plugins.Contracts;
using System;
using System.Text;

namespace Gif.Plugins.Business_Logic
{
    /// <summary>
    /// Base Class for plugin business logic
    /// </summary>
    public class BusinessLogic : IBusinessLogic
    {
        #region Properties

        /// <summary>
        /// Trace info
        /// </summary>
        public StringBuilder TraceLog { get; protected set; }
        /// <summary>
        /// Plugin Name
        /// </summary>
        public string PluginName { get; set; }

        #endregion

        /// <summary>
        ///     Returns the trace log if not null
        /// </summary>
        /// <returns>The TraceLog if present</returns>
        public string GetTraceInformation()
        {
            return TraceLog != null ? TraceLog.ToString() : String.Empty;
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

    }
}
