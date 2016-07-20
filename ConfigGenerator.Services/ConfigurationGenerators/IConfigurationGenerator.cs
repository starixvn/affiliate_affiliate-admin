namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using ConfigGenerator.Core;

    public interface IConfigurationGenerator
    {
        ConfigurationType ConfigurationType { get; set; }

        void Generate(ConfigModel configModel);
    }
}