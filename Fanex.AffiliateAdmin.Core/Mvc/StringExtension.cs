// -----------------------------------------------------------------------
// <copyright file="StringExtension.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Get left string of input string.
        /// </summary>
        /// <param name="value">String input.</param>
        /// <param name="length">Length need get string.</param>
        /// <returns>Substring by length.</returns>
        public static string LeftString(this string value, int length)
        {
            if (value.Length <= length)
            {
                return value;
            }
            else
            {
                return value.Substring(0, length) + "...";
            }
        }
    }
}
