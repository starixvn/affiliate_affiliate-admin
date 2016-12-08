namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    using System;

    [Flags]
    public enum AppMode : byte
    {
        Self = 0x0,
        Parasitic = 0x1
    }
}