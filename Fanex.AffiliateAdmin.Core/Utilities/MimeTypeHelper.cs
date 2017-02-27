namespace Fanex.AffiliateAdmin.Controller.Helpers
{
    using System.IO;
    using System.Linq;

    public class MimeTypeHelper
    {
        private static readonly byte[][] HeaderBytes = new byte[][]
        {
            new byte[] { 71, 73, 70, 56 }, // GIF
            new byte[] { 255, 216, 255 }, // JPG
            new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 }, // PNG
            new byte[] { 67, 87, 83 }, // SWF
            new byte[] { 70, 87, 83 }  // SWF
        };

        public static bool IsValidMimeType(Stream stream)
        {
            var file = ReadBytes(stream);
            foreach (var bytes in HeaderBytes)
            {
                if (file.Take(bytes.Length).SequenceEqual(bytes))
                {
                    return true;
                }
            }

            return false;
        }

        private static byte[] ReadBytes(Stream input)
        {
            byte[] buffer = new byte[16];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                if ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}