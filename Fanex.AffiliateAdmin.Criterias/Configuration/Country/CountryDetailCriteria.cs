// --------------------------------------------------------------------------
// <copyright file="CountryDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.Country
{
    using NAP.Framework.Data;

    /// <summary>
    /// CountryDetail Criteria class.
    /// </summary>
    public class CountryDetailCriteria : CriteriaBase
    {
        public string ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountrySelByID";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ID);
        }
    }
}