namespace ConfigGenerator
{
    /// <summary>
    /// Check condition before running generation process.
    /// </summary>
    public class CheckCondition
    {
        public int CheckedLicenseeNum { get; set; }

        public int CheckedSystemNum { get; set; }

        public int CheckedConfigurationNum { get; set; }

        public int CheckedEnvironmentNum { get; set; }
    }
}
