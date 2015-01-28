using System;
using System.Collections.Generic;
using System.Linq;
using ConfigGenerator.Core;
using ConfigGenerator.Services.SystemGenerators;

namespace ConfigGenerator.Services
{
    public class ConfigArgument
    {
        public SystemType SystemType { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public EnvironmentType EnvironmentType { get; set; }

        public IEnumerable<string> SelectedLicensees { get; set; }
    }

    public class GeneratorService
    {
        public void Generate(ConfigArgument configArgument)
        {
            foreach (var licensee in configArgument.SelectedLicensees)
            {
                GenerateOneLicensee(configArgument, licensee);
            }
        }

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
