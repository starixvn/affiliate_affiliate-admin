// -----------------------------------------------------------------------
// <copyright file="StatementViewModel.cs" company="Microsoft">
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
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatementViewModel
    {
        public int AffId { get; set; }

        [Display(Name = "TransType", ResourceType = typeof(Lang))]
        public string TransType { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Lang))]
        public string Status { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public string BaseCurrency { get; set; }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        public IList<TransType> TransTypeList { get; set; }

        public IEnumerable<SelectListItem> TransTypeListItem
        {
            get
            {
                TransTypeList.Insert(0, new TransType { TypeId = 0, TypeName = Lang.All });

                return TransTypeList.Select(trans => new SelectListItem
                {
                    Value = trans.TypeId != 0 ? trans.TypeId.ToString() : string.Empty,
                    Text = trans.TypeName
                }).ToList();
            }
        }

        public IList<SelectListItem> BaseCurrencyList { get; set; }

        public IEnumerable<SelectListItem> BaseCurrencyListItem
        {
            get
            {
                if (BaseCurrencyList.Count == 2 && BaseCurrencyList[0].Text == BaseCurrencyList[1].Text)
                {
                    BaseCurrencyList.RemoveAt(1);
                }

                return new SelectList(BaseCurrencyList, "Value", "Text");
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }
    }
}
