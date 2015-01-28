using ConfigGenerator.Core;
using ConfigGenerator.Services.EnvironmentGenerators;

namespace ConfigGenerator.Services.SystemGenerators
{
    public abstract class ComplexSystemGenerator : SystemGenerator
    {
        public override void Generate(ConfigArgument configArgument, ConfigModel configModel)
        {
            if (!IsValidSystem())
            {
                return;
            }

            var environmentGeneratorList = EnvironmentGeneratorFactory.GetSupportedEnvironmentGenerators(configArgument.EnvironmentType);

            configModel.SystemName = GetSystemName();

            foreach (var generator in environmentGeneratorList)
            {
                generator.Generate(configArgument, configModel.Clone());
            }
        }
    }

    public abstract class SimpleSystemGenerator : SystemGenerator
    {
        public override void Generate(ConfigArgument configArgument, ConfigModel configModel)
        {
            if (!IsValidSystem())
            {
                return;
            }

            var environmentGeneratorList = EnvironmentGeneratorFactory.GetSupportedEnvironmentGenerators(configArgument.EnvironmentType);

            configModel.SystemName = GetSystemName();

            foreach (var generator in environmentGeneratorList)
            {
                generator.Generate(configArgument, configModel.Clone());
            }
        }
    }
}