namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using Fanex.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public enum FormatType
    {
        /// <summary>
        /// Negative decimal format.
        /// </summary>
        [StringValue("({0:#,##0.00})")]
        NEGATIVE_DECIMAL_FORMAT,

        /// <summary>
        /// Decimal format.
        /// </summary>
        [StringValue("{0:#,##0.00}")]
        DECIMAL_FORMAT,

        /// <summary>
        /// Negative percent format.
        /// </summary>
        [StringValue("({0:#,##0.00}%)")]
        NEGATIVE_PERCENT_FORMAT,

        /// <summary>
        /// Percent format.
        /// </summary>
        [StringValue("{0:#,##0.00}%")]
        PERCENT_FORMAT,

        /// <summary>
        /// Integer format.
        /// </summary>
        [StringValue("{0:#,##0}")]        
        INTERGER_FORMAT,

        /// <summary>
        /// Negative integer format.
        /// </summary>
        [StringValue("({0:#,##0})")]
        NEGATIVE_INTERGER_FORMAT,

        /// <summary>
        /// Date format.
        /// </summary>
        [StringValue("{0:dd MMM yyyy}")]
        DATE_FORMAT,

        /// <summary>
        /// Month format.
        /// </summary>
        [StringValue("{0:MMM yyyy}")]
        MONTH_FORMAT,

        /// <summary>
        /// Year format.
        /// </summary>
        [StringValue("{0:yyyy}")]
        YEAR_FORMAT,

        /// <summary>
        /// Time format.
        /// </summary>
        [StringValue("{0:hh:mm:ss}")]
        TIME_FORMAT,

        /// <summary>
        /// Date time format.
        /// </summary>
        [StringValue("{0:dd MMM yyyy hh:mm:ss tt}")]
        DATETIME_FORMAT,

        /// <summary>
        /// Negative color format.
        /// </summary>
        [StringValue("neg")]
        NEGATIVE_COLOR_FORMAT,

        /// <summary>
        /// Full date and time format.
        /// </summary>
        [StringValue("dd MMM yyyy hh:mm:ss")]
        FULL_DATETIME_FORMAT,
        
        /// <summary>
        /// Date Time format follow IBC.
        /// </summary>
        [StringValue("{0:MM/dd/yyyy HH:mm:ss}")]
        FULL_DATETIME_SPLASH_FORMAT
    }
}
