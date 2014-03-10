// -----------------------------------------------------------------------
// <copyright file="ViewLogViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewLogViewModel
    {
        public ViewLogViewModel()
        {
            var dateFrom = DateTime.Now.AddMonths(AppSettings.ViewLogBackMonth);

            DateFrom = AppSettings.SystemStartDate.CompareTo(dateFrom) <= 0 ? dateFrom : AppSettings.SystemStartDate;
            DateTo = DateTime.Now;
        }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime DateFrom { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime DateTo { get; set; }

        public IList<LogType> LogTypeList { get; set; }

        [Display(Name = "LogType", ResourceType = typeof(Lang))]
        public string LogType { get; set; }

        public IEnumerable<SelectListItem> LogTypeListItem
        {
            get
            {
                LogTypeList.Insert(0, new LogType { TypeId = 0, TypeName = Lang.All });

                return LogTypeList.Select(log => new SelectListItem
                {
                    Value = log.TypeId.ToString(),
                    Text = log.TypeName
                }).ToList();
            }
        } 
    }
}
