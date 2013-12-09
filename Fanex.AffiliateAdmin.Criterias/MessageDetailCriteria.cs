// --------------------------------------------------------------------------
// <copyright file="MessageDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageDetailCriteria : CriteriaBase
    {
        public int? MsgId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}