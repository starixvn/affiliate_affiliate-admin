namespace ConfigGenerator.Services
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.SystemGenerators;

    /// <summary>
    /// Generator Service.
    /// </summary>
    public class GeneratorService
    {
        public void Generate(ConfigArgument configArgument)
        {
            foreach (var licensee in configArgument.SelectedLicensees)
            {
                GenerateOneLicensee(configArgument, licensee);
            }
        }

        /// <summary>
        /// Generate Configuration Of One-Specific Licensee.
        /// </summary>
        /// <param name="configArgument">Configuration Argument.</param>
        /// <param name="licenseeName">Licensee Name.</param>
        private void GenerateOneLicensee(ConfigArgument configArgument, string licenseeName)
        {
            var systemGeneratorList = SystemGeneratorFactory.GetSupportedSystemGenerators(configArgument.SystemType);

            var licensee = new ConfigModel
                {
                    Licensee = licenseeName,
                    SystemType = configArgument.SystemType
                };

            foreach (var generator in systemGeneratorList)
            {
                generator.Generate(configArgument, licensee);
            }
        }
    }
}