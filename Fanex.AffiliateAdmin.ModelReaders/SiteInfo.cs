using System;

namespace Fanex.AffiliateAdmin.ModelReaders
{
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