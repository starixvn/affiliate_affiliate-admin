namespace ConfigGenerator.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum SystemType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Admin Type.
        /// </summary>
        Admin = 1 << 0,

        /// <summary>
        /// User Type.
        /// </summary>
        User = 1 << 1,

        /// <summary>
        /// Banner Type.
        /// </summary>
        Banner = 1 << 2,

        /// <summary>
        /// Link Type.
        /// </summary>
        Link = 1 << 3,

        /// <summary>
        /// Web Service Type.
        /// </summary>
        WebService = 1 << 4,
    }
}