using System.Collections.Generic;
using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public static class EnvironmentGeneratorFactory
    {
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