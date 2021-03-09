using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;

namespace CoStudy.API.Application.Features
{
    /// <summary>
    /// Image
    /// </summary>
    public class ImageRequest
    {
        /// <summary>
        /// Gets or sets the posi x.
        /// </summary>
        /// <value>
        /// The posi x.
        /// </value>
        public int? PosiX { get; set; }
        /// <summary>
        /// Gets or sets the posi y.
        /// </summary>
        /// <value>
        /// The posi y.
        /// </value>
        public int? PosiY { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int? Width { get; set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the flip horizontal.
        /// </summary>
        /// <value>
        /// The flip horizontal.
        /// </value>
        public bool? FlipHorizontal { get; set; }
        /// <summary>
        /// Gets or sets the flip vertical.
        /// </summary>
        /// <value>
        /// The flip vertical.
        /// </value>
        public bool? FlipVertical { get; set; }
        /// <summary>
        /// Gets or sets the angle.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public int? Angle { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ImageExcute
    {
        /// <summary>
        /// Gets the image extension nullable.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="posiX">The posi x.</param>
        /// <param name="posiY">The posi y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="flipX">The flip x.</param>
        /// <param name="flipY">The flip y.</param>
        /// <param name="rotate">The rotate.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Lỗi định dạng</exception>
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
