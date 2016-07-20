namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Link Generator.
    /// </summary>
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
