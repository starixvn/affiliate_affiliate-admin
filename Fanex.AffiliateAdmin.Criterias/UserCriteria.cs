// -----------------------------------------------------------------------
// <copyright file="UserCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserCriteria : CriteriaBase
    {
        private string _firstChar;

        private string _userName;

        public int? CustId { get; set; }

        public int? GroupId { get; set; }

        public string UserName 
        {
            get
            {
                if (!string.IsNullOrEmpty(this._userName))
                {
                    return string.Format("%{0}%", _userName);
                }

                return _userName; 
            }

            set
            {
                this._userName = value;
            }
        }

        public string FirstChar
        {
            get 
            {
                if (!string.IsNullOrEmpty(_firstChar) &&
                     !_firstChar.Equals("All", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (_firstChar.Equals("0-9"))
                    {
                        _firstChar = string.Format("[{0}]", _firstChar);
                    }

                    _firstChar = string.Format("{0}%", _firstChar);
                }
                
                return _firstChar; 
            }
            
            set
            {
                _firstChar = value;
            }
        }

        public bool IsIt { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string Type { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;
            switch (this.Type)
            {
                case "ByGroup":
                    settingKey = "AffAdmin_Con_SelByGroupID";
                    break;
                case "ByCustId":
                    settingKey = "AffAdmin_Con_SelByCustID";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
