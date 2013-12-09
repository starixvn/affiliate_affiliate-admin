// --------------------------------------------------------------------------
// <copyright file="IEmailHistoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// Interface Email History service.
    /// </summary>
    public interface IEmailHistoryService
    {
        /// <summary>
        /// Get email history list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List email history.</returns>
        IEnumerable<EmailHistory> GetList(EmailHistoryCriteria criteria);

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ExecuteSp(EmailHistoryCriteria criteria);
    }
}