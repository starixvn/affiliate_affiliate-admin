namespace ConfigGenerator
{
    using System.IO;
    using ConfigGenerator.Core;

    /// <summary>
    /// Move configuration utility.
    /// </summary>
    public class MoveUtil
    {
        /// <summary>
        /// Move configuration.
        /// </summary>
        /// <param name="systemType">System type.</param>
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

        /// <summary>
        /// Move admin configuration.
        /// </summary>
        private static void MoveAdminConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Admin");
            var adminConfigFolder = Utility.GetAdminConfigurationFolder();
            MoveData(configFolder, adminConfigFolder);
        }

        /// <summary>
        /// Move banner configuration.
        /// </summary>
        private static void MoveBannerConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Banner");
            var bannerConfigFolder = Utility.GetBannerConfigurationFolder();
            MoveData(configFolder, bannerConfigFolder);
        }

        /// <summary>
        /// Move configuration files between source folder and destination folder.
        /// </summary>
        /// <param name="sourceFolder">Source folder.</param>
        /// <param name="destinationFolder">Destination folder.</param>
        private static void MoveData(string sourceFolder, string destinationFolder)
        {
            if (Directory.Exists(destinationFolder))
            {
                var configDir = new DirectoryInfo(sourceFolder);
                var subDirs = configDir.GetDirectories();

                foreach (var subDir in subDirs)
                {
                    var files = subDir.GetFiles();
                    foreach (var file in files)
                    {
                        var destPath = string.Format(@"{0}\{1}", destinationFolder, subDir.Name);
                        var dest = string.Format(@"{0}\{1}\{2}", destinationFolder, subDir.Name, file.Name);

                        if (!File.Exists(dest) && !Directory.Exists(destPath))
                        {
                            Directory.CreateDirectory(destPath);
                        }

                        file.CopyTo(dest, true);
                    }
                }
            }
            else
            {
                Directory.Move(sourceFolder, destinationFolder);
            }
        }

        /// <summary>
        /// Move affiliate link configuration.
        /// </summary>
        private static void MoveLinkConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "Link");
            var linkConfigFolder = Utility.GetLinkConfigurationFolder();
            MoveData(configFolder, linkConfigFolder);
        }

        /// <summary>
        /// Move affiliate user configuration.
        /// </summary>
        private static void MoveUserConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "User");
            var userConfigFolder = Utility.GetUserConfigurationFolder();
            MoveData(configFolder, userConfigFolder);
        }

        /// <summary>
        /// Move web service configuration.
        /// </summary>
        private static void MoveWebServiceConfig()
        {
            var configFolder = string.Format(@"{0}\{1}", Utility.GetConfigurationFolder(), "WebService");
            var webserviceConfigFolder = Utility.GetWebServiceConfigurationFolder();
            MoveData(configFolder, webserviceConfigFolder);
        }
    }
}