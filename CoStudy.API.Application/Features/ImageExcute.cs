using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;

namespace CoStudy.API.Application.Features
{
    public class ImageRequest
    {
        public int? PosiX { get; set; }
        public int? PosiY { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public bool? FlipHorizontal { get; set; }
        public bool? FlipVertical { get; set; }
        public int? Angle { get; set; }
        public string Url { get; set; }
    }
    public class ImageExcute
    {
        public Tuple<byte[], string> GetImageExtensionNullable(string url, int? posiX, int? posiY, int? width, int? height, bool? flipX, bool? flipY, float? rotate, IConfiguration configuration)
        {
            var webClient = new WebClient();
            byte[] b = webClient.DownloadData(url);
            var memoryStream = new MemoryStream(b);
            string imageType;
            var isImage = ImageValidator.IsImage(memoryStream, out imageType, configuration);
            if (!isImage)
                throw new Exception("Lỗi định dạng");
            var tempImage = System.Drawing.Image.FromStream(memoryStream);



            if (posiX.HasValue || posiY.HasValue || width.HasValue || height.HasValue)
            {
                if (!posiX.HasValue && !posiY.HasValue)
                {
                    var w1 = tempImage.Width;
                    var h1 = tempImage.Height;

                    if (width.HasValue)
                        w1 = width.Value;

                    if (height.HasValue)
                        h1 = height.Value;
                    tempImage = ImageExtension.ResizeImage(tempImage, w1, h1);
                }
                else
                {
                    var x = 0;
                    var y = 0;

                    if (posiX.HasValue)
                        x = posiX.Value;
                    if (posiY.HasValue)
                        y = posiY.Value;

                    var w = tempImage.Width;
                    var h = tempImage.Height;

                    if (width.HasValue)
                        w = width.Value;

                    if (height.HasValue)
                        h = height.Value;

                    System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, w, h);
                    tempImage = ImageExtension.CropImage(tempImage, rectangle);
                }

            }

            if (flipX.HasValue && flipX.Value == true)
                tempImage = ImageExtension.Flip180X(tempImage);
            if (flipY.HasValue && flipY.Value == true)
                tempImage = ImageExtension.Flip180Y(tempImage);

            if (rotate.HasValue)
            {
                tempImage = ImageExtension.RotateImage(tempImage, rotate.Value);
            }

            var newStream = new MemoryStream();

            tempImage.Save(newStream, ImageExtension.ParseImageFormat(imageType.Replace(".", String.Empty).ToLower()));

            var fileType = $"image/{imageType.Replace(".", String.Empty).ToLower()}";

            return new Tuple<byte[], string>(newStream.ToArray(), fileType);
        }

    }
}
