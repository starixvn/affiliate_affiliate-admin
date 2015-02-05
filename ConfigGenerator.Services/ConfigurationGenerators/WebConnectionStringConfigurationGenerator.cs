// --------------------------------------------------------------------------
// <copyright file="WebConnectionStringConfigurationGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// Web Connection String Configuration Generator.
    /// </summary>
    public class WebConnectionStringConfigurationGenerator : ConfigurationGenerator
    {
        public override void Generate(ConfigModel configModel)
        {
            if (CheckValidSystem(configModel))
            {
                base.Generate(configModel);
            }
        }

        protected override string GetConfigurationFolder()
        {
            return "ConnectionStrings";
        }

        protected override string GetConfigurationName()
        {
            return "Web.ConnectionString";
        }

        protected override string GetTransformResult(ConfigModel configModel)
        {
            return WebConnectionStringTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.ConnectionStrings);
        }

        private bool CheckValidSystem(ConfigModel configModel)
        {
            return configModel.SystemType.HasFlag(SystemType.Admin)
                || configModel.SystemType.HasFlag(SystemType.User)
                || configModel.SystemType.HasFlag(SystemType.WebService);
        }
    }
}