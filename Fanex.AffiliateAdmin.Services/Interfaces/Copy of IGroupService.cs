// -----------------------------------------------------------------------
// <copyright file="IGroupService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetListService
    {
        float GetExchangeRate(GetExchangeRateCriteria criteria);
    }
}
