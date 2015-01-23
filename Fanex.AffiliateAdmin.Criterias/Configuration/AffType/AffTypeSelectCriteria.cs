// --------------------------------------------------------------------------
// <copyright file="AffTypeSelectCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Select criteria class.
    /// </summary>
    public class AffTypeSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}