namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using System.IO;
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.Tranformers;

    /// <summary>
    /// SP Name Configuration Generator.
    /// </summary>
    public class SPNameConfigurationGenerator : ConfigurationGenerator
    {
        protected override string GetTransformResult(ConfigModel configModel)
        {
            return SPNameTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.SpName);
        }

        protected override string GetConfigurationFolder()
        {
            return "SpNames";
        }

        protected override string GetConfigurationName()
        {
            return "SpName";
        }

        protected override string GetExtenstionName()
        {
            return "xml";
        }
    }
}