// --------------------------------------------------------------------------
// <copyright file="SpNameTransformer.cs" company="Nexcel Solutions Vietnam">
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
    using ConfigGenerator.Services.T4Templates.Banner;
    using ConfigGenerator.Services.T4Templates.Link;
    using ConfigGenerator.Services.T4Templates.User;
    using ConfigGenerator.Services.T4Templates.WebService;

    /// <summary>
    /// SP Name Transformer.
    /// </summary>
    public static class SPNameTransformer
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

                case "Banner":

                    var banner = new SpNameBanner { Session = new Dictionary<string, object>() };

                    banner.Session["environment"] = configModel.EnvironmentName;

                    banner.Session["model"] = configModel.GetUserConnectionStringModelFile();

                    return banner.TransformText();

                case "Link":

                    var link = new SpNameLink { Session = new Dictionary<string, object>() };

                    link.Session["environment"] = configModel.EnvironmentName;

                    link.Session["model"] = configModel.GetUserConnectionStringModelFile();

                    return link.TransformText();

                case "WebService":

                    var webservice = new SpNameWebService { Session = new Dictionary<string, object>() };

                    webservice.Session["environment"] = configModel.EnvironmentName;

                    webservice.Session["model"] = configModel.GetUserConnectionStringModelFile();

                    return webservice.TransformText();
            }

            return string.Empty;
        }

        private static string GetAdminTransformResult(ConfigModel configModel)
        {
            var admin = new SpNameAdmin { Session = new Dictionary<string, object>() };
            admin.Session["environment"] = configModel.EnvironmentName;
            admin.Session["model"] = configModel.GetModelFile();
            return admin.TransformText();
        }

        private static string GetUserTransformResult(ConfigModel configModel)
        {
            var user = new SpNameUser { Session = new Dictionary<string, object>() };
            user.Session["environment"] = configModel.EnvironmentName;
            user.Session["model"] = configModel.GetModelFile();
            return user.TransformText();
        }
    }
}