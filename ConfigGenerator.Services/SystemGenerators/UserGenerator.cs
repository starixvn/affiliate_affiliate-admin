namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// User Generator.
    /// </summary>
    public class UserGenerator : ComplexSystemGenerator
    {
        public override string GetSystemName()
        {
            return "User";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.User);
        }
    }
}
