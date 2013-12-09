// --------------------------------------------------------------------------
// <copyright file="CommGroupAssignCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.8">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission Group Assign Criteria.
    /// </summary>
    public class CommGroupAssignCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        [SpParam(SqlParamName = "AffIDList")]
        public string AffIds { get; set; }

        [SpParam(Ignored = true)]
        public string Usernames { get; set; }

        [SpParam(Ignored = true)]
        public string Currency { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupAssign";
        }

        public override bool IsValid()
        {
            return GroupId > 0 && !string.IsNullOrEmpty(AffIds);
        }
    }
}