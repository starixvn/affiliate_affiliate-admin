namespace Fanex.AffiliateAdmin.Core.Services
{
    using System;
    using Fanex.Logging;

    /// <summary>
    /// Service Base class.
    /// </summary>
    public class ServiceBase
    {
        protected void HandleException(Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }
    }
}