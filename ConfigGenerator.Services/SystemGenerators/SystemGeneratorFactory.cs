// --------------------------------------------------------------------------
// <copyright file="SystemGeneratorFactory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.SystemGenerators
{
    using System;
    using System.Collections.Generic;
    using ConfigGenerator.Core;

    /// <summary>
    /// System Generator Factory.
    /// </summary>
    public class SystemGeneratorFactory
    {
        /// <summary>
        /// Get Supported System Generators.
        /// </summary>
        /// <param name="systemType">System Type.</param>
        /// <returns>System Generator List.</returns>
        public static IList<ISystemGenerator> GetSupportedSystemGenerators(SystemType systemType)
        {
            return new List<ISystemGenerator>
                {
                    new AdminGenerator
                        {
                            SystemType = systemType
                        },
                    new UserGenerator
                     {
                            SystemType = systemType
                        },
                    new BannerGenerator
                     {
                            SystemType = systemType
                        },
                    new LinkGenerator
                     {
                            SystemType = systemType
                        },
                    new WebServiceGenerator
                     {
                            SystemType = systemType
                        },
                };
        }

        /// <summary>
        /// Get System Generator.
        /// </summary>
        /// <param name="systemType">System Type.</param>
        /// <returns>System Generator.</returns>
        public static ISystemGenerator GetSystemGenerator(SystemType systemType)
        {
            switch (systemType)
            {
                case SystemType.None:
                    return null;
                case SystemType.Admin:
                    return new AdminGenerator
                         {
                             SystemType = systemType
                         };
                case SystemType.User:
                    return new UserGenerator
                        {
                            SystemType = systemType
                        };

                case SystemType.Banner:
                    return new BannerGenerator
                        {
                            SystemType = systemType
                        };

                case SystemType.Link:
                    return new LinkGenerator
                        {
                            SystemType = systemType
                        };

                case SystemType.WebService:
                    return new WebServiceGenerator
                        {
                            SystemType = systemType
                        };

                default:
                    throw new ArgumentOutOfRangeException("systemType");
            }
        }
    }
}