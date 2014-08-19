// --------------------------------------------------------------------------
// <copyright file="SiteInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.ConfigModelReaders
{
    using System;

    /// <summary>
    /// Site information.
    /// </summary>
    [Serializable]
    public class SiteInfo : MarshalByRefObject
    {
        public string Name { get; set; }

        public bool Status { get; set; }

        public string Environment { get; set; }

        public string WebConfigModel { get; set; }

        public string WebAppSettingModel { get; set; }

        public string WebConnectionStringModel { get; set; }
    }
}