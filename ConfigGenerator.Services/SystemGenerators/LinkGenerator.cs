using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
    public class LinkGenerator : SimpleSystemGenerator
    {
        public override string GetSystemName()
        {
            return "Link";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.Link);
        }
    }
}
