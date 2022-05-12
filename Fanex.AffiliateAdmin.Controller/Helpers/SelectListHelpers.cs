namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using Fanex.AffiliateAdmin.DTO;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public static class SelectListHelpers
    {
        public static SelectList GetLanguageOptionList(IEnumerable<Language> languageList, string language = null)
        {
            return new SelectList(languageList, "LanguageID", "LanguageName", language);
        }

        public static SelectList GetPlayerCurrencyOptionList(IEnumerable<Currency> playerCurrencyList, string currency = null)
        {
            return new SelectList(playerCurrencyList, "CurrencyID", "CurrencyCode", currency);
        }

        public static SelectList GetAffiliateTypeOptionList(IEnumerable<AffType> affTypeList, string affType = null)
        {
            return new SelectList(affTypeList, "TypeID", "Name", affType);
        }

        public static SelectList GetCommonOptionList(IEnumerable<SelectListItem> selectListItem, string selectedValue = null)
        {
            return new SelectList(selectListItem, "Value", "Text", selectedValue);
        }
    }
}
