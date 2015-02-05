// --------------------------------------------------------------------------
// <copyright file="ISystemGenerator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.SystemGenerators
{
    using ConfigGenerator.Core;

    public interface ISystemGenerator
    {
        void Generate(ConfigArgument configArgument, ConfigModel configModel);
    }
}