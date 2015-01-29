// --------------------------------------------------------------------------
// <copyright file="SimpleSystemGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.EnvironmentGenerators;

    /// <summary>
    /// Simple System Generator.
    /// </summary>
    public abstract class SimpleSystemGenerator : SystemGenerator
    {
        /// <summary>
        /// Generate Method.
        /// </summary>
        /// <param name="configArgument">Configuration Argument.</param>
        /// <param name="configModel">Configuration Model.</param>
        public override void Generate(ConfigArgument configArgument, ConfigModel configModel)
        {
            if (!IsValidSystem())
            {
                return;
            }

            var environmentGeneratorList = EnvironmentGeneratorFactory.GetSupportedEnvironmentGenerators(configArgument.EnvironmentType);

            configModel.SystemName = GetSystemName();

            foreach (var generator in environmentGeneratorList)
            {
                generator.Generate(configArgument, configModel.Clone());
            }
        }
    }
}