namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.Tranformers;

    /// <summary>
    /// Web Configuration Generator.
    /// </summary>
    public class WebConfigurationGenerator : ConfigurationGenerator
    {
        protected override string GetTransformResult(ConfigModel configModel)
        {
            configModel.IsGlobal = true;

            return WebConfigTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.WebConfig);
        }

        protected override string GetConfigurationFolder()
        {
            return "WebConfigs";
        }

        protected override string GetConfigurationName()
        {
            return "Web";
        }
    }
}