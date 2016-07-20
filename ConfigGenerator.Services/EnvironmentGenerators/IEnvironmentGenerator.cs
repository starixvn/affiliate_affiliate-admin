namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    public interface IEnvironmentGenerator
    {
        EnvironmentType EnvironmentType { get; set; }

        void Generate(ConfigArgument configArgument, ConfigModel configModel);
    }
}