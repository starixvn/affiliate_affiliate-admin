using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Nexcel.Affiliate.Integration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Nexcel.Affiliate.Integration.Configurations;

namespace SsoSimulator.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            FormsAuthentication.SetAuthCookie(username, true);
            HttpContext.Session.Add("username", username);
            HttpContext.Session.Add("password", password);

            return Redirect("Index");
        }

        [Authorize]
        public void Logout()
        {            
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            HttpContext.Response.Cookies.Clear();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        [Authorize]
        public void RedirectSSO()
        {
            var requestModel = new RequestSsoModel();
            requestModel.Username = HttpContext.User.Identity.Name;

            requestModel.EncryptedPassword = "754A738BF99D986D8716FE00A81BC5CA32916BF6";
            requestModel.Timestamp = GetDbTimeStamp();
            requestModel.ActiveDomain = IntegrationSettings.Settings.SsoUrl; // GetActiveDomain();
            requestModel.LanguageCode = "zh-CN";

            UrlFactory urlFactory = new UrlFactory();
            string ssoUrl = urlFactory.GenerateSsoLink(requestModel);
            Response.Redirect(ssoUrl);
        }

        private static long GetDbTimeStamp()
        {
            using (SqlConnection sqlConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["demo"].ToString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Lrf_GetDBCurrentDatetime", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    DateTime result = (DateTime)sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                    return result.Ticks;
                }
            }
        }

        private static string GetActiveDomain()
        {
            using (SqlConnection sqlConnection =
           new SqlConnection(ConfigurationManager.ConnectionStrings["demo"].ToString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Lrf_GetActiveDomain", sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    string result = (string)sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                    return result;
                }
            }
        }
    }
}
