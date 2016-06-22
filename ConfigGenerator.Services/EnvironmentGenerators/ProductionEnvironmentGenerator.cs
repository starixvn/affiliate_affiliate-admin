namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Production Environment Generator.
    /// </summary>
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