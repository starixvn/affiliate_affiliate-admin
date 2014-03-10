// --------------------------------------------------------------------------
// <copyright file="HomeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.11">Implement funcion get Current Commission Setting</change>
// </history>
namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Interface;
    using Models;

    /// <summary>
    /// Account Service.
    /// </summary>
    public class HomeService : QueryService, IHomeService
    {
        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List home view model.</returns>
        public IEnumerable<HomeViewModel> GeneralInfoSel(HomeGeneralInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<HomeViewModel>(criteria);
        }

        /// <summary>
        /// User online list.
        /// </summary>
        /// <param name="criteria">Home online criteria.</param>
        /// <returns>List user online.</returns>
        public IEnumerable<UserOnline> UserOnlineList(HomeOnlineListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<UserOnline>(criteria);
        }

        /// <summary>
        /// Get System Date time.
        /// </summary>
        /// <param name="criteria">System date time criteria.</param>
        /// <returns>System date time.</returns>
        public SystemDateTime GetSystemDateTime(SystemDateTimeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var list = GetData<SystemDateTime>(criteria);
            return list.FirstOrDefault();
        }
    }
}