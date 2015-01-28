using System;

namespace ConfigGenerator.Core
{
    [Flags]
    public enum SystemType
    {
        None = 0,
        Admin = 1 << 0,
        User = 1 << 1,
        Banner = 1 << 2,
        Link = 1 << 3,
        WebService = 1 << 4,
    }
}