// --------------------------------------------------------------------------
// <copyright file="SubMenu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.25">Created</change>
//   <change who="Leo.Vo" date="2013.06.30">GetLanguageOriginal</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// SubMenu class.
    /// </summary>
    public static class SubMenu
    {
        /// <summary>
        /// Gets the Home Menu link.
        /// </summary>
        /// <value>Return Home Menu link..</value>
        public static IList<Menu> Home
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.Home, Link = "~/" },
                    new Menu { Text = Lang.CurrOnlineList, Link = "~/Home/UserOnlineList" }
                };
            }
        }

        /// <summary>
        /// Gets Affiliate menu list.
        /// </summary>
        /// <value>Return Affiliate menu list.</value>
        public static IList<Menu> Affiliate
        {
            get
            {
                return new List<Menu>
                    {
                        new Menu { Text = Lang.MyFavorite, Link = "~/Affiliate/Index?isfavorite=true" },
                        new Menu { Text = Lang.Affiliate, Link = "~/Affiliate/Index" },
                        new Menu { Text = Lang.Member, Link = "~/Affiliate/MemberList" },
                        new Menu { Text = Lang.AffiliateFeedback, Link = "~/EmailHistory" }
                    };
            }
        }

        /// <summary>
        /// Gets Domain list link.
        /// </summary>
        /// <value>Return list link.</value>
        public static IList<Menu> CommGroup
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.CommissionGroup, Link = "~/CommGroup" }
                };
            }
        }

        /// <summary>
        /// Gets Commission group link.
        /// </summary>
        /// <value>Return list link.</value>
        public static IList<Menu> Exchange
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.CurrencyRates, Link = "~/Exchange" }
                };
            }
        }

        /// <summary>
        /// Gets Domain list link.
        /// </summary>
        /// <value>Return list link.</value>
        public static IList<Menu> Domain
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.AffDomain, Link = "~/Domain/AffDomain" },
                    new Menu { Text = Lang.MemberDomain, Link = "~/Domain/MemDomain" },
                    new Menu { Text = Lang.DomainControl, Link = "~/Domain/DomainControl" }
                };
            }
        }

        /// <summary>
        /// Gets User Management Menu.
        /// </summary>
        /// <value>Return Management Menu.</value>
        public static IList<Menu> Management
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.UserManagement, Link = "~/Management/Users" },
                    new Menu { Text = Lang.GroupManagement, Link = "~/Management/Groups" }
                };
            }
        }

        /// <summary>
        /// Gets Media Menu.
        /// </summary>
        /// <value>Return Media Menu.</value>
        public static IList<Menu> Media
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.BannerList, Link = "~/Media/BannerList" },
                    new Menu { Text = Lang.TextList, Link = "~/Media/TextList" }
                };
            }
        }

        /// <summary>
        /// Gets Message Menu.
        /// </summary>
        /// <value>Return Message Menu.</value>
        public static IList<Menu> Message
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.Message, Link = "~/Message" }
                };
            }
        }

        /// <summary>
        /// Gets Payment Menu.
        /// </summary>
        /// <value>Return Payment Menu.</value>
        public static IList<Menu> Payment
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.Payment, Link = "~/Payment" }
                };
            }
        }

        public static IList<Menu> Configuration
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.Configurations, Link = "~/Configuration" },
                    new Menu { Text = Lang.Country, Link = "~/Configuration/Country" },
                    new Menu { Text = Lang.Product, Link = "~/Configuration/Product" },
                    new Menu { Text = Lang.Language, Link = "~/Configuration/Language" },
                    new Menu { Text = Lang.AffType, Link = "~/Configuration/AffType" }
                };
            }
        }

        public static IList<Menu> AffiliateInfo
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.StatementHistory, Link = "~/Statement" },
                    new Menu { Text = Lang.Edit, Link = "~/AffiliateInfo/Edit" },
                    new Menu { Text = Lang.Note, Link = "~/Note" },
                    new Menu { Text = Lang.Adjustment, Link = "~/Adjustment" },
                    new Menu { Text = Lang.Member, Link = "~/Affiliate/MemberListByAffiliate" },
                    new Menu 
                    { 
                        Text = Lang.Statistics, 
                        Link = "~/TrackingStatistic/Referrer",
                        SubMenu = new List<Menu>
                            {
                                new Menu { Text = Lang.ReferrersStatistics, Link = "~/TrackingStatistic/Referrer" },
                                new Menu { Text = Lang.LandingPagesStatistics, Link = "~/TrackingStatistic/LandingPage" }
                            }
                    },                    
                    new Menu
                    {
                        Text = Lang.Reports, 
                        Link = "~/Report/CompanyWinlossByAffiliate",
                        SubMenu = new List<Menu>
                            {
                                new Menu { Text = Lang.CompanyWinloss, Link = "~/Report/CompanyWinlossByAffiliate" },
                                new Menu { Text = Lang.MemberWinloss, Link = "~/Report/MemberWinlossByAffiliate" },
                                new Menu { Text = Lang.AffiliateWinLoss, Link = "~/Report/AffiliateWinLossByAffiliate" },
                                new Menu { Text = Lang.AffiliateCommission, Link = "~/Report/AffiliateCommissionByAffiliate" }
                            }
                    },
                    new Menu { Text = Lang.ViewLog, Link = "~/ViewLog" }
                };
            }
        }

        public static IList<Menu> AffiliateNotApprove
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.Edit, Link = "~/AffiliateInfo/Edit" },
                    new Menu { Text = Lang.Note, Link = "~/Note" }
                };
            }
        }

        public static IList<Menu> Reports
        {
            get
            {
                return new List<Menu>
                {
                    new Menu { Text = Lang.CompanyWinloss, Link = "~/Report/CompanyWinloss" },
                    new Menu { Text = Lang.MemberStatistics, Link = "~/Report/MemberStatistics" },
                    new Menu { Text = Lang.AffiliateWinLoss, Link = "~/Report/AffiliateWinLoss" },
                    new Menu { Text = Lang.MemberWinloss, Link = "~/Report/MemberWinloss" },
                    new Menu { Text = Lang.AffiliateCommission, Link = "~/Report/AffiliateCommission" }
                };
            }
        }

        public static IEnumerable<Menu> GetPermissionMenus(bool isIT)
        {
            var menus = new List<Menu>
                {
                    new Menu { Text = Lang.FunctionPermission, Link = "~/Management/ChangePolicyUser" }
                };

            if (!isIT)
            {
                menus.Add(new Menu { Text = Lang.CurrenciesPermission, Link = "~/Management/ChangeCurrencyUser" });
            }

            return menus;
        }
    }
}