// -----------------------------------------------------------------------
// <copyright file="AffiliateStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateStatisticViewModel
    {
        #region Fields

        private IList<Product> _products;

        #endregion

        public IList<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public AffiliateStatistic Statistic { get; set; }

        public CommGroupInfo CommGroupInfo { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int AffId { get; set; }
        
        public string AffName { get; set; }

        public string AffCurrency { get; set; }

        public string BaseCurrency { get; set; }

        public DateTime FullDateTime
        {
            get
            {
                return new DateTime(Year, Month, 1);
            }

            set
            {
                Month = value.Month;
                Year = value.Year;
            }
        }

        public void AddProducts(IList<Product> products)
        {
            _products = products;
        }
    }
}
