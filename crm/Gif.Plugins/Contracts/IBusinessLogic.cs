namespace Gif.Plugins.Contracts
{
    public interface IBusinessLogic
    {
        /// <summary>
        ///     Returns the trace log if not null
        /// </summary>
        /// <returns>The TraceLog if present</returns>
        string GetTraceInformation();

        /// <summary>
        /// Build Trace message to sue when raising an invalid plugin exception
        /// </summary>
        /// <param name="value"></param>
        void Trace(string value);
    }
}
