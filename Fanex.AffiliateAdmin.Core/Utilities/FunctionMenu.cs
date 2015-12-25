// -----------------------------------------------------------------------
// <copyright file="FunctionMenu.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using Fanex.AffiliateAdmin.Resources;

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