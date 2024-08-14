namespace Fanex.AffiliateAdmin.Services.Helpers
{
    using Core.Services.Impl;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Data.Repository;

    public static class AffWriteLogHelper
    {
        private static IDbLogger _logger;

        private static IDbLogger DbLogger => _logger ?? (_logger = new DbLogger());

        public static void WriteLog(string typeName, string spName, string details, int affId = 0, string criteriaType = LogCriteriaType.AffWriteLog)
        {
            ICriteria criteria;
            if (criteriaType == LogCriteriaType.AffWriteLog)
            {
                criteria = BuildLogCriteria<AffWriteLogCriteria>(typeName, spName, details, affId);
            }
            else
            {
                criteria = BuildLogCriteria<AffWriteLogUserLogRunSpCriteria>(typeName, spName, details, affId);
            }
            DbLogger.AffWriteLog(criteria);
        }

        private static T BuildLogCriteria<T>(string typeName, string spName, string details, int affId) where T : new()
        {
            dynamic result = new T();

            result.SpName = spName;
            result.AffId = affId;
            result.Details = details;
            result.Creator = SessionManager.CurrentSession.CustID;
            result.Ip = Remote.GetClientIPAddress();

            if (typeof(T) == typeof(AffWriteLogCriteria))
            {
                result.TypeName = typeName;
            }

            return (T)result;
        }
    }
}
