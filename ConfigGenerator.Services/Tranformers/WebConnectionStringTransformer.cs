// --------------------------------------------------------------------------
// <copyright file="WebConnectionStringTransformer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Services.Tranformers
{
    using System.Collections.Generic;
    using ConfigGenerator.Core;
    using ConfigGenerator.Services.T4Templates.Admin;
    using ConfigGenerator.Services.T4Templates.User;
    using ConfigGenerator.Services.T4Templates.WebService;

    /// <summary>
    /// Web Connection String Transformer.
    /// </summary>
    public static class WebConnectionStringTransformer
    {
        /// <summary>
        /// Get Transform Result.
        /// </summary>
        /// <param name="configModel">Configuration Model.</param>
        /// <returns>Transform Result.</returns>
        public static string GetTransformResult(ConfigModel configModel)
        {
            switch (configModel.SystemName)
            {
                case "Admin":
                    return GetAdminTransformResult(configModel);
                case "User":
                    return GetUserTransformResult(configModel);
                case "WebService":
                    return GetWebServiceTransformResult(configModel);
            }

            return string.Empty;
        }

        private static string GetAdminTransformResult(ConfigModel configModel)
        {
            var admin = new WebConnectionStringAdmin { Session = new Dictionary<string, object>() };
            admin.Session["environment"] = configModel.EnvironmentName;
            admin.Session["model"] = configModel.GetModelFile();
            return admin.TransformText();
        }

        private static string GetUserTransformResult(ConfigModel configModel)
        {
            var user = new WebConnectionStringUser { Session = new Dictionary<string, object>() };
            user.Session["environment"] = configModel.EnvironmentName;
            user.Session["model"] = configModel.GetModelFile();
            return user.TransformText();
        }

        private static string GetWebServiceTransformResult(ConfigModel configModel)
        {
            var webservice = new WebConnectionStringWebService { Session = new Dictionary<string, object>() };
            webservice.Session["environment"] = configModel.EnvironmentName;
            webservice.Session["model"] = configModel.GetUserWebModelFile();
            return webservice.TransformText();
        }
    }
}