// -----------------------------------------------------------------------
// <copyright file="Currency.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Currency
    {
        public short CurrencyID { get; set; }
        public int OriginalCurrencyID { get; set; }
        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

        public int IsActive { get; set; }

        public int MultipliedBy { get; set; }
    }
}
