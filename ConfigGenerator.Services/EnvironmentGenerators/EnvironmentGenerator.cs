namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using System.Diagnostics.CodeAnalysis;
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.ConfigurationGenerators;

    /// <summary>
    /// Environment Generator.
    /// </summary>
    public abstract class EnvironmentGenerator : IEnvironmentGenerator
    {
        public EnvironmentType EnvironmentType { get; set; }

        /// <summary>
        /// Generate Method.
        /// </summary>
        /// <param name="configArgument">Configuration Argument.</param>
        /// <param name="configModel">Configuration Model.</param>
        public virtual void Generate(ConfigArgument configArgument, ConfigModel configModel)
        {
            if (!IsValidEnvironment())
            {
                return;
            }

            var configurationGeneratorList = ConfigurationGeneratorFactory.GetSupportedConfigurationGenerators(configArgument.ConfigurationType);

            foreach (var generator in configurationGeneratorList)
            {
                var configModel2 = configModel.Clone();

                configModel2.EnvironmentName = GetEnvironmentName();

                generator.Generate(configModel2);
            }
        }
        
        protected abstract bool IsValidEnvironment();

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accepted")]
        protected virtual string GetEnvironmentName()
        {
            return EnvironmentType.ToString().ToLower();
        }
    }
}