// -----------------------------------------------------------------------
// <copyright file="StatementViewModel.cs" company="Microsoft">
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
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StatementViewModel
    {
        #region Fields

        private IList<SelectListItem> _baseCurrencyList;

        private IList<TransType> _transTypeList;

        #endregion

        public int AffId { get; set; }

        [Display(Name = "TransType", ResourceType = typeof(Lang))]
        public string TransType { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Lang))]
        public string Status { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public byte BaseCurrency { get; set; }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        public IList<TransType> TransTypeList
        {
            get
            {
                return _transTypeList.Select(trans => new TransType
                {
                    TypeId = trans.TypeId,
                    TypeName = Lang.ResourceManager.GetString(trans.TypeName)
                }).ToList();
            }
        }

        public IEnumerable<SelectListItem> TransTypeListItem
        {
            get
            {
                var translated = _transTypeList.Select(trans => new SelectListItem
                {
                    Value = trans.TypeId != 0 ? trans.TypeId.ToString() : string.Empty,
                    Text = Lang.ResourceManager.GetString(trans.TypeName)
                }).ToList();

                translated.Insert(0, new SelectListItem { Value = string.Empty, Text = Lang.All });

                return translated;
            }
        }

        public IList<SelectListItem> BaseCurrencyList
        {
            get
            {
                return _baseCurrencyList;
            }
        }

        public IEnumerable<SelectListItem> BaseCurrencyListItem
        {
            get
            {
                if (_baseCurrencyList.Count == 2 && _baseCurrencyList[0].Text == _baseCurrencyList[1].Text)
                {
                    _baseCurrencyList.RemoveAt(1);
                }

                return new SelectList(_baseCurrencyList, "Value", "Text");
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        #region Methods

        public void AddBaseCurrencyList(IList<SelectListItem> baseCurrencyList)
        {
            _baseCurrencyList = baseCurrencyList;
        }

        public void AddTransTypeList(IList<TransType> transTypeList)
        {
            _transTypeList = transTypeList;
        }

        #endregion
    }
}
