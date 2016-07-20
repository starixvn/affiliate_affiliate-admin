namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public interface IMediaSettingService
    {
        IEnumerable<MediaType> GetBannerCategories();

        IEnumerable<MediaType> GetBannerTypes();

        IEnumerable<MediaType> GetBannerSizes();

        IEnumerable<MediaType> GetBannerSubCategories(short category);
    }
}