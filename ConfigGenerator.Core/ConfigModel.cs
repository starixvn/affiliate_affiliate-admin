namespace ConfigGenerator.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// Configuration Model.
    /// </summary>
    public class ConfigModel
    {
        public string ConfigFolder { get; set; }

        public string EnvironmentName { get; set; }

        public bool IsGlobal { get; set; }

        public string Licensee { get; set; }

        public string SystemName { get; set; }

        public SystemType SystemType { get; set; }

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

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
        public string GetFileFilter()
        {
            if (IsGlobal)
            {
                return "*.xml";
            }

            return "*." + Licensee + ".xml";
        }

        /// <summary>
        /// Get Model File.
        /// </summary>
        /// <returns>Model file.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
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

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
        public string GetModelPath()
        {
            if (ConfigFolder.Equals("SpNames", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format(@"{0}\{1}", SystemName, "ConnectionStrings");
            }

            return string.Format(@"{0}\{1}", SystemName, ConfigFolder);
        }

        /// <summary>
        /// Get connection string of affiliate user.
        /// </summary>
        /// <returns>Connection string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
        public string GetUserConnectionStringModelFile()
        {
            const string ModelPath = @"User\ConnectionStrings";

            var fullPath = string.Format(@"{0}\{1}", Utility.GetModelFolder(), ModelPath);

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

        /// <summary>
        /// Get user web model file.
        /// </summary>
        /// <returns>Web model file.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        public object GetUserWebModelFile()
        {
            const string ModelPath = @"User\WebConfigs";

            var fullPath = string.Format(@"{0}\{1}", Utility.GetModelFolder(), ModelPath);

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