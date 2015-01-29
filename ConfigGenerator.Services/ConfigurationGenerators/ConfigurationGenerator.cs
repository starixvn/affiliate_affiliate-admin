// --------------------------------------------------------------------------
// <copyright file="ConfigurationGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using System.IO;
    using ConfigGenerator.Core;

    /// <summary>
    /// Configuration Generator.
    /// </summary>
    public abstract class ConfigurationGenerator : IConfigurationGenerator
    {
        public ConfigurationType ConfigurationType { get; set; }

        /// <summary>
        /// Generate Method.
        /// </summary>
        /// <param name="configModel">Configuration Model.</param>
        public virtual void Generate(ConfigModel configModel)
        {
            if (!IsValidConfiguration())
            {
                return;
            }

            configModel.ConfigFolder = GetConfigurationFolder();

            var path = string.Format(
                @"{0}\{1}\{2}",
                Utility.GetConfigurationFolder(),
                configModel.SystemName,
                configModel.ConfigFolder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var configName = path + string.Format(
                @"\{0}.{1}.{2}.{3}",
                GetConfigurationName(),
                configModel.Licensee,
                configModel.EnvironmentName,
                GetExtenstionName());

            var result = GetTransformResult(configModel);

            File.WriteAllText(configName, result);
        }

        protected abstract string GetConfigurationFolder();

        protected abstract string GetConfigurationName();

        protected virtual string GetExtenstionName()
        {
            return "config";
        }

        protected virtual string GetTransformResult(ConfigModel configModel)
        {
            return "Output here";
        }

        protected abstract bool IsValidConfiguration();
    }
}