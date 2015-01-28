using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public interface IEnvironmentGenerator
    {
        EnvironmentType EnvironmentType { get; set; }

        void Generate(ConfigArgument configArgument, ConfigModel configModel);
    }
}