using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public class UatEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Uat);
        }

        protected override string GetEnvironmentName()
        {
            return "uat";
        }
    }
}