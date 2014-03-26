// --------------------------------------------------------------------------
// <copyright file="IReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IReportDataAccess
    {
        PlayerWinlossReport GetMemberWinloss(PlayerWinlossReportCriteria criteria);
    }
}
