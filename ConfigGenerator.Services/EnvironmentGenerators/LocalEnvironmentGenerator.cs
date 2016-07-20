namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Local Environment Generator.
    /// </summary>
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