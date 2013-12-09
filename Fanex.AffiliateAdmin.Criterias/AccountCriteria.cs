// -----------------------------------------------------------------------
// <copyright file="AccountCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public int GroupId { get; set; }

        public string Username { get; set; }

        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Language { get; set; }

        public string Ip { get; set; }

        public bool Closed { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

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
                case "Add":
                    settingKey = "AffAdmin_Con_AccountIns"; 
                    break;
                case "Edit":
                    settingKey = "AffAdmin_Con_AccountUpd";                       
                    break;
                case "Status":
                    settingKey = "AffAdmin_Con_AccountUpdClosed";
                    break;
                case "UserCurrency":
                    settingKey = "Aff_UserCurrencySel";
                    break;
                case "SystemCurrency":
                    settingKey = "AffAdmin_SystemSupportedCurrencies";
                    break;
                case "AllSystemCurrency":
                    settingKey = "AffAdmin_SystemAllSupportedCurrencies";
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
