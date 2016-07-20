namespace ConfigGenerator.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum ConfigurationType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Web Configuration Type.
        /// </summary>
        WebConfig = 1 << 0,

        /// <summary>
        /// Application Setting Type.
        /// </summary>
        AppSetting = 1 << 1,

        /// <summary>
        /// Connection String Type.
        /// </summary>
        ConnectionStrings = 1 << 2,

        /// <summary>
        /// SP Name Type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        SpName = 1 << 3
    }
}