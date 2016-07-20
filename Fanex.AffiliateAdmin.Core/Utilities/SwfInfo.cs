namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// Summary description for SwfInfo
    /// http://blogs.msdn.com/b/mswanson/archive/2006/10/15/example-c-code-for-reading-flash-swf-files.aspx
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "3rd source code")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "3rd source code")]
    [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "*", Justification = "3rd source code")]
    public class SwfInfo
    {
        #region Properties

        private string _filename;

        /// <summary>
        /// SWF filename to analyze
        /// </summary>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        private string _magicBytes;

        /// <summary>
        /// Magic bytes in a SWF file (FWS or CWS)
        /// </summary>
        public string MagicBytes
        {
            get { return _magicBytes; }
        }

        private bool _isCompressed;

        /// <summary>
        /// Flag to indicate a compressed file (CWS)
        /// </summary>
        public bool IsCompressed
        {
            get { return _isCompressed; }
        }

        private short _version;

        /// <summary>
        /// Flash major version
        /// </summary>
        public short Version
        {
            get { return _version; }
        }

        private int _size;

        /// <summary>
        /// Uncompressed file size (in bytes)
        /// </summary>
        public int Size
        {
            get { return _size; }
        }

        private int _width;

        /// <summary>
        /// Flash movie native width
        /// </summary>
        public int Width
        {
            get { return _width; }
        }

        private int _height;

        /// <summary>
        /// Flash movie native height
        /// </summary>
        public int Height
        {
            get { return _height; }
        }

        private bool _isValid;

        /// <summary>
        /// Flag indicating whether SWF file is valid
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }

        private double _frameRate = 0;

        /// <summary>
        /// Flash movie native frame-rate
        /// </summary>
        public double FrameRate
        {
            get { return _frameRate; }
        }

        private int _frameCount = 0;

        /// <summary>
        /// Flash movie total frames
        /// </summary>
        public int FrameCount
        {
            get { return _frameCount; }
        }

        #endregion Properties

        public SwfInfo(string filename)
        {
            LoadSwf(filename);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes", Justification = "Accept this method")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public void LoadSwf(string filename)
        {
            _filename = filename;

            using (var reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                // Read MAGIC FIELD
                _magicBytes = new string(reader.ReadChars(3));

                if (_magicBytes != "FWS" && _magicBytes != "CWS")
                {
                    _isValid = false;
                    throw new Exception(filename + " is not a valid/supported SWF file.");
                }

                // Compression
                _isCompressed = _magicBytes.StartsWith("C", StringComparison.OrdinalIgnoreCase);

                // Version
                _version = Convert.ToInt16(reader.ReadByte());

                // Size
                _size = 0;

                // 4 LSB-MSB
                for (int i = 0; i < 4; i++)
                {
                    byte t = reader.ReadByte();
                    _size += t << (8 * i);
                }

                // RECT... we will "simulate" a stream from now on... read remaining file
                byte[] buffer = reader.ReadBytes(_size);

                // First decompress GZ stream
                if (_isCompressed)
                {
                    // Let's set GZip magic bytes which GZipStream can process
                    Array.Resize(ref buffer, buffer.Length + 8);
                    for (int i = buffer.Length - 1; i > 9; i--)
                    {
                        buffer[i] = buffer[i - 8];
                    }

                    ((Array)(new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0 })).CopyTo(buffer, 0);

                    var ms = new MemoryStream(buffer);

                    var gzip = new GZipStream(ms, CompressionMode.Decompress);

                    var decompressedBuffer = new byte[buffer.Length + 1000000];

                    var gzipLength = ReadAllBytesFromStream(gzip, decompressedBuffer);

                    gzip.Close();
                    ms.Close();

                    Array.Resize(ref buffer, gzipLength);
                    Array.Resize(ref decompressedBuffer, gzipLength);
                    decompressedBuffer.CopyTo(buffer, 0);

                    Array.Clear(decompressedBuffer, 0, decompressedBuffer.Length);
                }

                byte cbyte = buffer[0];
                int bits = cbyte >> 3;

                Array.Reverse(buffer);
                Array.Resize(ref buffer, buffer.Length - 1);
                Array.Reverse(buffer);

                var cval = new BitArray(bits, false);

                // Current byte
                cbyte &= 7;
                cbyte <<= 5;

                // Current bit (first byte starts off already shifted)
                var cbit = 2;

                // Must get all 4 values in the RECT
                for (var i = 0; i < 4; i++)
                {
                    for (int j = 0; j < cval.Count; j++)
                    {
                        if ((cbyte & 128) > 0)
                        {
                            cval[j] = true;
                        }

                        cbyte <<= 1;
                        cbyte &= 255;
                        cbit--;

                        // We will be needing a new byte if we run out of bits
                        if (cbit < 0)
                        {
                            cbyte = buffer[0];

                            Array.Reverse(buffer);
                            Array.Resize(ref buffer, buffer.Length - 1);
                            Array.Reverse(buffer);

                            cbit = 7;
                        }
                    }

                    // O.k. full value stored... calculate
                    int c = 1;
                    int val = 0;

                    for (int j = cval.Count - 1; j >= 0; j--)
                    {
                        if (cval[j])
                        {
                            val += c;
                        }

                        c *= 2;
                    }

                    val /= 20;

                    switch (i)
                    {
                        case 0:
                            // tmp value
                            _width = val;
                            break;

                        case 1:
                            _width = val - _width;
                            break;

                        case 2:
                            // tmp value
                            _height = val;
                            break;

                        case 3:
                            _height = val - _height;
                            break;
                    }

                    cval.SetAll(false);
                }

                // Frame rate
                _frameRate += buffer[1];
                _frameRate += Convert.ToSingle(buffer[0] / 100);

                // Frames
                _frameCount += BitConverter.ToInt16(buffer, 2);
            }
        }

        public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            // Use this method is used to read all bytes from a stream.
            var offset = 0;
            var totalCount = 0;
            while (true)
            {
                var bytesRead = stream.Read(buffer, offset, 100);
                if (bytesRead == 0)
                {
                    break;
                }

                offset += bytesRead;
                totalCount += bytesRead;
            }

            return totalCount;
        }
    }
}