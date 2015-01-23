// --------------------------------------------------------------------------
// <copyright file="PolicyUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.6.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyUpdateCriteria : CriteriaBase
    {
        public string MemPolicys { get; set; }

        public string GroupPolicys { get; set; }

        public short FuncId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            if (string.IsNullOrEmpty(this.MemPolicys))
            {
                return "AffAdmin_Con_GroupPolicyUpd";
            }

            return "AffAdmin_Con_PolicyUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
