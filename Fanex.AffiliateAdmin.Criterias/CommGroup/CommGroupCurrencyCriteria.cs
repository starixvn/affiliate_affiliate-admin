// --------------------------------------------------------------------------
// <copyright file="CommGroupCurrencyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.3">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupCurrencyCriteria : CriteriaBase
    {
         public int UserId { get; set; }

         public string Currency { get; set; }

         public int LicenseeId { get; set; }

         public override string GetSettingKey()
         {
             return "AffAdmin_CommGroupSelListNormal";
         }

         public override bool IsValid()
         {
             return true;
         }
    }
}
