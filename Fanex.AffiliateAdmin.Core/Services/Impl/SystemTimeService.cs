namespace Fanex.AffiliateAdmin.Services
{
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using System;

    /// <summary>
    /// System Time Service.
    /// </summary>
    public static class SystemTimeService
    {
        private static readonly IServerService ServerService = new ServiceService();

        /// <summary>
        /// Get System Date time.
        /// </summary>
        /// <returns>System date time.</returns>
        public static DateTime GetSystemDateTime()
        {
            var serverTime = ServerService.GetSystemDateTime(new SystemDateTimeCriteria());

            return serverTime != null ? serverTime.DatabaseDateTime : DateTime.Now;
        }
    }
}