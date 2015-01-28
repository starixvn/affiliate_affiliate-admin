using ConfigGenerator.Core;

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    public interface IConfigurationGenerator
    {
        ConfigurationType ConfigurationType { get; set; }

        void Generate(ConfigModel configModel);
    }
}