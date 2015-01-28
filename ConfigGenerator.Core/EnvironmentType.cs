using System;

namespace ConfigGenerator.Core
{
    [Flags]
    public enum EnvironmentType
    {
        None = 0,
        Local = 1 << 0,
        Master = 1 << 1,
        Uat = 1 << 2,
        Production = 1 << 3
    }
}