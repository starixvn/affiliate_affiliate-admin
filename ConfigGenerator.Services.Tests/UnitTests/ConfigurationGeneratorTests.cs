using System.Collections.Generic;
using ConfigGenerator.Core;
using NUnit.Framework;

namespace ConfigGenerator.Services.Tests.UnitTests
{
    [TestFixture]
    public class ConfigurationGeneratorTests
    {
        [Test]
        public void Generate_Web_Config()
        {
            var service = new GeneratorService();

            var config = new ConfigArgument
                {
                    ConfigurationType = ConfigurationType.WebConfig,
                    EnvironmentType = EnvironmentType.Master,
                    SystemType = SystemType.User,
                    SelectedLicensees = new List<string> { "allbetclub" }
                };

            service.Generate(config);
        }
    }
}
