namespace Fanex.AffiliateAdmin.Services
{
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using System.Collections.Generic;

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
            : base(null)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Get Email History list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List email history.</returns>
        public IEnumerable<EmailHistory> GetList(EmailHistorySelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<EmailHistory>(criteria);
        }

        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Email history criteria.</param>
        public void ExecuteSP(EmailHistoryDelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            ExecuteOutput(criteria);
        }
    }
}