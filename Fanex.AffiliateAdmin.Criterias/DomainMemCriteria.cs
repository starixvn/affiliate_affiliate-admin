// --------------------------------------------------------------------------
// <copyright file="DomainMemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Domain Member site Criteria.
    /// </summary>
    public class DomainMemCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

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
                    return "AffAdmin_DomainMemSel";
                case DatabaseAction.Insert:
                    return "AffAdmin_DomainMemIns";
                case DatabaseAction.Update:
                    return "AffAdmin_DomainMemUpd";
                case DatabaseAction.Delete:
                    return "AffAdmin_DomainMemDel";
                case DatabaseAction.Detail:
                    return "AffAdmin_DomainMemSelById";
            }

            return "AffAdmin_DomainMemSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }    
}
