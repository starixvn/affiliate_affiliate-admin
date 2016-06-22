namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Affiliate.DTO;
    using Fanex.AffiliateAdmin.Criteria;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// RNG Casino Result Detail class.
    /// </summary>
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Reviewed. Suppression is OK here")]
    [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Since Betlist.dll used that data-structure.")]
    public class BetListResultsRNGCasinoDetailViewModel
    {
        public BetListResultsRNGCasinoDetailViewModel()
        {
            CasinoDetailList = new List<RNGCasinoResultWinlossDetail>();
            CasinoDetailHeaderList = new List<RNGCasinoResultWinlossDetailHeader>();
        }

        public BetListResultsRNGCasinoDetailCriteria Param { get; set; }

        public IList<RNGCasinoResultWinlossDetail> CasinoDetailList { get; set; }

        public IList<RNGCasinoResultWinlossDetailHeader> CasinoDetailHeaderList { get; set; }
    }
}
