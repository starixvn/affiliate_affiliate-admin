namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.EnvironmentGenerators;

    /// <summary>
    /// Simple System Generator.
    /// </summary>
    public abstract class SimpleSystemGenerator : SystemGenerator
    {
        /// <summary>
        /// Generate Method.
        /// </summary>
        /// <param name="configArgument">Configuration Argument.</param>
        /// <param name="configModel">Configuration Model.</param>
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