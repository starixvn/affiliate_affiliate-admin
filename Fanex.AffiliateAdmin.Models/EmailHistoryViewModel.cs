// --------------------------------------------------------------------------
// <copyright file="EmailHistoryViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System;

    /// <summary>
    /// Email History ViewModel.
    /// </summary>
    public class EmailHistoryViewModel
    {
        public EmailHistoryViewModel()
        {
            SearchOption = -1;
            FromDate = ToDate = DateTime.Now;
            IsAll = true;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IsAll { get; set; }

        public string SearchText { get; set; }

        public int SearchOption { get; set; }    

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }    
    }
}