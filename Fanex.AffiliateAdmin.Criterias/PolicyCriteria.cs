// -----------------------------------------------------------------------
// <copyright file="PolicyCriteria.cs" company="Microsoft">
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
    public class PolicyCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public int GroupId { get; set; }

        public bool IsUser { get; set; }

        public override string GetSettingKey()
        {
            if (this.IsUser)
            {
                return "AffAdmin_Con_PolicySelByCustID";
            }

            return "AffAdmin_Con_GroupPolicySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
