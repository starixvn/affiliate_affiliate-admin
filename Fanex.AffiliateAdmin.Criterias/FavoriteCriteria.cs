// --------------------------------------------------------------------------
// <copyright file="FavoriteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Favorite Criteria.
    /// </summary>
    public class FavoriteCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public int ErrCode { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        [SpParam(Ignored = true)]
        public CriteriaType Type { get; set; }

        public override string GetSettingKey()
        {
            if (Type == CriteriaType.Insert)
            {
                return "AffAdmin_FavoriteIns";
            }

            return "AffAdmin_FavoriteDel";
        }

        public override bool IsValid()
        {
            return AffId > 0;
        }
    }
}