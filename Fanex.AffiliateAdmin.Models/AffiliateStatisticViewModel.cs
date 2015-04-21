// -----------------------------------------------------------------------
// <copyright file="AffiliateStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateStatisticViewModel
    {
        #region Fields

        private IList<Product> _products;

        #endregion Fields

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
            _products = products.Select(t => new Product()
                {
                    ProductID = t.ProductID,
                    ProductName = Lang.ResourceManager.GetString(t.ProductName.Replace(" ", string.Empty)),
                    TotalQualifiedMembers = t.TotalQualifiedMembers,
                    QMDeposit = t.QMDeposit,
                    QMTurnOver = t.QMTurnOver
                }).ToList();
        }
    }
}