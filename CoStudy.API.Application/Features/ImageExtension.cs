using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;

namespace CoStudy.API.Application.Features
{
    public static class ImageExtension
    {

        enum Dimensions
        {
            Width,
            Height
        }
        enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }

        public static System.Drawing.Imaging.ImageFormat ParseImageFormat(string str)
        {
            try
            {
                if (str == "jpg")
                    str = "jpeg";
                if (str == "tif")
                    str = "tiff";
                str = str.First().ToString().ToUpper() + str.Substring(1);
                return (System.Drawing.Imaging.ImageFormat)typeof(System.Drawing.Imaging.ImageFormat)
                        .GetProperty(str, BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase)
                        .GetValue(null);
            }
            catch (Exception)
            {
                throw new Exception("Lỗi định dạng");
            }
        }

        public static Image ScaleByPercent(Image imgPhoto, double Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Transparent);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image CropImage(Image imgPhoto, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(imgPhoto);
            try
            {
                Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
                return (Image)(bmpCrop);
            }
            catch (Exception)
            {
                throw new Exception("Ngoài vùng crop");
            }

        }

        /// <summary>
        /// Change image's color to black and white
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <returns></returns>
        public static Image ToBlackAndWhite(Image imgPhoto)
        {
            Bitmap bmp = new Bitmap(imgPhoto);
            int rgb;
            Color c;

            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    c = bmp.GetPixel(x, y);
                    rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            return bmp;

        }

        public static Image Flip180X(Image imgPhoto)
        {
            imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return imgPhoto;
        }

        public static Image Flip180Y(Image imgPhoto)
        {
            imgPhoto.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return imgPhoto;
        }

        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> that is just large enough
        /// to contain the rotated image without cutting any corners off.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            const double pi2 = Math.PI / 2.0;

            // Why can't C# allow these to be const, or at least readonly
            // *sigh*  I'm starting to talk like Christian Graus :omg:
            double oldWidth = (double)image.Width;
            double oldHeight = (double)image.Height;

            // Convert degrees to radians
            double theta = ((double)angle) * Math.PI / 180.0;
            double locked_theta = theta;

            // Ensure theta is now [0, 2pi)
            while (locked_theta < 0.0)
                locked_theta += 2 * Math.PI;

            double newWidth, newHeight;
            int nWidth, nHeight; // The newWidth/newHeight expressed as ints

            #region Explaination of the calculations
            /*
			 * The trig involved in calculating the new width and height
			 * is fairly simple; the hard part was remembering that when 
			 * PI/2 <= theta <= PI and 3PI/2 <= theta < 2PI the width and 
			 * height are switched.
			 * 
			 * When you rotate a rectangle, r, the bounding box surrounding r
			 * contains for right-triangles of empty space.  Each of the 
			 * triangles hypotenuse's are a known length, either the width or
			 * the height of r.  Because we know the length of the hypotenuse
			 * and we have a known angle of rotation, we can use the trig
			 * function identities to find the length of the other two sides.
			 * 
			 * sine = opposite/hypotenuse
			 * cosine = adjacent/hypotenuse
			 * 
			 * solving for the unknown we get
			 * 
			 * opposite = sine * hypotenuse
			 * adjacent = cosine * hypotenuse
			 * 
			 * Another interesting point about these triangles is that there
			 * are only two different triangles. The proof for which is easy
			 * to see, but its been too long since I've written a proof that
			 * I can't explain it well enough to want to publish it.  
			 * 
			 * Just trust me when I say the triangles formed by the lengths 
			 * width are always the same (for a given theta) and the same 
			 * goes for the height of r.
			 * 
			 * Rather than associate the opposite/adjacent sides with the
			 * width and height of the original bitmap, I'll associate them
			 * based on their position.
			 * 
			 * adjacent/oppositeTop will refer to the triangles making up the 
			 * upper right and lower left corners
			 * 
			 * adjacent/oppositeBottom will refer to the triangles making up 
			 * the upper left and lower right corners
			 * 
			 * The names are based on the right side corners, because thats 
			 * where I did my work on paper (the right side).
			 * 
			 * Now if you draw this out, you will see that the width of the 
			 * bounding box is calculated by adding together adjacentTop and 
			 * oppositeBottom while the height is calculate by adding 
			 * together adjacentBottom and oppositeTop.
			 */
            #endregion

            double adjacentTop, oppositeTop;
            double adjacentBottom, oppositeBottom;

            // We need to calculate the sides of the triangles based
            // on how much rotation is being done to the bitmap.
            //   Refer to the first paragraph in the explaination above for 
            //   reasons why.
            if ((locked_theta >= 0.0 && locked_theta < pi2) ||
                (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2)))
            {
                adjacentTop = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
                oppositeTop = Math.Abs(Math.Sin(locked_theta)) * oldWidth;

                adjacentBottom = Math.Abs(Math.Cos(locked_theta)) * oldHeight;
                oppositeBottom = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
            }
            else
            {
                adjacentTop = Math.Abs(Math.Sin(locked_theta)) * oldHeight;
                oppositeTop = Math.Abs(Math.Cos(locked_theta)) * oldHeight;

                adjacentBottom = Math.Abs(Math.Sin(locked_theta)) * oldWidth;
                oppositeBottom = Math.Abs(Math.Cos(locked_theta)) * oldWidth;
            }

            newWidth = adjacentTop + oppositeBottom;
            newHeight = adjacentBottom + oppositeTop;

            nWidth = (int)Math.Ceiling(newWidth);
            nHeight = (int)Math.Ceiling(newHeight);

            Bitmap rotatedBmp = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                // This array will be used to pass in the three points that 
                // make up the rotated image
                Point[] points;

                /*
				 * The values of opposite/adjacentTop/Bottom are referring to 
				 * fixed locations instead of in relation to the
				 * rotating image so I need to change which values are used
				 * based on the how much the image is rotating.
				 * 
				 * For each point, one of the coordinates will always be 0, 
				 * nWidth, or nHeight.  This because the Bitmap we are drawing on
				 * is the bounding box for the rotated bitmap.  If both of the 
				 * corrdinates for any of the given points wasn't in the set above
				 * then the bitmap we are drawing on WOULDN'T be the bounding box
				 * as required.
				 */
                if (locked_theta >= 0.0 && locked_theta < pi2)
                {
                    points = new Point[] {
                                             new Point( (int) oppositeBottom, 0 ),
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( 0, (int) adjacentBottom )
                                         };

                }
                else if (locked_theta >= pi2 && locked_theta < Math.PI)
                {
                    points = new Point[] {
                                             new Point( nWidth, (int) oppositeTop ),
                                             new Point( (int) adjacentTop, nHeight ),
                                             new Point( (int) oppositeBottom, 0 )
                                         };
                }
                else if (locked_theta >= Math.PI && locked_theta < (Math.PI + pi2))
                {
                    points = new Point[] {
                                             new Point( (int) adjacentTop, nHeight ),
                                             new Point( 0, (int) adjacentBottom ),
                                             new Point( nWidth, (int) oppositeTop )
                                         };
                }
                else
                {
                    points = new Point[] {
                                             new Point( 0, (int) adjacentBottom ),
                                             new Point( (int) oppositeBottom, 0 ),
                                             new Point( (int) adjacentTop, nHeight )
                                         };
                }

                g.DrawImage(image, points);
            }

            return rotatedBmp;
        }


        public static Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            Image imgPhoto = image;

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }
    }
}
