namespace Fanex.AffiliateAdmin.Criterias
{
    public enum DatabaseAction : int
    {
        /// <summary>
        /// None (Default value).
        /// </summary>
        None,

        /// <summary>
        /// Select Data.
        /// </summary>
        Select = 1,

        /// <summary>
        /// Insert Data.
        /// </summary>
        Insert = 2,
        
        /// <summary>
        /// Update Data.
        /// </summary>
        Update = 3,

        /// <summary>
        /// Delete Data.
        /// </summary>
        Delete = 4,

        /// <summary>
        /// Select Detail.
        /// </summary>
        Detail = 5,
    }
}