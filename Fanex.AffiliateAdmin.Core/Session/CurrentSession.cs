namespace Fanex.AffiliateAdmin.Core.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configurations;
    using Enumerations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;

    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        private readonly IEnumerable<FunctionPolicy> _policies;

        public CurrentSession(
            Account user,
            IEnumerable<FunctionPolicy> policies,
            IEnumerable<Currency> currencies)
        {
            this.CustID = user.CustID;
            this.Username = user.Username;
            this.OnUserID = user.OnUserID;
            this.IsIT = user.IsIT;
            this._policies = policies ?? new List<FunctionPolicy>();
            this.CurrencyList = currencies;
            this.FunctionMenus = CreateFunctionMenus();
        }

        public AppMode ApplicationMode { get; set; }

        public string VendorID { get; set; }

        public string VendorIdentityID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public int OnUserID { get; set; }

        public bool IsIT { get; set; }

        public IEnumerable<Currency> CurrencyList { get; private set; }

        public IEnumerable<FunctionMenu> FunctionMenus { get; private set; }

        public bool HasReadPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanRead);
        }

        public bool HasHostPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanHost);
        }

        private IEnumerable<FunctionMenu> CreateFunctionMenus()
        {
            var menuList = new List<FunctionMenu>();
            menuList.Add(new FunctionMenu()
            {
                Name = "Home",
                Link = "~/"
            });

            if (_policies.Any() == false)
            {
                return menuList;
            }

            if (IsIT || CurrencyList.Any())
            {
                AddFunctionMenu(menuList, FunctionPolicyNames.Affiliate, "AffiliateInfo", "~/AffiliateInfo");
                AddFunctionMenu(menuList, FunctionPolicyNames.Affiliate, "AffManagement", "~/Affiliate");
                AddFunctionMenu(menuList, FunctionPolicyNames.Payment, "Payment", "~/Payment");
                AddFunctionMenu(menuList, FunctionPolicyNames.CommissionGroup, "CommissionGroup", "~/CommGroup");
                AddFunctionMenu(menuList, FunctionPolicyNames.AffReport, "Reports", "~/Report");
            }

            AddFunctionMenu(menuList, FunctionPolicyNames.Media, "MarketingTools", "~/Media/BannerList");
            AddFunctionMenu(menuList, FunctionPolicyNames.HotNews, "Message", "~/Message");

            if (IsIT || CurrencyList.Any())
            {
                AddFunctionMenu(menuList, FunctionPolicyNames.AffExchangeRate, "Currency", "~/Exchange");
            }

            AddFunctionMenu(menuList, FunctionPolicyNames.SystemSettings, "Configurations", "~/Configuration");
            AddFunctionMenu(menuList, FunctionPolicyNames.Domain, "Domain", "~/Domain/AffDomain");
            AddFunctionMenu(menuList, FunctionPolicyNames.UserControl, "UserManagement", "~/Management/Users");

            return menuList;
        }

        private void AddFunctionMenu(List<FunctionMenu> menus, string functionName, string name, string link)
        {
            if (HasReadPolicy(functionName))
            {
                menus.Add(new FunctionMenu()
                {
                    Name = name,
                    Link = link
                });
            }
        }
    }
}