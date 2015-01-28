using System.IO;
using ConfigGenerator.Core;
using ConfigGenerator.Services.Tranformers;

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    public class WebConnectionStringConfigurationGenerator : ConfigurationGenerator
    {
        public override void Generate(ConfigModel configModel)
        {
            if (CheckValidSystem(configModel))
            {
                base.Generate(configModel);
            }
        }

        private bool CheckValidSystem(ConfigModel configModel)
        {
            return configModel.SystemType.HasFlag(SystemType.Admin)
                || configModel.SystemType.HasFlag(SystemType.User)
                || configModel.SystemType.HasFlag(SystemType.WebService);
        }

        protected override string GetTransformResult(ConfigModel configModel)
        {
            return WebConnectionStringTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.ConnectionStrings);
        }

        protected override string GetConfigurationFolder()
        {
            return "ConnectionStrings";
        }

        protected override string GetConfigurationName()
        {
            return "Web.ConnectionString";
        }
    }
}