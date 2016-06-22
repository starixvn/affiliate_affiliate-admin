namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.Tranformers;

    /// <summary>
    /// Web Application Setting Configuration Generator.
    /// </summary>
    public class WebAppSettingConfigurationGenerator : ConfigurationGenerator
    {
        public override void Generate(ConfigModel configModel)
        {
            if (CheckValidSystem(configModel))
            {
                base.Generate(configModel);
            }
        }

        protected override string GetConfigurationFolder()
        {
            return "AppSettings";
        }

        protected override string GetConfigurationName()
        {
            return "Web.AppSetting";
        }

        protected override string GetTransformResult(ConfigModel configModel)
        {
            var modelFile = configModel.GetModelFile();

            if (string.IsNullOrEmpty(modelFile))
            {
                return string.Empty;
            }

            return WebAppSettingTransformer.GetTransformResult(configModel, modelFile);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.AppSetting);
        }

        private bool CheckValidSystem(ConfigModel configModel)
        {
            return configModel.SystemType.HasFlag(SystemType.Admin)
                || configModel.SystemType.HasFlag(SystemType.User);
        }
    }
}