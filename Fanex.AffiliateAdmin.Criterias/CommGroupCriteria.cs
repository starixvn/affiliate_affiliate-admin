// --------------------------------------------------------------------------
// <copyright file="CommGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission group criteria class.
    /// </summary>
    public class CommGroupCriteria : CriteriaBase
    {
        public string GroupName { get; set; }

        public string Currencies { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public string GroupIDList { get; set; }

        public int TotalNoneGroup { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; } 

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}