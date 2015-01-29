// --------------------------------------------------------------------------
// <copyright file="SystemGenerator.cs" company="Nexcel Solutions Vietnam">
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
    /// System Generator.
    /// </summary>
    public abstract class SystemGenerator : ISystemGenerator
    {
        public SystemType SystemType { get; set; }

        public abstract void Generate(ConfigArgument configArgument, ConfigModel configModel);

        public abstract string GetSystemName();

        protected abstract bool IsValidSystem();
    }
}