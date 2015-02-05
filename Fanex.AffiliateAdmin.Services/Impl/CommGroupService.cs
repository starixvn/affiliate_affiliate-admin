// --------------------------------------------------------------------------
// <copyright file="CommGroupService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.Data.Repository;

    /// <summary>
    /// Email History Service.
    /// </summary>
    public class CommGroupService : QueryService, ICommGroupService
    {
        #region Fields

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommGroupService" /> class.
        /// </summary>
        public CommGroupService()
            : base(null)
        {
        }

        #endregion Constructor

        /// <summary>
        /// Get deduction settings.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        public IEnumerable<DeductionSetting> GetDeductionSettings(DeductionSettingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<DeductionSetting>(criteria);
        }

        /// <summary>
        /// Get group details.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        public IEnumerable<CommGroupTierSetting> GetTierSettings(TierSettingCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<CommGroupTierSetting>(criteria);
        }

        /// <summary>
        /// Get list data.
        /// </summary>
        /// <typeparam name="T">DTO generic type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List data.</returns>
        public IEnumerable<T> GetListData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<T>(criteria);
        }

        public IEnumerable<CommGroup> GetById(CommGroupSelByIdCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<CommGroup>(criteria);
        }
    }
}