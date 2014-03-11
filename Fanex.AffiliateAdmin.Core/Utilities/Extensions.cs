// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Fanex.Common.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            return property.GetValue(obj, null);
        }

        public static IDictionary<string, object> AddProperty(this object obj, string name, object value)
        {
            var dictionary = obj.ToDictionary();
            dictionary.Add(name, value);
            return dictionary;
        }

        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            var properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(obj));
            }

            return result;
        }

        public static void AddOrAppend(this IDictionary<string, object> obj, string key, object value)
        {
            if (obj.ContainsKey(key))
            {
                obj[key] += value.ToString();
            }
            else
            {
                obj[key] = value;
            }
        }

        public static string OrDefault(this string str, string @default = default(string))
        {
            return string.IsNullOrWhiteSpace(str) ? @default : str;
        }

        /// <summary>
        /// Will get the string value for a given enumerations value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enumeration.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>String attribute value.</returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            var type = value.GetType();

            // Get fieldinfo for this type
            var fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Value : null;
        }
    }
}
