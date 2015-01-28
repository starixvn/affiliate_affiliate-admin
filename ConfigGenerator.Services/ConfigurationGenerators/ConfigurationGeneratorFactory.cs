using System;
using System.Collections.Generic;
using ConfigGenerator.Core;

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    public static class ConfigurationGeneratorFactory
    {
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
                    new SpNameConfigurationGenerator
                        {
                            ConfigurationType = configurationType
                        }
                };
        }

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
                    return new SpNameConfigurationGenerator
                    {
                        ConfigurationType = configurationType
                    };
                default:
                    throw new ArgumentOutOfRangeException("configurationType");
            }
        }
    }
}