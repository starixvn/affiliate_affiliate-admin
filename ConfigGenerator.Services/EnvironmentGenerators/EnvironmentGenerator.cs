using ConfigGenerator.Core;
using ConfigGenerator.Services.ConfigurationGenerators;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public abstract class EnvironmentGenerator : IEnvironmentGenerator
    {
        public EnvironmentType EnvironmentType { get; set; }

        public virtual void Generate(ConfigArgument configArgument, ConfigModel configModel)
        {
            if (!IsValidEnvironment())
            {
                return;
            }

            var configurationGeneratorList = ConfigurationGeneratorFactory.GetSupportedConfigurationGenerators(configArgument.ConfigurationType);

            foreach (var generator in configurationGeneratorList)
            {
                var configModel2 = configModel.Clone();

                configModel2.EnvironmentName = GetEnvironmentName();

                generator.Generate(configModel2);
            }
        }

        protected abstract bool IsValidEnvironment();

        protected virtual string GetEnvironmentName()
        {
            return EnvironmentType.ToString().ToLower();
        }
    }
}