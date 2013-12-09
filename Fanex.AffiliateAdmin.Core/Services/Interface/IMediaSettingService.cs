// --------------------------------------------------------------------------
// <copyright file="IMediaSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMediaSettingService
    {
        IEnumerable<MediaType> GetBannerCategories();

        IEnumerable<MediaType> GetBannerTypes();

        IEnumerable<MediaType> GetBannerSizes();

        IEnumerable<MediaType> GetBannerSubCategories(short category);
    }
}