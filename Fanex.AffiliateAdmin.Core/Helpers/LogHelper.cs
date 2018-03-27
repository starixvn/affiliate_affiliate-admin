// -----------------------------------------------------------------------
// <copyright file="LogHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Helpers
{
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class LogHelper
    {
        public static void WriteError(Exception exception)
        {
            var request = HttpContext.Current.Request;
            string message = exception.Message +
                             "\r\nSOURCE: " + exception.Source +
                             "\r\nDOMAIN: " + request.Headers["Host"] +
                             "\r\nFORM: " + string.Join("~", request.Form) +
                             "\r\nUrl: " + request.RawUrl +
                             "\r\nQUERYSTRING: " + string.Join("~", request.QueryString) +
                             "\r\nTARGETSITE: " + exception.TargetSite +
                             "\r\nSTACKTRACE: " + exception.StackTrace +
                             "\r\nBrowser:" + request.Browser.Type.ToString() +
                             "\r\nBrowserVer:" + request.Browser.Version.ToString() +
                             "\r\nAgent:" + request.UserAgent.ToString();
            var sqlException = exception as SqlException;
            if (sqlException != null)
            {
                message += "\r\nProcedure: " + sqlException.Procedure +
                    "\r\nLine Number: " + sqlException.LineNumber +
                    "\r\nServer: " + sqlException.Server;
            }

            var session = SessionManager.CurrentSession;
            if (session != null)
            {
                message += "\r\nSystemInfo:UserId=" + session.CustID + "-" + session.Username;
            }

            LogService.Error(message, exception);
        }
    }
}