// --------------------------------------------------------------------------
// <copyright file="SystemDateTimeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.07.10">Created</change>
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
    public class SystemDateTimeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSelDBTime";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}