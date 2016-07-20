namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Fanex.Data.Repository;

    /// <summary>
    /// Interface service.
    /// </summary>
    public interface ICommGroupService
    {
        /// <summary>
        /// Execute store procedure.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ExecuteOutput(ICriteria criteria);

        /// <summary>
        /// Get deduction settings.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        IEnumerable<DeductionSetting> GetDeductionSettings(DeductionSettingCriteria criteria);

        /// <summary>
        /// Get group details.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        IEnumerable<CommGroupTierSetting> GetTierSettings(TierSettingCriteria criteria);

        /// <summary>
        /// Get list data.
        /// </summary>
        /// <typeparam name="T">DTO generic type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        IEnumerable<T> GetListData<T>(ICriteria criteria);

        IEnumerable<CommGroup> GetById(CommGroupSelByIdCriteria criteria);
    }
}