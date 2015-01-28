using System;
using System.IO;

namespace ConfigGenerator.Core
{
    public class ConfigModel
    {
        public string SystemName { get; set; }

        public SystemType SystemType { get; set; }

        public string Licensee { get; set; }

        public string ConfigFolder { get; set; }

        public string EnvironmentName { get; set; }

        public bool IsGlobal { get; set; }

        public ConfigModel Clone()
        {
            return new ConfigModel
                {
                    SystemName = SystemName,
                    SystemType = SystemType,
                    Licensee = Licensee,
                    ConfigFolder = ConfigFolder,
                    EnvironmentName = EnvironmentName
                };
        }

        public string GetModelFile()
        {
            var fullPath = string.Format(@"{0}\{1}", Utility.GetModelFolder(), GetModelPath());

            if (!Directory.Exists(fullPath))
            {
                return string.Empty;
            }

            var directory = new DirectoryInfo(fullPath);
            var files = directory.GetFiles(GetFileFilter());

            if (files.GetLength(0) != 1)
            {
                return string.Empty;
            }

            return files[0].FullName;
        }

        public string GetModelPath()
        {
            if (ConfigFolder.Equals("SpNames", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(@"{0}\{1}", SystemName, "ConnectionStrings");
            }
                
            return string.Format(@"{0}\{1}", SystemName, ConfigFolder);
        }

        public string GetFileFilter()
        {
            if (IsGlobal)
            {
                return "*.xml";
            }

            return "*." + Licensee + ".xml";
        }

        public string GetUserConnectionStringModelFile()
        {
            const string modelPath = @"User\ConnectionStrings";

            var fullPath = string.Format(@"{0}\{1}", Utility.GetModelFolder(), modelPath);

            if (!Directory.Exists(fullPath))
            {
                return string.Empty;
            }

            var directory = new DirectoryInfo(fullPath);

            IsGlobal = false;

            var files = directory.GetFiles(GetFileFilter());

            if (files.GetLength(0) != 1)
            {
                return string.Empty;
            }

            return files[0].FullName;
        }

        public object GetUserWebModelFile()
        {
            const string modelPath = @"User\WebConfigs";

            var fullPath = string.Format(@"{0}\{1}", Utility.GetModelFolder(), modelPath);

            if (!Directory.Exists(fullPath))
            {
                return string.Empty;
            }

            var directory = new DirectoryInfo(fullPath);

            IsGlobal = true;

            var files = directory.GetFiles(GetFileFilter());

            if (files.GetLength(0) != 1)
            {
                return string.Empty;
            }

            return files[0].FullName;
        }
    }
}