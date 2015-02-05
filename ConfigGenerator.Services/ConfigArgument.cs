// --------------------------------------------------------------------------
// <copyright file="ConfigArgument.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

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