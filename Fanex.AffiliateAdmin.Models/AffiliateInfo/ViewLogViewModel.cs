// -----------------------------------------------------------------------
// <copyright file="ViewLogViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewLogViewModel
    {
        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime DateFrom { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime DateTo { get; set; }

        public IEnumerable<LogType> LogTypeList { get; set; }

        [Display(Name = "LogType", ResourceType = typeof(Lang))]
        public string LogType { get; set; }

        public IEnumerable<SelectListItem> LogTypeListItem
        {
            get
            {
                var actualLogTypeList = LogTypeList.ToList();
                actualLogTypeList.Insert(0, new LogType { TypeId = 0, TypeName = Lang.All, Description = Lang.All });

                return actualLogTypeList.Select(log => new SelectListItem
                {
                    Value = log.TypeId.ToString(),
                    Text = log.Description
                });
            }
        }
    }
}