// --------------------------------------------------------------------------
// <copyright file="FormaterCurrency.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.11">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Format currency class.
    /// </summary>
    public class FormaterCurrency
    {
        public const string DECIMALFORMAT20 = "{0:#,##0.00}";
        private const string _Minus = "<span style='color: red'>{0}</span>";
        private static readonly Dictionary<string, CultureInfo> _SymbolsByCode = new Dictionary<string, CultureInfo>();
        private static CultureInfo _defaultCulture = Thread.CurrentThread.CurrentCulture;

        /// <summary>
        /// Gets Currency Symbol.
        /// </summary>
        /// <param name="currencyCode">Currency Code.</param>
        /// <returns>Return currency symbol.</returns>
        public static string GetCurrencySymbol(string currencyCode = null)
        {
            if (currencyCode == null)
            {
                return _defaultCulture.NumberFormat.CurrencySymbol;
            }

            if (_SymbolsByCode.Count == 0)
            {
                InitCurrencyCodeMapper();
            }

            if (_SymbolsByCode.ContainsKey(currencyCode))
            {
                return _SymbolsByCode[currencyCode].NumberFormat.CurrencySymbol;
            }

            return currencyCode;
        }

        /// <summary>
        /// Set Default Currency.
        /// </summary>
        /// <param name="currencyCode">Currency Code.</param>
        public static void SetDefaultCurrency(string currencyCode)
        {
            _defaultCulture = GetCulture(currencyCode);
        }

        /// <summary>
        /// Set Default Currency by Culture.
        /// </summary>
        /// <param name="culture">Culture Info.</param>
        public static void SetDefaultCurrency(CultureInfo culture)
        {
            _defaultCulture = culture;
        }

        /// <summary>
        /// Gets Culture.
        /// </summary>
        /// <param name="currencyCode">Currency Code.</param>
        /// <returns>Return culture info.</returns>
        public static CultureInfo GetCulture(string currencyCode)
        {
            if (_SymbolsByCode.Count == 0)
            {
                InitCurrencyCodeMapper();
            }

            if (string.IsNullOrEmpty(currencyCode))
            {
                return _SymbolsByCode["MYR"];
            }

            if (currencyCode.ToUpper() == "UUS")
            {
                return _SymbolsByCode["USD"];
            }

            return _SymbolsByCode[currencyCode];
        }

        /// <summary>
        /// Format Money.
        /// </summary>
        /// <param name="value">Value of number.</param>
        /// <param name="culture">Culture format number.</param>
        /// <param name="checkMinus">Has Check Minus.</param>
        /// <returns>Return number after format.</returns>
        public static string Format(object value, CultureInfo culture, bool checkMinus)
        {
            double d = 0;
            double.TryParse(value.ToString(), out d);
            var currencySymbol = culture.NumberFormat.CurrencySymbol ?? _defaultCulture.NumberFormat.CurrencySymbol;
            var newValue = string.Format("{0}{1}", currencySymbol, string.Format(DECIMALFORMAT20, value));
            if (d < 0 && checkMinus)
            {
                if (newValue.Contains("-"))
                {
                    newValue = string.Format("({0})", newValue.Replace("-", string.Empty));
                }

                newValue = string.Format(_Minus, newValue);
            }

            return newValue;
        }

        /// <summary>
        /// Format money.
        /// </summary>
        /// <param name="value">Value of number.</param>
        /// <param name="currencySymbol">Currency Symbol.</param>
        /// <param name="checkMinus">Is Check Minus.</param>
        /// <returns>Return number after format.</returns>
        public static string Format(object value, string currencySymbol, bool checkMinus = true)
        {
            return Format(value, GetCulture(currencySymbol), checkMinus);
        }

        /// <summary>
        /// Format money.
        /// </summary>
        /// <param name="value">Value of number.</param>
        /// <param name="checkMinus">Is Check Minus.</param>
        /// <returns>Return number after format.</returns>
        public static string Format(object value, bool checkMinus = true)
        {
            return Format(value, _defaultCulture, checkMinus);
        }

        /// <summary>
        /// Initialize Currency Code Mapper.
        /// </summary>
        private static void InitCurrencyCodeMapper()
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var item in regions)
            {
                var key = new RegionInfo(item.LCID).ISOCurrencySymbol;

                if (!_SymbolsByCode.ContainsKey(key))
                {
                    item.NumberFormat.CurrencyDecimalDigits = 2;

                    _SymbolsByCode.Add(key, item);
                }
            }
        }
    }
}