using ConfigGenerator.Core;

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    public class MasterEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Master);
        }

        protected override string GetEnvironmentName()
        {
            return "master";
        }
    }
}