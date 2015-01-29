// --------------------------------------------------------------------------
// <copyright file="WebConfigurationGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.Tranformers;

    /// <summary>
    /// Web Configuration Generator.
    /// </summary>
    public class WebConfigurationGenerator : ConfigurationGenerator
    {
        protected override string GetTransformResult(ConfigModel configModel)
        {
            configModel.IsGlobal = true;

            return WebConfigTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.WebConfig);
        }

        protected override string GetConfigurationFolder()
        {
            return "WebConfigs";
        }

        protected override string GetConfigurationName()
        {
            return "Web";
        }
    }
}