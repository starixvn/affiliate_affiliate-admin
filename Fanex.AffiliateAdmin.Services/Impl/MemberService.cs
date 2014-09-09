// --------------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using Core.Services;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberService : QueryService, IMemberService
    {
        public MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria)
        {
            return GetEntity<MemberDetail>(criteria);
        }
    }
}