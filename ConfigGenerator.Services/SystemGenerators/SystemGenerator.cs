namespace ConfigGenerator.Services.SystemGenerators
{
    using System.Diagnostics.CodeAnalysis;
    using Core;

    /// <summary>
    /// System Generator.
    /// </summary>
    public abstract class SystemGenerator : ISystemGenerator
    {
        public SystemType SystemType { get; set; }

        public abstract void Generate(ConfigArgument configArgument, ConfigModel configModel);

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accepted")]
        public abstract string GetSystemName();

        protected abstract bool IsValidSystem();
    }
}