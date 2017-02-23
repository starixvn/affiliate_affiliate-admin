namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using System.IO;
    using System.Linq;

    public class MimeTypeHelper
    {
        private static readonly byte[] GIF = { 71, 73, 70, 56 };
        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] SWF = { 70, 87, 83 };

        public static bool IsValidMimeType(Stream stream)
        {
            var file = ReadBytes(stream);

            return file.Take(4).SequenceEqual(GIF) || file.Take(3).SequenceEqual(JPG) || file.Take(16).SequenceEqual(PNG) || file.Take(3).SequenceEqual(SWF);
        }

        private static byte[] ReadBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}