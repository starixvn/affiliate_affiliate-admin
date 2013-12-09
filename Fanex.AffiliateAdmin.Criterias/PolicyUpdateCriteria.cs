// -----------------------------------------------------------------------
// <copyright file="PolicyUpdateCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

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
