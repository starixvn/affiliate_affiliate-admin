using ConfigGenerator.Core;
using ConfigGenerator.Services.EnvironmentGenerators;

namespace ConfigGenerator.Services.SystemGenerators
{
    public class BannerGenerator : SimpleSystemGenerator
    {
        public override string GetSystemName()
        {
            return "Banner";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.Banner);
        }
    }
}
