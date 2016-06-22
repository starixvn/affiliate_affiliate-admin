namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemDateTime
    {
        public DateTime DatabaseDateTime { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "Accept GMT abbreviation")]
        public string GMTOffSet { get; set; }
    }
}
