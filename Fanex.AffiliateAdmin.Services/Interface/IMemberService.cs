// --------------------------------------------------------------------------
// <copyright file="IMemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// IMemberService Interface.
    /// </summary>
    public interface IMemberService
    {
        MemberDetail GetMemberDetail(GetMemberDetailCriteria criteria);
    }
}