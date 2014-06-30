// --------------------------------------------------------------------------
// <copyright file="ProductAddCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using System.Diagnostics.CodeAnalysis;
    using NAP.Framework.Data;

    /// <summary>
    /// ProductAdd Criteria class.
    /// </summary>
    public class ProductAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string BettypeList { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public int? API_ProductID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ProductName);
        }
    }
}