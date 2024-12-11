namespace Fanex.AffiliateAdmin.Models
{
    using Fanex.AffiliateAdmin.DTO;
    using System.Collections.Generic;

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
