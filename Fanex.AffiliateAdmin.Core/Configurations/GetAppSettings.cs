// --------------------------------------------------------------------------
// <copyright file="GetAppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.6">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System;
    using System.Configuration;

    /// <summary>
    /// Get value of application settings.
    /// </summary>
    /// <typeparam name="T">A generic type.</typeparam>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public static class GetAppSettings<T>
    {
        public static T GetValue(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
