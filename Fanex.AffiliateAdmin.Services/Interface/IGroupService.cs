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
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGroupService
    {
        IEnumerable<GroupInfo> GetListGroup(GroupListCriteria criteria);

        GroupInfo GetGroupInfo(GroupListCriteria criteria);

        void GroupInsert(GroupInfoInsCriteria criteria);

        void GroupUpdate(GroupInfoUpdCriteria criteria);

        void GroupDelete(GroupInfoDelCriteria criteria);

        void GroupUpdatePolicy(ICriteria criteria);
    }
}