namespace ConfigGenerator.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Flags]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum EnvironmentType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Local Type.
        /// </summary>
        Local = 1 << 0,

        /// <summary>
        /// Master Type.
        /// </summary>
        Master = 1 << 1,

        /// <summary>
        /// User Acceptance Test Type.
        /// </summary>
        Uat = 1 << 2,

        /// <summary>
        /// Production Type.
        /// </summary>
        Production = 1 << 3
    }
}