// --------------------------------------------------------------------------
// <copyright file="AffTypeDeleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias.Configuration.AffType
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete criteria class.
    /// </summary>
    public class AffTypeDeleteCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string IDList { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffTypeDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(IDList);
        }
    }
}