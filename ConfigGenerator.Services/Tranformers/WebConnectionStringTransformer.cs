using System.Collections.Generic;
using ConfigGenerator.Core;
using ConfigGenerator.Services.T4Templates.Admin;
using ConfigGenerator.Services.T4Templates.Banner;
using ConfigGenerator.Services.T4Templates.Link;
using ConfigGenerator.Services.T4Templates.User;
using ConfigGenerator.Services.T4Templates.WebService;

namespace ConfigGenerator.Services.Tranformers
{
    public static class WebConnectionStringTransformer
    {
        public static string GetTransformResult(ConfigModel configModel)
        {
            switch (configModel.SystemName)
            {
                case "Admin":
                    var admin = new WebConnectionStringAdmin { Session = new Dictionary<string, object>() };

                    admin.Session["environment"] = configModel.EnvironmentName;

                    admin.Session["model"] = configModel.GetModelFile();

                    return admin.TransformText();

                case "User":
                    var user = new WebConnectionStringUser { Session = new Dictionary<string, object>() };

                    user.Session["environment"] = configModel.EnvironmentName;

                    user.Session["model"] = configModel.GetModelFile();

                    return user.TransformText();

                case "WebService":

                    var webservice = new WebConnectionStringWebService { Session = new Dictionary<string, object>() };

                    webservice.Session["environment"] = configModel.EnvironmentName;

                    webservice.Session["model"] = configModel.GetUserWebModelFile();

                    return webservice.TransformText();
            }

            return string.Empty;
        }
    }
}