using System;

namespace ConfigGenerator.Core
{
    [Flags]
    public enum ConfigurationType
    {
        None = 0,
        WebConfig = 1 << 0,
        AppSetting = 1 << 1,
        ConnectionStrings = 1 << 2,
        SpName = 1 << 3
    }
}