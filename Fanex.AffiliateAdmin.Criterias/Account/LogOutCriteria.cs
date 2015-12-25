// --------------------------------------------------------------------------
// <copyright file="LogOutCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.07">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Logout Criteria.
    /// </summary>
    public class LogOutCriteria : CriteriaBase
    {
        public int OnUserId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Logout";
        }
    }
}