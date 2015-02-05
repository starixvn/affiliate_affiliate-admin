// --------------------------------------------------------------------------
// <copyright file="ConfigurationGeneratorFactory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using System;
    using System.Collections.Generic;
    using ConfigGenerator.Core;

    /// <summary>
    /// Configuration Generator Factory.
    /// </summary>
    public static class ConfigurationGeneratorFactory
    {
        /// <summary>
        /// Get Configuration Generator.
        /// </summary>
        /// <param name="configurationType">Configuration Type.</param>
        /// <returns>Configuration Generator.</returns>
        public static IConfigurationGenerator GetConfigurationGenerator(ConfigurationType configurationType)
        {
            switch (configurationType)
            {
                case ConfigurationType.None:
                    return null;
                case ConfigurationType.WebConfig:
                    return new WebConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        };
                case ConfigurationType.AppSetting:
                    return new WebAppSettingConfigurationGenerator
                    {
                        ConfigurationType = configurationType
                    };
                case ConfigurationType.ConnectionStrings:
                    return new WebConnectionStringConfigurationGenerator
                    {
                        ConfigurationType = configurationType
                    };
                case ConfigurationType.SpName:
                    return new SPNameConfigurationGenerator
                    {
                        ConfigurationType = configurationType
                    };
                default:
                    throw new ArgumentOutOfRangeException("configurationType");
            }
        }

        /// <summary>
        /// Get Supported Configuration Generators.
        /// </summary>
        /// <param name="configurationType">Configuration Type.</param>
        /// <returns>Configuration Generator List.</returns>
        public static IList<IConfigurationGenerator> GetSupportedConfigurationGenerators(ConfigurationType configurationType)
        {
            return new List<IConfigurationGenerator>
                {
                    new WebConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        },
                    new WebAppSettingConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        },
                    new WebConnectionStringConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        },
                    new SPNameConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        }
                };
        }
    }
}