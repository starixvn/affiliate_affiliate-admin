namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using Core.Configurations;
    using Core.Utilities;
    using DTO;
    using Models;
    using Resources;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class BannerHelper
    {
        public static BannerStorage UploadFile(BannerAddViewModel model, out string errorMessage)
        {
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFileName = model.FileName,
                File = model.File,
                BannerType = model.ImageType
            };

            errorMessage = bannerStorage.UploadBanner();

            return bannerStorage;
        }

        public static BannerStorage UpdateFile(BannerEditViewModel model, BannerDetail bannerDetail, out string errorMessage)
        {
            errorMessage = string.Empty;
            var bannerStorage = new BannerStorage
            {
                AffType = model.AffType,
                Category = model.Category,
                SubCategory = model.SubCategory,
                BannerFileName = model.FileName,
                File = model.File,
                BannerType = model.ImageType
            };

            if (bannerStorage.File == null)
            {
                // Check the valid extension.
                if (!bannerStorage.IsValidExtension())
                {
                    errorMessage = Lang.errFileExtension;
                }
                else
                {
                    var mediaType = MediaSettings.BannerSizes.FirstOrDefault(t => t.TypeId == bannerDetail.Size);
                    if (mediaType != null)
                    {
                        bannerStorage.BannerSize = mediaType.Value;
                    }

                    if (bannerDetail.ImageType.HasValue)
                    {
                        errorMessage = bannerStorage.MoveBannerFile(bannerDetail.Content);
                    }
                }
            }
            else
            {
                errorMessage = bannerStorage.UploadBanner();
                if (string.IsNullOrEmpty(errorMessage))
                {
                    // Delete the old banner file.
                    DeleteFile(bannerDetail.Content);
                }
            }

            return bannerStorage;
        }

        public static void DeleteFile(string content)
        {
            IEnumerable<string> bannerPaths, thumbnailPaths;
            GetPaths(content, out bannerPaths, out thumbnailPaths);

            Common.DeleteFiles(bannerPaths);
            Common.DeleteFiles(thumbnailPaths);
        }

        private static void GetPaths(
            string content,
            out IEnumerable<string> bannerPaths,
            out IEnumerable<string> thumbnailPaths)
        {
            var oldContent = content.Replace('/', '\\');

            var oldBannerFile = Path.GetFileName(oldContent);
            var oldThumbnailFile = BannerStorage.GetThumbnailFileName(oldContent);

            bannerPaths = MediaSettings.BannerFolders.Select(x => Path.Combine(x, oldContent));
            thumbnailPaths = bannerPaths.Select(x => x.Replace(oldBannerFile, oldThumbnailFile));
        }
    }
}