// --------------------------------------------------------------------------
// <copyright file="LocalEnvironmentGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// Local Environment Generator.
    /// </summary>
    public class LocalEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Local);
        }

        protected override string GetEnvironmentName()
        {
            return "local";
        }
    }
}