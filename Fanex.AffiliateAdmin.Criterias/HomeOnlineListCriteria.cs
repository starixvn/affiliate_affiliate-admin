// --------------------------------------------------------------------------
// <copyright file="HomeOnlineListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Home Online List Criteria.
    /// </summary>
    public class HomeOnlineListCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int NRows { get; set; }

        public bool IsIT { get; set; }

        public int LicenseeId { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_UserOnlineList";
        }

        /// <summary>
        /// Validation of criteria.
        /// </summary>
        /// <returns>Validate criteria.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
