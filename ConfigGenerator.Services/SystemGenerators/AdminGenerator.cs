namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Admin Generator.
    /// </summary>
    public class AdminGenerator : ComplexSystemGenerator
    {
        public override string GetSystemName()
        {
            return "Admin";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.Admin);
        }
    }
}
