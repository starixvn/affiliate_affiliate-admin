namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Resources;
    using Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerStorage
    {
        /// <summary>
        /// Format of ThumbNail file.
        /// {0}: Original file name.
        /// {1}: File extension.
        /// </summary>
        public const string ThumbNailFormat = "{0}_thumbnail{1}";

        private static readonly string[] AllowedExtensions = new string[] { ".gif", ".jpg", ".png", ".jpeg", ".swf" };

        private readonly IMediaSettingService _mediaSettingService = new MediaSettingService();

        private string _bannerFolder;

        private string _bannerSize;

        public short AffType { get; set; }

        public short Category { get; set; }

        public short? SubCategory { get; set; }

        public string BannerFileName { get; set; }

        public HttpPostedFileBase File { get; set; }

        public short BannerType { get; set; }

        public string BannerSize
        {
            get { return _bannerSize; }
            set { _bannerSize = value; }
        }

        public string RelativeBannerPath
        {
            get
            {
                return Path.Combine(BannerFolder, BannerFileName);
            }
        }

        private string ThumbnailFileName
        {
            get
            {
                return GetThumbnailFileName(BannerFileName);
            }
        }

        /// <value>Relative banner folder value.</value>
        /// <summary>
        /// Gets relative banner folder value.
        /// </summary>
        private string BannerFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_bannerFolder))
                {
                    // get category name
                    string subCategoryName, affTypeName;
                    string categoryName = subCategoryName = affTypeName = string.Empty;

                    // get sub category name.
                    var categoryEntity = MediaSettings.BannerCategories.FirstOrDefault(x => x.TypeId == Category);
                    if (categoryEntity != null)
                    {
                        categoryName = categoryEntity.Value;
                    }

                    // get sub category name.
                    var subCategories = _mediaSettingService.GetBannerSubCategories(Category);
                    var subCategoryEntity =
                        subCategories.FirstOrDefault(x => SubCategory != null && x.TypeId == SubCategory.Value);

                    if (subCategoryEntity != null)
                    {
                        subCategoryName = SubCategory != null ? subCategoryEntity.Value : string.Empty;
                    }

                    // get affiliate type name.
                    var firstAffType = AppSettings.AffTypes.FirstOrDefault(x => x.TypeID == AffType);

                    if (firstAffType != null)
                    {
                        affTypeName = firstAffType.Name;
                    }

                    _bannerFolder = Path.Combine(affTypeName, categoryName, subCategoryName);
                }

                return _bannerFolder;
            }
        }

        /// <value>Full banner folder.</value>
        /// <summary>
        /// Gets full banner folder.
        /// </summary>
        private IEnumerable<string> BannerFullPaths
        {
            get
            {
                return MediaSettings.BannerFolders.Select(x => Path.Combine(x, BannerFolder, BannerFileName));
            }
        }

        private IEnumerable<string> ThumbnailFullPaths
        {
            get
            {
                return MediaSettings.BannerFolders.Select(x => Path.Combine(x, BannerFolder, ThumbnailFileName));
            }
        }

        public static string GetThumbnailFileName(string fileName)
        {
            return string.Format(
                ThumbNailFormat,
                Path.GetFileNameWithoutExtension(fileName),
                Path.GetExtension(fileName));
        }

        private bool HasExistedBannerFile()
        {
            return Common.AnyFileExists(BannerFullPaths) || Common.AnyFileExists(ThumbnailFullPaths);
        }

        public bool IsValidExtension()
        {
            var extension = Path.GetExtension(BannerFileName).ToLower();
            return AllowedExtensions.Any(x => x == extension);
        }

        private bool HasExceedSizeLimit()
        {
            return File.ContentLength > MediaSettings.MaxImageSize;
        }

        public string UploadBanner()
        {
            if (!IsValidExtension())
            {
                return Lang.errFileExtension;
            }

            if (HasExceedSizeLimit())
            {
                return string.Format(Lang.MaxFileSizeLimit, MediaSettings.MaxImageSize);
            }

            if (HasExistedBannerFile())
            {
                return Lang.FileNameExisted;
            }

            foreach (var folder in MediaSettings.BannerFolders)
            {
                var bannerFile = Path.Combine(folder, BannerFolder, BannerFileName);

                var bannerDir = Path.Combine(folder, BannerFolder);

                if (Directory.Exists(bannerDir) == false)
                {
                    Directory.CreateDirectory(bannerDir);
                }

                if (UploadFile(folder, bannerFile) == false)
                {
                    return Lang.ErrorRequest;
                }
            }

            return string.Empty;
        }

        private bool UploadFile(string folder, string bannerFile)
        {
            var thumbnailPath = string.Empty;
            try
            {
                // Save the banner file.
                File.SaveAs(bannerFile);

                // Get banner size.
                // 1. is image file.
                // 2. is flash file.
                if (BannerType == 1)
                {
                    var bitmap = new Bitmap(File.InputStream);
                    _bannerSize = string.Format("{0}x{1}", bitmap.Width, bitmap.Height);
                    thumbnailPath = Path.Combine(folder, BannerFolder, ThumbnailFileName);
                    ThumbnailCreator.CreateThumbnailFromImage(bitmap, thumbnailPath);
                }
                else
                {
                    var swfInfo = new SwfInfo(bannerFile);
                    _bannerSize = string.Format("{0}x{1}", swfInfo.Width, swfInfo.Height);
                }

                return true;
            }
            catch (Exception ex)
            {
                Common.DeleteFile(bannerFile);
                if (string.IsNullOrEmpty(thumbnailPath) == false)
                {
                    Common.DeleteFile(thumbnailPath);
                }

                LogHelper.WriteError(ex);
            }

            return false;
        }

        /// <summary>
        /// Delete the banner file.
        /// </summary>
        public void DeleteBanners()
        {
            Common.DeleteFiles(BannerFullPaths);
            Common.DeleteFiles(ThumbnailFullPaths);
        }

        /// <summary>
        /// Move banner file to another position.
        /// </summary>
        /// <param name="oldBannerPath">The old banner path.</param>
        /// <param name="oldThumbnailPath">The old thumbnail path.</param>
        /// <returns>The moving result.</returns>
        public string MoveBannerFile(string oldRelativeBannerPath)
        {
            oldRelativeBannerPath = oldRelativeBannerPath.Replace('/', '\\');
            var newRelativeBannerPath = Path.Combine(BannerFolder, BannerFileName);
            if (!newRelativeBannerPath.Equals(oldRelativeBannerPath, System.StringComparison.OrdinalIgnoreCase))
            {
                // Banner Type is image.
                if (BannerType == 1)
                {
                    MoveThumbnailFiles(oldRelativeBannerPath);
                }

                var isFileExist = false;
                foreach (var folder in MediaSettings.BannerFolders)
                {
                    var oldFullPath = Path.Combine(folder, oldRelativeBannerPath);
                    var newFullPath = Path.Combine(folder, BannerFolder, BannerFileName);

                    if (System.IO.File.Exists(oldFullPath))
                    {
                        isFileExist = true;
                        Common.MoveFile(oldFullPath, newFullPath);
                    }
                }

                if (isFileExist == false)
                {
                    return string.Format(Lang.CouldNotFindPath, oldRelativeBannerPath.Replace("\\", "/"));
                }
            }

            return string.Empty;
        }

        private void MoveThumbnailFiles(string oldRelativeBannerPath)
        {
            var oldThumbnailFileName = GetThumbnailFileName(oldRelativeBannerPath);
            var oldBannerFileName = Path.GetFileName(oldRelativeBannerPath);
            var oldThumbnailRelativePath = oldRelativeBannerPath.Replace(oldBannerFileName, oldThumbnailFileName);
            var newThumbnailRelativePath = Path.Combine(BannerFolder, GetThumbnailFileName(BannerFileName));

            foreach (var folder in MediaSettings.BannerFolders)
            {
                var oldThumbnailFullPath = Path.Combine(folder, oldThumbnailRelativePath);
                var newThumbnailFullPath = Path.Combine(folder, newThumbnailRelativePath);
                if (System.IO.File.Exists(oldThumbnailFullPath))
                {
                    Common.MoveFile(oldThumbnailFullPath, newThumbnailFullPath);
                }
            }
        }
    }
}