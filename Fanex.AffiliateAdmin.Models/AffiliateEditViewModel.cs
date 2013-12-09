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
        public Affiliate Affiliate { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public IList<SystemSetting> PaymentMethods { get; set; }

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
    }
}
