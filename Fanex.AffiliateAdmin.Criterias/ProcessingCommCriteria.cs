// --------------------------------------------------------------------------
// <copyright file="ProcessingCommCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Commission group criteria class.
    /// </summary>
    public class ProcessingCommCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string AffName { get; set; }

        public string AffTypes { get; set; }

        public string CommGroups { get; set; }

        public string PreferLanguages { get; set; }

        public string Currencies { get; set; }

        public byte BaseCurrency { get; set; }

        public byte SortBy { get; set; }

        public bool ExcludeTest { get; set; }

        public bool IsITAdmin { get; set; }

        [SpParam(SqlParamName = "ProductIds")]
        public string Products { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommissionSettle";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}