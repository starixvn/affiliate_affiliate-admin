namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// UAT Environment Generator.
    /// </summary>
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