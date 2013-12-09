// --------------------------------------------------------------------------
// <copyright file="ConfigurationViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ConfigurationViewModel
    {
        public SystemSetting TypeList { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public IEnumerable<SelectListItem> TypeOptionList
        {
            get 
            {
                var selectItemList = TypeList.SValue.Split(',').Select(t => new SelectListItem
                { 
                    Text = t,
                    Value = t
                }).ToList();
                
                selectItemList.Insert(0, new SelectListItem { Text = Lang.All, Value = string.Empty });

                return selectItemList;
            }
        }
    }
}