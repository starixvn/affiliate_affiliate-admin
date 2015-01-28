using System.Collections.Generic;
using ConfigGenerator.Core;
using ConfigGenerator.Services.T4Templates.Admin;
using ConfigGenerator.Services.T4Templates.Banner;
using ConfigGenerator.Services.T4Templates.Link;
using ConfigGenerator.Services.T4Templates.User;
using ConfigGenerator.Services.T4Templates.WebService;

namespace ConfigGenerator.Services.Tranformers
{
    public static class WebConfigTransformer
    {
        public static string GetTransformResult(ConfigModel configModel)
        {
            switch (configModel.SystemName)
            {
                case "Admin":
                    var admin = new WebConfigAdmin { Session = new Dictionary<string, object>() };

                    admin.Session["environment"] = configModel.EnvironmentName;

                    admin.Session["model"] = configModel.GetModelFile();

                    return admin.TransformText();
                case "User":

                    var user  = new WebConfigUser { Session = new Dictionary<string, object>() };

                    user.Session["environment"] = configModel.EnvironmentName;

                    user.Session["model"] = configModel.GetModelFile();;

                    return user.TransformText();

                case "Banner":
                    var banner  = new WebConfigBanner { Session = new Dictionary<string, object>() };

                    banner.Session["environment"] = configModel.EnvironmentName;

                    banner.Session["model"] = configModel.GetUserWebModelFile();;

                    banner.Session["connectionstringmodel"] = configModel.GetUserConnectionStringModelFile();

                    return banner.TransformText();

                case "Link":
                    var link = new WebConfigLink { Session = new Dictionary<string, object>() };

                    link.Session["environment"] = configModel.EnvironmentName;

                    link.Session["model"] = configModel.GetUserWebModelFile();

                    link.Session["connectionstringmodel"] = configModel.GetUserConnectionStringModelFile();

                    return link.TransformText();

                case "WebService":
                    var webservice = new WebConfigWebService { Session = new Dictionary<string, object>() };

                    webservice.Session["environment"] = configModel.EnvironmentName;

                    webservice.Session["model"] = configModel.GetUserWebModelFile();

                    return webservice.TransformText();
            }

            return string.Empty;
        }
    }
}