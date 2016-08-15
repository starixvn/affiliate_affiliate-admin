namespace SsoSimulator.Controllers
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Mvc;
    using System.Web.Security;
    using Nexcel.Affiliate.Integration;

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
            if (password != "1234aa")
            {
                ViewBag.Message = "Wrong username or password!";
                return View();
            }

            var encryptedPassword = GetEncryptedPassword(username, "spondemo"); // "754A738BF99D986D8716FE00A81BC5CA32916BF6";
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                ViewBag.Message = "Username is not existed!";
                return View();
            }

            FormsAuthentication.SetAuthCookie(username, true);
            HttpContext.Session.Add("username", username);
            HttpContext.Session.Add("password", encryptedPassword);
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
            requestModel.EncryptedPassword = "754A738BF99D986D8716FE00A81BC5CA32916BF6";//HttpContext.Session["password"].ToString();
            requestModel.Timestamp = GetDbTimeStamp();
            requestModel.LanguageCode = "zh-CN";

            UrlFactory urlFactory = new UrlFactory();
            string ssoUrl = urlFactory.GenerateSsoLink(requestModel);
            Response.Redirect(ssoUrl);
        }

        public ActionResult Error()
        {
            return View();
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

        private static string GetEncryptedPassword(string userName, string licenseeName)
        {
            using (SqlConnection sqlConnection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["demo"].ToString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand(
                    "Select UserPwd from Users where Username = @Username and LicenseeName = @Licenseename", sqlConnection))
                {
                    sqlCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = userName;
                    sqlCommand.Parameters.Add("@LicenseeName", SqlDbType.VarChar).Value = licenseeName;
                    sqlConnection.Open();
                    var result = sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                    return result != null ? result.ToString() : string.Empty;
                }
            }
        }
    }
}