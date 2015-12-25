// --------------------------------------------------------------------------
// <copyright file="HomeGeneralInfoCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Home General Information Criteria.
    /// </summary>
    public class HomeGeneralInfoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_HomeGeneralInfo";
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
