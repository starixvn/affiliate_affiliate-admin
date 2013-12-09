// --------------------------------------------------------------------------
// <copyright file="ThumbnailCreator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.23">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Drawing;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ThumbnailCreator
    {
        /// <summary>
        /// Create banner thumbnail.
        /// </summary>
        /// <param name="bmp">The banner image.</param>
        /// <param name="filePath">The thumbnail file path.</param>
        public static void CreateThumbnailFromImage(Bitmap bmp, string filePath)
        {
            // Get thumbnail size.
            var size = GetThumbnailSize(bmp.Width, bmp.Height);
            var result = new Bitmap(size.Width, size.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(bmp, 0, 0, size.Width, size.Height);
            }

            result.Save(filePath);
        }

        /// <summary>
        /// Use the SWFToImage library to create thumbnail from flash file.
        /// From website BYTESCOUT.COM.
        /// </summary>
        /// <param name="swfInfo">The flash info object.</param>
        /// <param name="imageFilePath">The thumbnail file path.</param>
        public static void CreateThumbnailFromFlash(SwfInfo swfInfo, string imageFilePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get standard size of thumbnail file.
        /// </summary>
        /// <param name="width">The original width.</param>
        /// <param name="height">The original height.</param>
        /// <returns>The thumbnail size.</returns>
        private static Size GetThumbnailSize(int width, int height)
        {
            var size = new Size
            {
                Width = width,
                Height = height
            };

            var heightRatio = MediaSettings.ThumbnailHeight * 1.0f / height;
            var widthRatio = MediaSettings.ThumbnailWidth * 1.0f / width;

            var ratio = Math.Min(heightRatio, widthRatio);

            // Determine the thumbnail size by the standard thumbnail height.            
            if (ratio < 1)
            {
                size.Width = (int)(ratio * width);
                size.Height = (int)(ratio * height);
            }

            return size;
        }
    }
}
