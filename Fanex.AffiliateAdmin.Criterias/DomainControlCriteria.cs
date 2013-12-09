// --------------------------------------------------------------------------
// <copyright file="DomainControlCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.9.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Domain Control Criteria.
    /// </summary>
    public class DomainControlCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public int AffDomainID { get; set; }

        public int MemberDomainID { get; set; }

        public string CountryID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public int Creator { get; set; }

        public bool Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        [SpParam(Ignored = true)]
        public DatabaseAction DBAction { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            switch (DBAction)
            {
                case DatabaseAction.Select:
                    return "AffAdmin_DomainControlSel";
                case DatabaseAction.Insert:
                    return "AffAdmin_DomainControlIns";
                case DatabaseAction.Update:
                    return "AffAdmin_DomainControlUpd";
                case DatabaseAction.Delete:
                    return "AffAdmin_DomainControlDel";
                case DatabaseAction.Detail:
                    return "AffAdmin_DomainControlSelById";
            }

            return "AffAdmin_DomainControlSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
