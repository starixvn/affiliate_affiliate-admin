// -----------------------------------------------------------------------
// <copyright file="FunctionMenu.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using Fanex.AffiliateAdmin.Resources;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class FunctionMenu
    {
        public string Text
        {
            get
            {
                return Lang.ResourceManager.GetString(Name);
            }
        }

        public string Name { get; set; }

        public string Link { get; set; }
    }
}