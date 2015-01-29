// --------------------------------------------------------------------------
// <copyright file="SpNameConfigurationGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using System.IO;
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.Tranformers;

    /// <summary>
    /// SP Name Configuration Generator.
    /// </summary>
    public class SpNameConfigurationGenerator : ConfigurationGenerator
    {
        protected override string GetTransformResult(ConfigModel configModel)
        {
            return SpNameTransformer.GetTransformResult(configModel);
        }

        protected override bool IsValidConfiguration()
        {
            return ConfigurationType.HasFlag(ConfigurationType.SpName);
        }

        protected override string GetConfigurationFolder()
        {
            return "SpNames";
        }

        protected override string GetConfigurationName()
        {
            return "SpName";
        }

        protected override string GetExtenstionName()
        {
            return "xml";
        }
    }
}