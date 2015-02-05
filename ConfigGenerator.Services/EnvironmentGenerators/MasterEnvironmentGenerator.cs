// --------------------------------------------------------------------------
// <copyright file="MasterEnvironmentGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.EnvironmentGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Master Environment Generator.
    /// </summary>
    public class MasterEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Master);
        }

        protected override string GetEnvironmentName()
        {
            return "master";
        }
    }
}