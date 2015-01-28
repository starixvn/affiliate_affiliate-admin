using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
    public interface ISystemGenerator
    {
        void Generate(ConfigArgument configArgument, ConfigModel configModel);
    }
}