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

        private IEnumerable<SelectListItem> _countries;

        private IEnumerable<SystemSetting> _paymentMethods;

        #endregion Fields

        #region Properties

        public int AffId { get; set; }

        public Affiliate Affiliate { get; set; }

        public IEnumerable<SelectListItem> Countries
        {
            get
            {
                return _countries;
            }
        }

        public IEnumerable<SelectListItem> PaymentMethodListItem
        {
            get
            {
                return _paymentMethods.Select(payment => new SelectListItem
                {
                    Value = payment.SName,
                    Text = payment.SValue,
                    Selected = Affiliate.PreferredPayment == payment.SName
                }).ToList();
            }
        }

        #endregion Properties

        #region Methods

        public void AddCountries(IEnumerable<Country> countries)
        {
            _countries = countries.Select(country => new SelectListItem
            {
                Value = country.CountryID,
                Text = country.CountryID == "-" ? Resources.Country.Others : Resources.Country.ResourceManager.GetString(country.CountryID)
            }).ToList();
        }

        public void AddPaymentMethods(IEnumerable<SystemSetting> paymentMethods)
        {
            _paymentMethods = paymentMethods;
        }

        #endregion Methods
    }
}