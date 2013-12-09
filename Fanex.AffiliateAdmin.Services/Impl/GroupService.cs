// -----------------------------------------------------------------------
// <copyright file="GroupService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DataAccess.Impl;
    using DataAccess.Interface;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupService : QueryService, IGroupService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly IGroupDataAccess _groupDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService" /> class.
        /// </summary>
        public GroupService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupService" /> class.
        /// </summary>
        /// <param name="groupDataAccess">Group data access.</param>
        private GroupService(IGroupDataAccess groupDataAccess)
        {
            _groupDataAccess = groupDataAccess ?? new GroupDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Get List Group.
        /// </summary>
        /// <param name="criteria">Criteria List Group.</param>
        /// <returns>List group.</returns>
        public IEnumerable<GroupInfo> GetListGroup(GroupListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<GroupInfo>(criteria);
        }

        /// <summary>
        /// Get Information Group.
        /// </summary>
        /// <param name="criteria">Criteria List Group.</param>
        /// <returns>Group Information.</returns>
        public GroupInfo GetGroupInfo(GroupListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetData<GroupInfo>(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Insert new group.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        public void GroupInsert(GroupCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        public void GroupUpdate(GroupCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        public void GroupDelete(GroupCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update group policy.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        public void GroupUpdatePolicy(PolicyUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
