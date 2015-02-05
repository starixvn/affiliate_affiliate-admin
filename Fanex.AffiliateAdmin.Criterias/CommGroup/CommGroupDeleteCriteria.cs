// --------------------------------------------------------------------------
// <copyright file="CommGroupDeleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.20">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// COMMGroupAddCriteria class.
    /// </summary>
    public class CommGroupDeleteCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}