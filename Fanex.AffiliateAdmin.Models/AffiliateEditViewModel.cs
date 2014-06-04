// -----------------------------------------------------------------------
// <copyright file="AffiliateEditViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateEditViewModel
    {
        #region Fields

        private IEnumerable<Country> _countries;

        private IEnumerable<SystemSetting> _paymentMethods;

        #endregion

        #region Properties

        public Affiliate Affiliate { get; set; }

        public IEnumerable<Country> Countries
        {
            get
            {
                return _countries;
            }
        }

        public IEnumerable<SystemSetting> PaymentMethods
        {
            get
            {
                return _paymentMethods;
            }
        }

        public decimal AffMinWire { get; set; }

        public decimal AffMinNeteller { get; set; }

        public decimal AffMinCheque { get; set; }

        public decimal AffMinMoneyBookers { get; set; }

        public decimal AffMinPlayer { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodListItem
        {
            get
            {
                return PaymentMethods.Select(payment => new SelectListItem
                {
                    Value = payment.SName,
                    Text = payment.SValue,
                    Selected = Affiliate.PreferredPayment == payment.SName
                }).ToList();
            }
        } 

        #endregion

        #region Methods

        public void AddCountries(IEnumerable<Country> countries)
        {
            _countries = countries;
        }

        public void AddPaymentMethods(IEnumerable<SystemSetting> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        #endregion
    }
}
