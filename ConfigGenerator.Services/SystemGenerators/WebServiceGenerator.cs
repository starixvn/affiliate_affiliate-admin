namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Web Service Generator.
    /// </summary>
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
