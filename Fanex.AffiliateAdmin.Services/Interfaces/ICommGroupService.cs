namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Fanex.Data.Repository;

    public interface ICommGroupService
    {
        IEnumerable<CommGroupTierSetting> GetTierSettings(TierSettingCriteria criteria);

        void ExecuteOutput(ICriteria criteria);

        IEnumerable<T> GetListData<T>(ICriteria criteria);

        IEnumerable<CommGroup> GetById(CommGroupSelByIdCriteria criteria);

        void BuildGroupData(IEnumerable<CommGroup> groups, DateTime date, string groupIDList);
    }
}