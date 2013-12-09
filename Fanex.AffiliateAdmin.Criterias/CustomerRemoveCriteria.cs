// --------------------------------------------------------------------------
// <copyright file="CustomerRemoveCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.14">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Country Criteria.
    /// </summary>
    public class CustomerRemoveCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets Username.
        /// </summary>
        /// <value>Username of customer.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets Admin Username login.
        /// </summary>
        /// <value>Admin user id.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets Affiliate Type.
        /// </summary>
        /// <value>Admin user location address.</value>
        public string IP { get; set; }

        /// <summary>
        /// Gets or sets ErrCode.
        /// </summary>
        /// <value>Error Code.</value>
        public int ErrCode { get; set; }

        public long RefNo { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_MemberDel";
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