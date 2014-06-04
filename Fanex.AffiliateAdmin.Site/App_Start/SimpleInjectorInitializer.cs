// --------------------------------------------------------------------------
// <copyright file="SimpleInjectorInitializer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Define Simple Injector</change>
// </history>

namespace Fanex.AffiliateAdmin.Site
{
    using System.Reflection;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Services.Impl;
    using Fanex.AffiliateAdmin.Services.Interface;
    using SimpleInjector;
    using SimpleInjector.Integration.Web.Mvc;

    /// <summary>
    /// Simple Injector Initializer.
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>
        /// Initialize application.
        /// </summary>
        public static void Initialize()
        {
            // Create the container as usual.
            var container = new Container();

            // Register my types, for instance:
            InitializeContainer(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        /// <summary>
        /// Initialize Container.
        /// </summary>
        /// <param name="container">Container injection.</param>
        private static void InitializeContainer(Container container)
        {
            container.Register<IAccountService, AccountService>();
            container.Register<IHomeService, HomeService>();
            container.Register<IAffiliateService, AffiliateService>();
            container.Register<IEmailHistoryService, EmailHistoryService>();
            container.Register<IDomainService, DomainService>();
            container.Register<IGroupService, GroupService>();
            container.Register<IPolicyService, PolicyService>();
            container.Register<IExchangeRateService, ExchangeRateService>();
            container.Register<ICommGroupService, CommGroupService>();
            container.Register<IPermissionCurrencyService, PermissionCurrencyService>();
            container.Register<ISystemSettingService, SystemSettingService>();
            container.Register<IPaymentService, PaymentService>();
            container.Register<IMediaService, MediaService>();
            container.Register<IMediaSettingService, MediaSettingService>();
            container.Register<ICountryService, CountryService>();
            container.Register<ITransTypeService, TransTypeService>();
            container.Register<IStatementService, StatementService>();
            container.Register<INoteService, NoteService>();
            container.Register<IViewLogService, ViewLogService>();
            container.Register<IAdjustmentService, AdjustmentService>();
            container.Register<ITrackingStatisticService, TrackingStatisticService>();
            container.Register<IReportService, ReportService>();           
            container.Register<IMessageService, MessageService>();            
        }
    }
}