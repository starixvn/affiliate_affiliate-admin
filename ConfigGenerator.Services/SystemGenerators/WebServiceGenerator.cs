using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
    public class WebServiceGenerator : SimpleSystemGenerator
    {
        public override string GetSystemName()
        {
            return "WebService";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.WebService);
        }
    }
}
