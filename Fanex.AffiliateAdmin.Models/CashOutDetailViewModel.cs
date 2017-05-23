using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.AffiliateAdmin.Models
{
    public class CashOutDetailViewModel
    {
        public long RefNo { get; set; }

        public decimal ExchangeRate { get; set; }

        public long TranId { get; set; }

        public long OriginalTransId { get; set; }

        public long BetType { get; set; }
    }
}
