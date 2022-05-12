using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System.Collections.Generic;

namespace Fanex.AffiliateAdmin.Site
{
    public static class LoggingConfig
    {
        public static void RegisterSentry()
        {
            var appSetting = AppSetting.Instance;
            var env = appSetting.Environment;
            var dsn = appSetting.GetValue("SentryDsn");
            var category = appSetting.GetValue("LoggingCategory");

            if (!string.IsNullOrEmpty(dsn))
            {
                LogManager.SetDefaultLogCategory(category)
                    .Use(new List<ILogEngine>
                    {
                        new SentryLogging(new SentryEngineOptions
                        {
                            Dsn = new Dsn(dsn),
                            Environment = env
                        })
                    });
            }
        }
    }
}