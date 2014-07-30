// --------------------------------------------------------------------------
// <copyright file="ServiceWrapper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.04">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Xml.Linq;

    /// <summary>
    /// Service Wrapper.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class ServiceWrapper
    {
        private static MemberSiteService.ServiceSoapClient _client;

        private static MemberSiteService.ServiceSoapClient Client
        {
            get { return _client ?? (_client = new MemberSiteService.ServiceSoapClient("ServiceSoap")); }
        }

        public static XElement GetRegList(string siteName, DateTime targetDate)
        {
            return Client.GetRegList(siteName, targetDate);
        }

        public static XElement UpdateBtage(string siteName, string licenseeUserName, string newBtag)
        {
            return Client.UpdateBtage(siteName, licenseeUserName, newBtag);
        }

        public static XElement GetLicenseeCustInfo(string siteName, string licenseeUserName)
        {
            return Client.GetLicenseeCustInfo(siteName, licenseeUserName);
        }

        public static XElement Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount)
        {
            return Client.Transfer(siteName, licenseeUserName, amount, dummyAccount);
        }
    }
}