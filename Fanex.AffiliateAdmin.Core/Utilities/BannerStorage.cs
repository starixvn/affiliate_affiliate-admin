// --------------------------------------------------------------------------
// <copyright file="BannerStorage.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.25">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Resources;

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

        public string BannerFile { get; set; }

        public HttpPostedFileBase File { get; set; }

        public short BannerType { get; set; }

        public string BannerName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(BannerFile);
            }
        }

        public string BannerExtension
        {
            get { return Path.GetExtension(BannerFile); }
        }

        public string BannerSize
        {
            get { return _bannerSize; }
            set { _bannerSize = value; }
        }

        public string ThumbnailFile
        {
            get
            {
                return string.Format(ThumbNailFormat, BannerName, BannerExtension);
            }
        }

        public string MediaFolder
        {
            get
            {
                return MediaSettings.BannerDirectory;
            }
        }

        /// <value>Relative banner folder value.</value>
        /// <summary>
        /// Gets relative banner folder value.
        /// </summary>
        public string BannerFolder
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

        public string RelativeBannerPath
        {
            get
            {
                return Path.Combine(BannerFolder, BannerFile);
            }
        }

        /// <value>Full banner folder.</value>
        /// <summary>
        /// Gets full banner folder.
        /// </summary>
        public string BannerPath
        {
            get
            {
                return Path.Combine(MediaFolder, BannerFolder, BannerFile);
            }
        }

        public string ThumbnailPath
        {
            get
            {
                return Path.Combine(MediaFolder, BannerFolder, ThumbnailFile);
            }
        }

        public bool HasExistedBannerFile()
        {
            if (System.IO.File.Exists(BannerPath) || System.IO.File.Exists(ThumbnailPath))
            {
                return true;
            }

            return false;
        }

        public bool IsValidExtension()
        {
            var extension = BannerExtension.ToLower();
            return AllowedExtensions.Any(x => x == extension);
        }

        public bool HasExceedSizeLimit()
        {
            return File.ContentLength > MediaSettings.ImageSize;
        }

        /// <summary>
        /// Upload banner file.
        /// </summary>
        /// <returns>The uploading result.</returns>
        public string UploadBanner()
        {
            // Check the valid extension.
            if (!IsValidExtension())
            {
                return Lang.errFileExtension;
            }

            // Check the maximum file size.
            if (HasExceedSizeLimit())
            {
                return string.Format(Lang.MaxFileSizeLimit, MediaSettings.ImageSize);
            }

            // Check the banner file exist.
            if (HasExistedBannerFile())
            {
                return Lang.FileNameExisted;
            }

            // Create banner directory.
            var bannerDir = Path.Combine(MediaFolder, BannerFolder);

            if (Directory.Exists(bannerDir) == false)
            {
                Directory.CreateDirectory(bannerDir);
            }

            // Save the banner file.
            File.SaveAs(BannerPath);

            // Get banner size.
            // 1. is image file.
            // 2. is flash file.
            if (BannerType == 1)
            {
                var bitmap = new Bitmap(File.InputStream);
                _bannerSize = string.Format("{0}x{1}", bitmap.Width, bitmap.Height);
                ThumbnailCreator.CreateThumbnailFromImage(bitmap, ThumbnailPath);
            }
            else
            {
                var swfInfo = new SwfInfo(BannerPath);
                _bannerSize = string.Format("{0}x{1}", swfInfo.Width, swfInfo.Height);
            }

            return string.Empty;
        }

        /// <summary>
        /// Delete the banner file.
        /// </summary>
        public void DeleteBanner()
        {
            Common.DeleteFile(BannerPath);
            Common.DeleteFile(ThumbnailPath);
        }

        /// <summary>
        /// Move banner file to another position.
        /// </summary>
        /// <param name="oldBannerPath">The old banner path.</param>
        /// <param name="oldThumbnailPath">The old thumbnail path.</param>
        /// <returns>The moving result.</returns>
        public string MoveBannerFile(string oldBannerPath, string oldThumbnailPath)
        {
            if (!BannerPath.Equals(oldBannerPath))
            {
                // Banner Type is image.
                if (BannerType == 1)
                {
                    if (System.IO.File.Exists(oldBannerPath) && System.IO.File.Exists(oldThumbnailPath))
                    {
                        Common.MoveFile(oldBannerPath, BannerPath);
                        Common.MoveFile(oldThumbnailPath, ThumbnailPath);

                        return string.Empty;
                    }

                    return string.Format(Lang.CouldNotFindPath, oldBannerPath);
                }

                if (System.IO.File.Exists(oldBannerPath))
                {
                    Common.MoveFile(oldBannerPath, BannerPath);

                    return string.Empty;
                }

                return string.Format(Lang.CouldNotFindPath, oldBannerPath);
            }

            return string.Empty;
        }
    }
}
