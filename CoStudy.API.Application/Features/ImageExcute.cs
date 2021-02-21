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
            WebClient webClient = new WebClient();
            byte[] b = webClient.DownloadData(url);
            MemoryStream memoryStream = new MemoryStream(b);
            string imageType;
            bool isImage = ImageValidator.IsImage(memoryStream, out imageType, configuration);
            if (!isImage)
                throw new Exception("Lỗi định dạng");
            System.Drawing.Image tempImage = System.Drawing.Image.FromStream(memoryStream);



            if (posiX.HasValue || posiY.HasValue || width.HasValue || height.HasValue)
            {
                if (!posiX.HasValue && !posiY.HasValue)
                {
                    int w1 = tempImage.Width;
                    int h1 = tempImage.Height;

                    if (width.HasValue)
                        w1 = width.Value;

                    if (height.HasValue)
                        h1 = height.Value;
                    tempImage = ImageExtension.ResizeImage(tempImage, w1, h1);
                }
                else
                {
                    int x = 0;
                    int y = 0;

                    if (posiX.HasValue)
                        x = posiX.Value;
                    if (posiY.HasValue)
                        y = posiY.Value;

                    int w = tempImage.Width;
                    int h = tempImage.Height;

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

            MemoryStream newStream = new MemoryStream();

            tempImage.Save(newStream, ImageExtension.ParseImageFormat(imageType.Replace(".", String.Empty).ToLower()));

            string fileType = $"image/{imageType.Replace(".", String.Empty).ToLower()}";

            return new Tuple<byte[], string>(newStream.ToArray(), fileType);
        }

    }
}
