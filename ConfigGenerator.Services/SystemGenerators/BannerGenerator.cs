// --------------------------------------------------------------------------
// <copyright file="BannerGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    /// <summary>
    /// Banner Generator.
    /// </summary>
    public class BannerGenerator : SimpleSystemGenerator
    {
        public override string GetSystemName()
        {
            return "Banner";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.Banner);
        }
    }
}