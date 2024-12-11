namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using Fanex.AffiliateAdmin.Core.Configurations;
    using System;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ThumbnailCreator
    {
        /// <summary>
        /// Create banner thumbnail.
        /// </summary>
        /// <param name="image">The banner image.</param>
        /// <param name="filePath">The thumbnail file path.</param>
        public static void CreateThumbnailFromImage(Image image, string filePath)
        {
            // Get thumbnail size.
            var size = GetThumbnailSize(image.Width, image.Height);
            var result = new Bitmap(size.Width, size.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(image, 0, 0, size.Width, size.Height);
            }

            result.Save(filePath);
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
