// -----------------------------------------------------------------------
// <copyright file="LicenseeUtility.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ConfigGenerator.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LicenseeUtility
    {
        private static IList<Licensee> _licensees;

        public static IList<Licensee> GetSupportedLicensees()
        {
            var manager = new SiteManager
            {
                XmlFile = Utility.GetModelFolder() + @"\SiteConfigModel.xml"
            };

            manager.LoadXml();

            return manager.GetSupportedLicenseeList();
        }

        public static IList<Licensee> SupportedLicensees 
        {
            get
            {
                _licensees = _licensees ?? GetSupportedLicensees();

                return _licensees;
            }
        }
    }
}
