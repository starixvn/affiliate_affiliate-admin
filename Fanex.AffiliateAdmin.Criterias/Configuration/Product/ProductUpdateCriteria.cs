// --------------------------------------------------------------------------
// <copyright file="ProductUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// ProductUpdate Criteria class.
    /// </summary>
    public class ProductUpdateCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public byte ProductID { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string BettypeList { get; set; }

        public int? API_ProductID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductUpd";
        }

        public override bool IsValid()
        {
            return ProductID > 0;
        }
    }
}