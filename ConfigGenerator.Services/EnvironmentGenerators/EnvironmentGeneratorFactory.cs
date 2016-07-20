namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using System.Collections.Generic;
    using ConfigGenerator.Core;

    /// <summary>
    /// Environment Generator Factory.
    /// </summary>
    public static class EnvironmentGeneratorFactory
    {
        /// <summary>
        /// Get Supported Environment Generators.
        /// </summary>
        /// <param name="environmentType">Environment Type.</param>
        /// <returns>Environment Generator List.</returns>
        public static IList<IEnvironmentGenerator> GetSupportedEnvironmentGenerators(EnvironmentType environmentType)
        {
            return new List<IEnvironmentGenerator>
                {
                    new LocalEnvironmentGenerator
                        {
                            EnvironmentType = environmentType
                        },
                    new MasterEnvironmentGenerator
                        {
                            EnvironmentType = environmentType
                        },
                    new UatEnvironmentGenerator
                        {
                            EnvironmentType = environmentType
                        },
                    new ProductionEnvironmentGenerator
                        {
                            EnvironmentType = environmentType
                        }
                };
        }
    }
}