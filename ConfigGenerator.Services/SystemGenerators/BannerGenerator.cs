namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Banner Generator.
    /// </summary>
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