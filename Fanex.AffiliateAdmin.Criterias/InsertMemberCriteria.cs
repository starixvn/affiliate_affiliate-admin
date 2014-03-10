// --------------------------------------------------------------------------
// <copyright file="InsertMemberCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.10.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InsertMemberCriteria : CriteriaBase
    {
        public string AffUserName { get; set; }

        public string Username { get; set; }

        public string DepositCustID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int IBCCustID { get; set; }

        public string IBCUserName { get; set; }

        public int AgentID { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? RegisterDate { get; set; }

        public DateTime? AffAffectedDate { get; set; }

        public string Btag { get; set; }

        public long ClickID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int AffId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CustomerInsMemberManually";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
