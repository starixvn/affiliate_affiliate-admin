namespace ConfigGenerator.Services
{
    using System.Collections.Generic;
    using ConfigGenerator.Core;

    /// <summary>
    /// Configuration Argument.
    /// </summary>
    public class ConfigArgument
    {
        public ConfigurationType ConfigurationType { get; set; }

        public EnvironmentType EnvironmentType { get; set; }

        public IEnumerable<string> SelectedLicensees { get; set; }

        public SystemType SystemType { get; set; }
    }
}