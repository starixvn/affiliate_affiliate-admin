// --------------------------------------------------------------------------
// <copyright file="ProductDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.6">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductDetail Criteria class.
    /// </summary>
    public class ProductDetailCriteria : CriteriaBase
    {
        public byte ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductSelByID";
        }

        public override bool IsValid()
        {
            return ID > 0;
        }
    }
}