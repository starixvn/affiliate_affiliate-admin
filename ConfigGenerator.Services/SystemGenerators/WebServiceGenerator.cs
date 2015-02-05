// --------------------------------------------------------------------------
// <copyright file="WebServiceGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// Web Service Generator.
    /// </summary>
    public class WebServiceGenerator : SimpleSystemGenerator
    {
        public override string GetSystemName()
        {
            return "WebService";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.WebService);
        }
    }
}
