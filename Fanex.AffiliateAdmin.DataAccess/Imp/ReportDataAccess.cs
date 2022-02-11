namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Data;
    using Common.Extensions;
    using Core.Utilities;
    using Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class ReportDataAccess
    {
        protected const string AffColSuffix = "_Aff";

        protected const string SysColSuffix = "Base";

        #region Common helpers

        /// <summary>
        /// Get output parameters from database.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <param name="objectDB">Object database.</param>
        protected void GetOutputParams(ICriteria criteria, ObjectDb objectDB)
        {
            var outputParams = objectDB.GetOutputParams();

            if (outputParams != null)
            {
                var properties = criteria.GetType().GetProperties();

                foreach (string outputParam in outputParams.Keys)
                {
                    foreach (var property in properties)
                    {
                        if (property.CanWrite && property.Name.ToLower() == outputParam)
                        {
                            property.SetValue(criteria, outputParams[outputParam], null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set Property Value.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="value">Value Input.</param>
        /// <param name="propertyName">Property Name.</param>
        protected void SetPropertyValue(IDataReader reader, object value, string propertyName)
        {
            value.SetPropertyValue(propertyName, reader.ToDecimal(propertyName));
        }

        #endregion Common helpers
    }
}