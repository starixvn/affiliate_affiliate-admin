// -----------------------------------------------------------------------
// <copyright file="ViewLogViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.AffiliateAdmin.Services;

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
        #region Fields

        private IEnumerable<LogType> _logTypeList;

        #endregion

        public ViewLogViewModel()
        {
            var dateFrom = SystemTimeService.GetSystemDateTime().AddMonths(AppSettings.ViewLogBackMonth);

            DateFrom = AppSettings.SystemStartDate.CompareTo(dateFrom) <= 0 ? dateFrom : AppSettings.SystemStartDate;
            DateTo = SystemTimeService.GetSystemDateTime();
        }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime DateFrom { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime DateTo { get; set; }

        public IEnumerable<LogType> LogTypeList
        {
            get
            {
                return _logTypeList;
            }
        }

        [Display(Name = "LogType", ResourceType = typeof(Lang))]
        public string LogType { get; set; }

        public IEnumerable<SelectListItem> LogTypeListItem
        {
            get
            {
                var actualLogTypeList = _logTypeList.ToList();
                actualLogTypeList.Insert(0, new LogType { TypeId = 0, TypeName = Lang.All });

                return actualLogTypeList.Select(log => new SelectListItem
                {
                    Value = log.TypeId.ToString(),
                    Text = log.TypeName
                }).ToList();
            }
        }

        #region Methods

        public void AddLogTypeList(IEnumerable<LogType> logTypeList)
        {
            _logTypeList = logTypeList;
        }

        #endregion
    }
}
