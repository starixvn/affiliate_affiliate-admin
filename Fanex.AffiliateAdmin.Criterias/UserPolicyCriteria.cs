// --------------------------------------------------------------------------
// <copyright file="UserPolicyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserPolicyCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets Customer ID.
        /// </summary>
        /// <value>Customer ID.</value>
        public int CustID { get; set; }

        /// <summary>
        /// Gets or sets GroupID.
        /// </summary>
        /// <value>Group ID of user.</value>
        public int GroupID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_AdminPolicySelByCustID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}