// --------------------------------------------------------------------------
// <copyright file="EmailHistoryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;

    /// <summary>
    /// Email History Service.
    /// </summary>
    public class EmailHistoryService : QueryService, IEmailHistoryService
    {
        #region Fields

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHistoryService" /> class.
        /// </summary>
        public EmailHistoryService()
            ////: this(null)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Get Email History list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List email history.</returns>
        public IEnumerable<EmailHistory> GetList(EmailHistoryCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<EmailHistory>(criteria);
        }

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Email history criteria.</param>        
        public void ExecuteSP(EmailHistoryCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");            
            ExecuteOutput(criteria);           
        }
    }
}