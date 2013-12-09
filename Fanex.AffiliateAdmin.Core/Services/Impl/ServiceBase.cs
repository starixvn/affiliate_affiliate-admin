// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.03">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Services
{
    using System;
    using Fanex.Logging;

    /// <summary>
    /// Service Base class.
    /// </summary>
    public class ServiceBase
    {
        protected void HandleException(Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }
    }
}