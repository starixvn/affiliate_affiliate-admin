using System;
using System.Collections.Generic;
using ConfigGenerator.Core;

namespace ConfigGenerator.Services.SystemGenerators
{
    public class SystemGeneratorFactory
    {
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