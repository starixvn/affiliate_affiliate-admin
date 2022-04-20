using System.Collections.Generic;

namespace Fanex.AffiliateAdmin.Core.Constants
{
    public static class Constants
    {
        public static readonly List<string> FavoriteStatusList = new List<string>
        {
            "approved",
            "closed",
            "lockedout"
        };

        public static readonly List<string> DefaultStatusList = new List<string>
        {
            "approved",
            "pending",
            "lockedout"
        };
    }
}
