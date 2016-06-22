namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias;

    /// <summary>
    /// RNG Casino Result class.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Reviewed. Suppression is OK here")]
    [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Since Betlist.dll used that data-structure.")]
    public class BetListResultsRNGCasinoViewModel
    {
        public BetListResultsRNGCasinoViewModel(
            IList<RNGCasinoResult> data,
            IList<RNGCasinoResult> subTotal,
            BetListResultsRNGCasinoCriteria criteria)
        {
            Data = data;
            SubTotal = subTotal;
            Param = criteria;
            RefNo = criteria.RefNo.ToString();
            IsShowPic = criteria.IsShowPic;
            Username = criteria.SystemId;
        }

        public IList<RNGCasinoResult> Data { get; set; }

        public IList<RNGCasinoResult> SubTotal { get; set; }

        public BetListResultsRNGCasinoCriteria Param { get; set; }

        public string Username { get; set; }

        public string RefNo { get; set; }

        public string Currency { get; set; }

        public bool IsShowPic { get; set; }

        public int AffId { get; set; }
    }
}