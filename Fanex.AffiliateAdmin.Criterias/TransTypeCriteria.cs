// -----------------------------------------------------------------------
// <copyright file="TransTypeCriteria.cs" company="Microsoft">
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
    public class TransTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_TransTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
