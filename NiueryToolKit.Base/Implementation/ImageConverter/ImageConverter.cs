using NiueryToolKit.Base.Interface.ImageConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiueryToolKit.Base.Implementation.ImageConverter
{
    public class ImageConverter : IImageConverter
    {

        public Size IconSize { get; set; } = new Size(256, 256);

        public static readonly string[] SupportedFormats = { "PNG", "JPG", "JPEG", "BMP", "ICO" };

        public void ConvertImage(string sourcePath, string targetPath, string targetFormat)
        {
            if (!File.Exists(sourcePath))
                throw new FileNotFoundException("Source file not found", sourcePath);

            targetFormat = targetFormat.Trim().ToUpper();
            if (!Array.Exists(SupportedFormats, f => f == targetFormat))
                throw new ArgumentException($"The target format {targetFormat} is not supported. Supported formats:{string.Join(",", SupportedFormats)}");

            var targetImageFormat = GetImageFormat(targetFormat);

            using (var sourceStream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
            using (var sourceImage = Image.FromStream(sourceStream))
            {
                if (targetFormat == "ICO")
                {
                    SaveAsIco(sourceImage, targetPath);
                }
                else
                {
                    if (targetFormat is "JPG" or "JPEG")
                    {
                        SaveJpegWithQuality(sourceImage, targetPath, 95);
                    }
                    else
                    {
                        sourceImage.Save(targetPath, targetImageFormat);
                    }
                }
            }
        }

        private void SaveAsIco(Image image, string targetPath)
        {
            //Size size = new Size(image.Width,image.Height);
            Size size = IconSize;
            using (var icoBitmap = new Bitmap(image, size))
            {
                using (var icon = Icon.FromHandle(icoBitmap.GetHicon()))
                using (var fs = new FileStream(targetPath, FileMode.Create))
                {
                    icon.Save(fs);
                }
            }
        }

        private void SaveJpegWithQuality(Image image, string path, long quality)
        {
            quality = Math.Clamp(quality, 0, 100);

            var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            image.Save(path, jpgEncoder, encoderParams);
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            throw new ArgumentException($"Encoder for format {format} not found");
        }

        private ImageFormat GetImageFormat(string format)
        {
            return format switch
            {
                "PNG" => ImageFormat.Png,
                "JPG" or "JPEG" => ImageFormat.Jpeg,
                "BMP" => ImageFormat.Bmp,
                "ICO" => ImageFormat.Icon,
                _ => throw new ArgumentException($"Unsupported format:{format}")
            };
        }
    }
}
