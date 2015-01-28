using System.IO;
using ConfigGenerator.Core;
using ConfigGenerator.Services.Tranformers;

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    public class SpNameConfigurationGenerator : ConfigurationGenerator
    {
        protected override string GetTransformResult(ConfigModel configModel)
        {
            return SpNameTransformer.GetTransformResult(configModel);
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