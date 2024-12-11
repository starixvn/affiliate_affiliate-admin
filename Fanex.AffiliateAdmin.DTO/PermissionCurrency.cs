// -----------------------------------------------------------------------
// <copyright file="PermissionCurrency.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionCurrency
    {
        public short CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }

        public int IsActive { get; set; }

        public bool IsChecked
        {
            get
            {
                if (this.IsActive == 1)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
