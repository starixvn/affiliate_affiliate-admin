namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    public interface ISystemGenerator
    {
        void Generate(ConfigArgument configArgument, ConfigModel configModel);
    }
}