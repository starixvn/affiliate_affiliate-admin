// --------------------------------------------------------------------------
// <copyright file="ProductSelectCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Product
{
    using Fanex.Data.Repository;

    /// <summary>
    /// ProductSelect Criteria class.
    /// </summary>
    public class ProductSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ProductSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}