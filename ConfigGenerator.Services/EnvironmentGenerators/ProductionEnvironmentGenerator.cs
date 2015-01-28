using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public class ProductionEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Production);
        }

        protected override string GetEnvironmentName()
        {
            return "production";
        }
    }
}