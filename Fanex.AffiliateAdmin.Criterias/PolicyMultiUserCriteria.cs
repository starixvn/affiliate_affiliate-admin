// -----------------------------------------------------------------------
// <copyright file="PolicyMultiUserCriteria.cs" company="Microsoft">
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
    public class PolicyMultiUserCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_Batch_PolicySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
