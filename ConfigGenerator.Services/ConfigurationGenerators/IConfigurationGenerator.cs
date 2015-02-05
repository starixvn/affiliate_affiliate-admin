// --------------------------------------------------------------------------
// <copyright file="IConfigurationGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    using ConfigGenerator.Core;

    public interface IConfigurationGenerator
    {
        ConfigurationType ConfigurationType { get; set; }

        void Generate(ConfigModel configModel);
    }
}