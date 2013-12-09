// --------------------------------------------------------------------------
// <copyright file="LoginCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Login Criteria.
    /// </summary>
    public class LoginCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets Customer ID.
        /// </summary>
        /// <value>Customer ID.</value>
        public int CustID { get; set; }

        /// <summary>
        /// Gets or sets Username.
        /// </summary>
        /// <value>Username Admin.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        /// <value>User password Admin.</value>
        public string UserPwd { get; set; }

        /// <summary>
        /// Gets or sets LoginIP.
        /// </summary>
        /// <value>Admin IP Login.</value>
        public string LoginIP { get; set; }

        /// <summary>
        /// Gets or sets NetSessionID.
        /// </summary>
        /// <value>Net Session ID.</value>
        public string NetSessionID { get; set; }

        /// <summary>
        /// Gets or sets OnUserID.
        /// </summary>
        /// <value>On User ID.</value>
        public string OnUserID { get; set; }

        /// <summary>
        /// Gets or sets ErrCode.
        /// </summary>
        /// <value>Error Code.</value>
        public int ErrCode { get; set; }
        
        /// <summary>
        /// Gets or sets From.
        /// </summary>
        /// <value>From system.</value>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the IT is enabled.
        /// </summary>
        /// <value>Flag is it or not.</value>
        public bool IsIT { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_Login";
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
