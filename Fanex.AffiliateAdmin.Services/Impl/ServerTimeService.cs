// --------------------------------------------------------------------------
// <copyright file="ServerTimeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.1">Created</change>
// </history>

using System;

namespace Fanex.AffiliateAdmin.Services
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    public static class ServerTimeService
    {
        private static readonly IHomeService HomeService = new HomeService();
        /// <summary>
        /// Get System Date time.
        /// </summary>
        /// <param name="criteria">System date time criteria.</param>
        /// <returns>System date time.</returns>
        public static DateTime GetSystemDateTime()
        {
            var serverTime = HomeService.GetSystemDateTime(new SystemDateTimeCriteria());
            return serverTime != null ? serverTime.DatabaseDateTime : DateTime.Now;
        }
    }
}