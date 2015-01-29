// --------------------------------------------------------------------------
// <copyright file="LicenseeUtility.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Core
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LicenseeUtility
    {
        private static IList<Licensee> _licensees;

        public static IList<Licensee> SupportedLicensees
        {
            get
            {
                if (_licensees != null)
                {
                    return _licensees;
                }

                var manager = new SiteManager
                {
                    XmlFile = Utility.GetModelFolder() + @"\SiteConfigModel.xml"
                };

                manager.LoadXml();

                _licensees = manager.GetSupportedLicenseeList();

                return _licensees;
            }
        }
    }
}