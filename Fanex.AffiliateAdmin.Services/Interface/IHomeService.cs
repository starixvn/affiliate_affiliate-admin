// --------------------------------------------------------------------------
// <copyright file="IHomeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.11">Implement funcion get Current Commission Setting</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface Account service.
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Get general information (win lost, bonus, pending affiliate,....).
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List home view model.</returns>
        HomeViewModel GeneralInfoSel(HomeGeneralInfoCriteria criteria);

        /// <summary>
        /// Get user online list.
        /// </summary>
        /// <param name="criteria">Home online criteria.</param>
        /// <returns>List user online.</returns>
        IEnumerable<UserOnline> UserOnlineList(HomeOnlineListCriteria criteria);

        /// <summary>
        /// Get Affiliate System DateTime.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return System DateTime.</returns>
        SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria);
    }
}