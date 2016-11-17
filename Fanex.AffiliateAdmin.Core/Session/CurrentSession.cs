namespace Fanex.AffiliateAdmin.Core.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;

    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        private readonly IEnumerable<FunctionPolicy> _policies;

        private readonly IEnumerable<FunctionMenu> _menus;

        public CurrentSession(
            Account user,
            IEnumerable<FunctionPolicy> policies,
            IEnumerable<Currency> currencies,
            IEnumerable<FunctionMenu> menus)
        {
            this.CustID = user.CustID;
            this.Username = user.Username;
            this.OnUserID = user.OnUserID;
            this.IsIT = user.IsIT;
            this._policies = policies ?? new List<FunctionPolicy>();
            this.CurrencyList = currencies;

            FunctionMenus = menus;
        }

        public int CustID { get; set; }

        public string Username { get; set; }

        public int OnUserID { get; set; }

        public bool IsIT { get; set; }

        public IEnumerable<Currency> CurrencyList { get; private set; }

        public AffiliateSession Affiliate { get; set; }

        public IEnumerable<FunctionMenu> FunctionMenus { get; private set; }

        public bool HasReadPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanRead);
        }

        public bool HasHostPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanHost);
        }
    }
}