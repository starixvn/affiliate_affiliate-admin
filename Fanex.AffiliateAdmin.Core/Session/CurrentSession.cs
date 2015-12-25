// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        private readonly IEnumerable<FunctionPolicy> _policies;

        private readonly IList<FunctionMenu> _menus;

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

            _menus = new List<FunctionMenu>();
            InitializeFunctionMenuItems();
        }

        public int CustID { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int OnUserID { get; set; }

        public bool IsIT { get; set; }

        public IEnumerable<Currency> CurrencyList { get; private set; }

        public AffiliateSession Affiliate { get; set; }

        public IEnumerable<FunctionMenu> FunctionMenus
        {
            get
            {
                return _menus;
            }
        }

        public bool HasReadPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanRead);
        }

        public bool HasHostPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanHost);
        }

        private void InitializeFunctionMenuItems()
        {
            _menus.Add(new FunctionMenu()
            {
                Name = "Home",
                Link = "~/"
            });

            if (_policies.Any() == false)
            {
                return;
            }

            if (IsIT || CurrencyList.Any())
            {
                AddFunctionMenu(FunctionPolicyNames.Affiliate, "AffiliateInfo", "~/AffiliateInfo");
                AddFunctionMenu(FunctionPolicyNames.Affiliate, "AffManagement", "~/Affiliate");

                AddFunctionMenu(FunctionPolicyNames.Payment, "Payment", "~/Payment");
                AddFunctionMenu(FunctionPolicyNames.CommissionGroup, "CommissionGroup", "~/CommGroup");
                AddFunctionMenu(FunctionPolicyNames.AffReport, "Reports", "~/Report");
            }

            AddFunctionMenu(FunctionPolicyNames.Media, "MarketingTools", "~/Media/BannerList");
            AddFunctionMenu(FunctionPolicyNames.HotNews, "Message", "~/Message");

            if (IsIT || CurrencyList.Any())
            {
                AddFunctionMenu(FunctionPolicyNames.AffExchangeRate, "Currency", "~/Exchange");
            }

            AddFunctionMenu(FunctionPolicyNames.SystemSettings, "Configurations", "~/Configuration");
            AddFunctionMenu(FunctionPolicyNames.Domain, "Domain", "~/Domain/AffDomain");
            AddFunctionMenu(FunctionPolicyNames.UserControl, "UserManagement", "~/Management/Users");
        }

        private void AddFunctionMenu(string functionName, string name, string link)
        {
            if (HasReadPolicy(functionName))
            {
                _menus.Add(new FunctionMenu()
                {
                    Name = name,
                    Link = link
                });
            }
        }
    }
}