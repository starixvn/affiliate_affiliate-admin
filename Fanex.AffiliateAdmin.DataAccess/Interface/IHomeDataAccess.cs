// --------------------------------------------------------------------------
// <copyright file="IHomeDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.02">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess.Interface
{
    using Criterias;
    using Models;

    /// <summary>
    /// Interface Account.
    /// </summary>
    public interface IHomeDataAccess
    {
        /// <summary>
        /// Select win lost, bonus, number of banners, affiliate,.....
        /// </summary>
        /// <param name="criteria">Home Info criteria.</param>
        /// <returns>Return Home View Model.</returns>
        HomeViewModel HomeGeneralInfoSel(HomeGeneralInfoCriteria criteria);
    }
}