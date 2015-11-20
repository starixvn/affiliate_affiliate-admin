// --------------------------------------------------------------------------
// <copyright file="MessageAddCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageAddCriteria : CriteriaBase
    {
        public bool Important { get; set; }

        public string Xml { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_HotNewsIns";
        }

        public override bool IsValid()
        {
            if (Xml == null)
            {
                return false;
            }

            return true;
        }
    }
}