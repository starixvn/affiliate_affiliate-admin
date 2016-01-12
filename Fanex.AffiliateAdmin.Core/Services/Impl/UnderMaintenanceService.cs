// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.05">Created</change>
//   <change who="Johnny.Trinh" date="2013.07.03">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Caching;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Under Maintenance Service.
    /// </summary>
    public static class UnderMaintenanceService
    {
        /// <summary>
        /// Time Cache in Seconds.
        /// </summary>
        private const int _TimeCacheUM = 30;

        /// <summary>
        /// Cache maintenance.
        /// </summary>
        private static MemoryCache _memoryCache = MemoryCache.Default;

        /// <summary>
        /// Gets Down time.
        /// </summary>
        /// <param name="startTime">Start Time of Down time.</param>
        /// <param name="endTime">End Time of Down time.</param>
        /// <returns>Return down time.</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Accept type reference")]
        public static bool GetDownTime(ref string startTime, ref string endTime)
        {
            startTime = endTime = null;
            DateTime now = DateTime.Now;

            if (AppSettings.SkipCheckUMTime)
            {
                return false;
            }

            if (now >= AppSettings.UMStartTime && now <= AppSettings.UMEndTime)
            {
                startTime = AppSettings.UMStartTime.ToString("hh:mm tt");
                endTime = AppSettings.UMEndTime.ToString("hh:mm tt");
                return true;
            }

            if (AppSettings.LBUrl == null)
            {
                return false;
            }

            var maintenanceController = new UMBusiness();

            bool underMaintenance = false;

            if (_memoryCache["IsUM"] == null)
            {
                underMaintenance = maintenanceController.isUnderMaintainence(Sites.Affiliate, AppSettings.LBUrl.ToString(), ref startTime, ref endTime);
                _memoryCache.Add("IsUM", underMaintenance, DateTime.Now.AddSeconds(_TimeCacheUM));

                if (string.IsNullOrWhiteSpace(startTime))
                {
                    Logging.LogService.Error("UM service has problems!");
                    return false;
                }

                _memoryCache.Add("StartTime", startTime, DateTime.Now.AddSeconds(_TimeCacheUM));
                _memoryCache.Add("EndTime", endTime, DateTime.Now.AddSeconds(_TimeCacheUM));
            }
            else
            {
                underMaintenance = Convert.ToBoolean(_memoryCache["IsUM"]);
                startTime = Convert.ToString(_memoryCache["StartTime"]);
                endTime = Convert.ToString(_memoryCache["EndTime"]);
            }

            return underMaintenance;
        }
    }
}