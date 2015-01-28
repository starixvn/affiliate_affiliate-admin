using System;
using System.IO;
using ConfigGenerator.Core;

namespace ConfigGenerator
{
    public class MoveUtil
    {
        private const string Backupdirectory = @"Backup";

        public static void MoveConfig(SystemType systemType)
        {
            if (systemType.HasFlag(SystemType.Admin))
            {
                MoveAdminConfig();
            }

            if (systemType.HasFlag(SystemType.User))
            {
                MoveUserConfig();
            }

            if (systemType.HasFlag(SystemType.Banner))
            {
                MoveBannerConfig();
            }

            if (systemType.HasFlag(SystemType.Link))
            {
                MoveLinkConfig();
            }

            if (systemType.HasFlag(SystemType.WebService))
            {
                MoveWebServiceConfig();
            }
        }

        private static void MoveWebServiceConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "WebService");
            var webserviceConfigFolder = Utility.GetWebServiceConfigurationFolder();
            MoveData(configFolder, webserviceConfigFolder);
        }

        private static void MoveLinkConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Link");
            var linkConfigFolder = Utility.GetLinkConfigurationFolder();
            MoveData(configFolder, linkConfigFolder);
        }

        private static void MoveBannerConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Banner");
            var bannerConfigFolder = Utility.GetBannerConfigurationFolder();
            MoveData(configFolder, bannerConfigFolder);
        }

        private static void MoveUserConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "User");
            var userConfigFolder = Utility.GetUserConfigurationFolder();
            MoveData(configFolder, userConfigFolder);
        }

        private static void MoveAdminConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Admin");
            var adminConfigFolder = Utility.GetAdminConfigurationFolder();
            MoveData(configFolder, adminConfigFolder);
        }

        private static void MoveData(string configFolder, string adminConfigFolder)
        {
            if (Directory.Exists(adminConfigFolder))
            {
                var configDir = new DirectoryInfo(configFolder);
                var subDirs = configDir.GetDirectories();

                foreach (var subDir in subDirs)
                {
                    var files = subDir.GetFiles();
                    foreach (var file in files)
                    {
                        var destPath = string.Format(@"{0}\{1}", adminConfigFolder, subDir.Name);
                        var dest = string.Format(@"{0}\{1}\{2}", adminConfigFolder, subDir.Name, file.Name);

                        if (!File.Exists(dest) && !Directory.Exists(destPath))
                        {
                            Directory.CreateDirectory(destPath);
                        }

                        file.CopyTo(dest,  true);
                    }
                }
            }
            else
            {
                Directory.Move(configFolder, adminConfigFolder);
            }
        }
    }
}