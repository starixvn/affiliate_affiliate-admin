namespace Fanex.AffiliateAdmin.Core.Session
{
    using System.Collections.Generic;
    using Enumerations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;

    public interface ICurrentSession
    {
        AppMode ApplicationMode { get; set; }

        string VendorID { get; set; }

        string VendorIdentityID { get; set; }

        int CustID { get; set; }

        string Username { get; set; }

        int OnUserID { get; set; }

        bool IsIT { get; set; }

        IEnumerable<Currency> CurrencyList { get; }

        IEnumerable<FunctionMenu> FunctionMenus { get; }

        bool HasReadPolicy(string functionName);

        bool HasHostPolicy(string functionName);

        void SetPolicyList(IEnumerable<FunctionPolicy> policyList);
    }
}