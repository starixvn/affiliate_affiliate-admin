namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Master Environment Generator.
    /// </summary>
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