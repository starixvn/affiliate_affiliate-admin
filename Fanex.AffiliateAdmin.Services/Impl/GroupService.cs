// -----------------------------------------------------------------------
// <copyright file="GroupService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services
{
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupService : QueryService, IGroupService
    {
        private const string ItGroupName = "Aff_IT_Dept";

        public bool IsItGroup(int groupId)
        {
            var criteria = new GroupListCriteria
            {
                IsIt = SessionManager.CurrentSession.IsIT,
                LicenseeId = AppSettings.LicenseeID,
                GroupId = groupId
            };

            var result = GetEntity<GroupInfo>(criteria);
            return result != null && result.GroupName.Equals(ItGroupName, System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get List Group.
        /// </summary>
        /// <param name="criteria">Criteria List Group.</param>
        /// <returns>List group.</returns>
        public IEnumerable<GroupInfo> GetListGroup(GroupListCriteria criteria, bool removeItGroup = false)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = GetData<GroupInfo>(criteria);
            if (removeItGroup)
            {
                result = result.Where(x => !x.GroupName.Equals(ItGroupName, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }

            foreach (var group in result)
            {
                var translated = Lang.ResourceManager.GetString(group.GroupName.Replace(" ", string.Empty));
                group.GroupName = string.IsNullOrEmpty(translated) ? group.GroupName : translated;
            }

            return result;
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
        public void GroupInsert(GroupInfoInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        public void GroupUpdate(GroupInfoUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        public void GroupDelete(GroupInfoDelCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Update group policy.
        /// </summary>
        /// <param name="criteria">Policy update criteria.</param>
        public void GroupUpdatePolicy(ICriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}