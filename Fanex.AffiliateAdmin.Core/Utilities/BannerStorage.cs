namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Controller.Helpers;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Resources;
    using Helpers;

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

        public short AffType { get; set; }

        public short Category { get; set; }

        public short? SubCategory { get; set; }

        public string BannerFileName { get; set; }

        public HttpPostedFileBase File { get; set; }

        public short BannerType { get; set; }

        public string BannerSize { get; set; }

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
                    string subCategoryName, affTypeName;
                    string categoryName = subCategoryName = affTypeName = string.Empty;

                    var categoryEntity = MediaSettings.BannerCategories.FirstOrDefault(x => x.TypeId == Category);
                    if (categoryEntity != null)
                    {
                        categoryName = categoryEntity.Value;
                    }

                    var subCategories = _mediaSettingService.GetBannerSubCategories(Category);
                    var subCategoryEntity =
                        subCategories.FirstOrDefault(x => SubCategory != null && x.TypeId == SubCategory.Value);

                    if (subCategoryEntity != null)
                    {
                        subCategoryName = SubCategory != null ? subCategoryEntity.Value : string.Empty;
                    }

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

        public string UploadBanner()
        {
            if (!IsValidExtension() || !MimeTypeHelper.IsValidMimeType(File.InputStream))
            {
                return Lang.errFileExtension;
            }

            if (File.ContentLength > MediaSettings.MaxImageSize)
            {
                return string.Format(Lang.MaxFileSizeLimit, MediaSettings.MaxImageSize);
            }

            if (HasExistedBannerFile())
            {
                return Lang.FileNameExisted;
            }

            if (!EnsureFoldersExisted(BannerFolder))
            {
                return Lang.AccessDenied;
            }

            foreach (var folder in MediaSettings.BannerFolders)
            {
                var bannerFile = Path.Combine(folder, BannerFolder, BannerFileName);

                if (UploadFile(folder, bannerFile) == false)
                {
                    return Lang.ErrorRequest;
                }
            }

            return string.Empty;
        }

        private bool EnsureFoldersExisted(string bannerFolder)
        {
            foreach (var folder in MediaSettings.BannerFolders)
            {
                var bannerDir = Path.Combine(folder, bannerFolder);

                LogHelper.WriteInfo($"BannerDir: {bannerDir}");

                if (Directory.Exists(bannerDir) == false)
                {
                    try
                    {
                        Directory.CreateDirectory(bannerDir);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteInfo($"BannerDir failed: {bannerDir}");
                        LogHelper.WriteInfo(ex.Message);
                        return false;
                    }
                }
            }

            return true;
        }

        private bool UploadFile(string folder, string bannerFile)
        {
            var thumbnailPath = string.Empty;
            try
            {
                File.SaveAs(bannerFile);

                // Get banner size.
                // 1. is image file.
                // 2. is flash file.
                if (BannerType == 1)
                {
                    var bitmap = new Bitmap(File.InputStream);
                    BannerSize = string.Format("{0}x{1}", bitmap.Width, bitmap.Height);
                    thumbnailPath = Path.Combine(folder, BannerFolder, ThumbnailFileName);
                    ThumbnailCreator.CreateThumbnailFromImage(bitmap, thumbnailPath);
                }
                else
                {
                    var swfInfo = new SwfInfo(bannerFile);
                    BannerSize = string.Format("{0}x{1}", swfInfo.Width, swfInfo.Height);
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

        public void DeleteBanners()
        {
            Common.DeleteFiles(BannerFullPaths);
            Common.DeleteFiles(ThumbnailFullPaths);
        }

        public string MoveBannerFile(string oldRelativeBannerPath)
        {
            oldRelativeBannerPath = oldRelativeBannerPath.Replace('/', '\\');
            var newRelativeBannerPath = Path.Combine(BannerFolder, BannerFileName);
            if (!newRelativeBannerPath.Equals(oldRelativeBannerPath, System.StringComparison.OrdinalIgnoreCase))
            {
                if (!EnsureFoldersExisted(BannerFolder))
                {
                    return Lang.AccessDenied;
                }

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