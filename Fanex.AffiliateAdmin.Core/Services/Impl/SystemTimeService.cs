// --------------------------------------------------------------------------
// <copyright file="SystemTimeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.1">Created</change>
// </history>

// --------------------------------------------------------------------------
// <copyright file="SystemTimeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;


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