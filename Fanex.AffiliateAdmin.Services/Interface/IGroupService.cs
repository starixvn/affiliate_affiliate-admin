// -----------------------------------------------------------------------
// <copyright file="IGroupService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGroupService
    {
        IEnumerable<GroupInfo> GetListGroup(GroupListCriteria criteria);

        GroupInfo GetGroupInfo(GroupListCriteria criteria);

        void GroupInsert(GroupCriteria criteria);

        void GroupUpdate(GroupCriteria criteria);

        void GroupDelete(GroupCriteria criteria);

        void GroupUpdatePolicy(PolicyUpdateCriteria criteria);
    }
}
