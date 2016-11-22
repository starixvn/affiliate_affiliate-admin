namespace Fanex.AffiliateAdmin.Core.Session
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        int CustID { get; set; }

        string Username { get; set; }

        int OnUserID { get; set; }

        bool IsIT { get; set; }

        IEnumerable<Currency> CurrencyList { get; }

        IEnumerable<FunctionMenu> FunctionMenus { get; }

        bool HasReadPolicy(string functionName);

        bool HasHostPolicy(string functionName);
    }
}