// --------------------------------------------------------------------------
// <copyright file="CommGroupDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission group detail criteria class.
    /// </summary>
    public class CommGroupDetailCriteria : CriteriaBase
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        public string GroupIDList { get; set; }

        public DateTime? Date { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupDetailSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}