// --------------------------------------------------------------------------
// <copyright file="UserGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// User Generator.
    /// </summary>
    public class UserGenerator : ComplexSystemGenerator
    {
        public override string GetSystemName()
        {
            return "User";
        }

        protected override bool IsValidSystem()
        {
            return SystemType.HasFlag(SystemType.User);
        }
    }
}
