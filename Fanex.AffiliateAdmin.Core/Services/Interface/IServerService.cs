// --------------------------------------------------------------------------
// <copyright file="IServerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.10.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;

    public interface IServerService
    {
        /// <summary>
        /// Get Affiliate System DateTime.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return System DateTime.</returns>
        SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria); 
    }
}