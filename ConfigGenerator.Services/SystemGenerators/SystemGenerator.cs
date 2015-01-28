using ConfigGenerator.Core;
using ConfigGenerator.Services.ConfigurationGenerators;

namespace ConfigGenerator.Services.SystemGenerators
{
    public abstract class SystemGenerator : ISystemGenerator
    {
        public SystemType SystemType { get; set; }

        public abstract string GetSystemName();

        public abstract void Generate(ConfigArgument configArgument, ConfigModel configModel);

        protected abstract bool IsValidSystem();
    }
}