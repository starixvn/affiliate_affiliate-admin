// --------------------------------------------------------------------------
// <copyright file="AffiliateListReportViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateListReportViewModel
    {
        #region Fields

        private IList<AffiliateInfo> _affiliateList = new List<AffiliateInfo>();

        private IList<CurrencyStatistics> _currencyList = new List<CurrencyStatistics>();

        #endregion

        #region Properties

        public IList<AffiliateInfo> AffiliateList
        {
            get
            {
                return _affiliateList;
            }
        }

        public IList<CurrencyStatistics> CurrencyList
        {
            get
            {
                return _currencyList;
            }
        }

        #endregion

        #region Methods

        public void AddAffiliateList(IList<AffiliateInfo> affiliateList)
        {
            _affiliateList = affiliateList;
        }

        public void AddCurrencyList(IList<CurrencyStatistics> currencyList)
        {
            _currencyList = currencyList;
        }

        #endregion
    }
}
