namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Note Delete Criteria.
    /// </summary>
    public class SelByGroupIDCriteria : CriteriaBase
    {
        private string _firstChar;

        private string _userName;

        public int Groupid { get; set; }

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

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_SelByGroupID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}