using System.Collections.Generic;
using ConfigGenerator.Core;
using ConfigGenerator.Services.T4Templates.Admin;
using ConfigGenerator.Services.T4Templates.User;

namespace ConfigGenerator.Services.Tranformers
{
    public static class WebAppSettingTransformer
    {
        public static string GetTransformResult(ConfigModel configModel, string modelFile)
        {
            switch (configModel.SystemName)
            {
                case "Admin":
                    var admin = new WebAppSettingAdmin { Session = new Dictionary<string, object>() };

                    admin.Session["environment"] = configModel.EnvironmentName;

                    admin.Session["model"] = modelFile;

                    return admin.TransformText();
                case "User":

                    var user = new WebAppSettingUser { Session = new Dictionary<string, object>() };

                    user.Session["environment"] = configModel.EnvironmentName;

                    user.Session["model"] = modelFile;

                    return user.TransformText();
            }

            return string.Empty;
        }
    }
}