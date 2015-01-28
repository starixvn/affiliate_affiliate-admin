using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
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
