// -----------------------------------------------------------------------
// <copyright file="ServiceWrapper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceWrapper
    {       
        public static XElement GetRegList(string siteName, DateTime targetDate)
        {
            return null;// Client.GetRegList(siteName, targetDate);
        }

        public static XElement UpdateBtage(string siteName, string licenseeUserName, string newBtag)
        {
            return null;//Client.UpdateBtage(siteName, licenseeUserName, newBtag);
        }

        public static XElement GetLicenseeCustInfo(string siteName, string licenseeUserName)
        {
            return null;//Client.GetLicenseeCustInfo(siteName, licenseeUserName);
        }
        public static XElement Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount)
        {
            return null;//Client.Transfer(siteName, licenseeUserName, amount, dummyAccount);
        }
    }
}
