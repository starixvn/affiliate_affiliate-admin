namespace Fanex.AffiliateAdmin.Models
{
    using Fanex.AffiliateAdmin.Core.Configurations;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Change Account Type View Model.
    /// </summary>
    public class ChangeAccountTypeViewModel
    {
        public string AffIds { get; set; }

        public string Usernames { get; set; }

        public int TestLevel { get; set; }

        public IEnumerable<SelectListItem> AccountTypeOptionList
        {
            get
            {
                return AffiliateSettings.AccountTypes.Select(t => new SelectListItem
                {
                    Value = t.TypeID.ToString(),
                    Text = t.TypeName,
                    Selected = t.TypeID == TestLevel
                });
            }
        }
    }
}
