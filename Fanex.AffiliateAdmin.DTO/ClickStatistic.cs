// -----------------------------------------------------------------------
// <copyright file="ClickStatistic.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClickStatistic
    {
        public string DomainReferrer { get; set; }

        public string WebsiteUrl { get; set; }

        public string SportCode { get; set; }

        public string LanguageCode { get; set; }

        public string LanguageName { get; set; }

        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public int Signups { get; set; }

        public int FDM { get; set; }
    }
}
