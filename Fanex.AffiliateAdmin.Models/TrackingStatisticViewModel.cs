// -----------------------------------------------------------------------
// <copyright file="TrackingStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TrackingStatisticViewModel
    {
        #region Fields

        private IEnumerable<SystemSetting> _customerPageList;

        #endregion

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string BaseLanguage { get; set; }

        [Display(Name = "MemberPage", ResourceType = typeof(Lang))]
        public string CustomerPage { get; set; }

        public IEnumerable<SystemSetting> CustomerPageList
        {
            get
            {
                return _customerPageList;
            }
        }

        public IEnumerable<SelectListItem> CustomerPageOptionList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (var item in _customerPageList)
                {
                    var translated = Lang.ResourceManager.GetString(item.SValue);
                    list.Add(new SelectListItem
                    {
                        Value = item.SValue,
                        Text = string.IsNullOrEmpty(translated) ? item.SValue : translated
                    });
                }

                return list;
            }
        }

        #region Methods

        public void AddCustomerPageList(IEnumerable<SystemSetting> customerPageList)
        {
            _customerPageList = customerPageList;
        }

        #endregion
    }
}
