using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
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
