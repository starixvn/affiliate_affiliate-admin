// --------------------------------------------------------------------------
// <copyright file="AddConfigurationViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AddConfigurationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "KeyFieldIsRequired")]
        public string Key { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ValueFieldIsRequired")]
        public string Value { get; set; }

        public SystemSetting TypeList { get; set; }

        public IEnumerable<SelectListItem> TypeOptionList
        {
            get
            {
                var selectItemList = TypeList.SValue.Split(',').Select(t => new SelectListItem
                {
                    Text = t,
                    Value = t
                }).ToList();

                return selectItemList;
            }
        }

        public IEnumerable<SystemSetting> EnvironmentList { get; set; }

        public IEnumerable<SelectListItem> EnvironmentOptionList
        {
            get
            {
                var selectItemList = EnvironmentList.Select(t => new SelectListItem
                {
                    Text = t.SValue,
                    Value = t.SValue
                }).ToList();

                selectItemList.Insert(0, new SelectListItem { Text = Lang.None, Value = string.Empty });

                return selectItemList;
            }
        }

        public string Description { get; set; }

        public string Environment { get; set; }
    }
}
