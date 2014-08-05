// --------------------------------------------------------------------------
// <copyright file="Generator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.1.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// Get random string.
        /// </summary>
        /// <param name="length">Length of string random.</param>
        /// <returns>Result of string after random.</returns>
        public static string GetRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
