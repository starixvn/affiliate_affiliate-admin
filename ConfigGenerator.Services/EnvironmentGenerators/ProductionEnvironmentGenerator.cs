// --------------------------------------------------------------------------
// <copyright file="ProductionEnvironmentGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// Production Environment Generator.
    /// </summary>
    public class ProductionEnvironmentGenerator : EnvironmentGenerator
    {
        protected override bool IsValidEnvironment()
        {
            return EnvironmentType.HasFlag(EnvironmentType.Production);
        }

        protected override string GetEnvironmentName()
        {
            return "production";
        }
    }
}