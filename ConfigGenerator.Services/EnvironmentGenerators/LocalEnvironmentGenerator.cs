using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public class LocalEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Local);
        }

        protected override string GetEnvironmentName()
        {
            return "local";
        }
    }
}