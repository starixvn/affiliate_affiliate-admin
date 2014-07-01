// --------------------------------------------------------------------------
// <copyright file="DomainAffCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Domain Affiliate Criteria.
    /// </summary>
    public class DomainAffCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
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
                    return "AffAdmin_DomainAffSel";
                case DatabaseAction.Insert:
                    return "AffAdmin_DomainAffIns";
                case DatabaseAction.Update:
                    return "AffAdmin_DomainAffUpd";
                case DatabaseAction.Delete:
                    return "AffAdmin_DomainAffDel";
                case DatabaseAction.Detail:
                    return "AffAdmin_DomainAffSelById";
            }

            return "AffAdmin_DomainAffSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }    
}
