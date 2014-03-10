// -----------------------------------------------------------------------
// <copyright file="Formater.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.Globalization;

    /// <summary>
    /// Format class.
    /// </summary>
    public static class Formater
    {
        private static CultureInfo info = new CultureInfo("en-US");

        static Formater()
        {
            info = new CultureInfo("en-US");
        }

        public static string FormatObject(object value, FormatType type)
        {
            return string.Format(info, type.GetStringValue(), value);
        }
    }
}
