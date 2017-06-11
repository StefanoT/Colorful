using System;
using System.Drawing;

namespace Colorful.Model
{
    public class ColorService : IColorService
    {
        /// <summary>
        /// implements image colorfulness measure with the Hasler and Süsstrunk algorithm
        /// </summary>
        /// <param name="imageFileName">input image file to process</param>
        /// <returns>colorfulness measure</returns>
        /// <remarks>http://www.pyimagesearch.com/2017/06/05/computing-image-colorfulness-with-opencv-and-python/</remarks>
        public double ComputeColorIndex(string imageFileName)
        {
            Bitmap sourceImage;
            try
            {
                sourceImage = new Bitmap(imageFileName);
            } catch (Exception e)
            {
                sourceImage = null;
            }
            if (sourceImage == null)
                return 0;

            int totalPixels = sourceImage.Width * sourceImage.Height;
            if (totalPixels <= 0)
                return 0;
            // compute mean
            double rg_mean = 0;
            double yb_mean = 0;
            for (int y = 0; y < sourceImage.Height; y++)
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Color currentPixel = sourceImage.GetPixel(x, y);
                    rg_mean += Math.Abs((int)currentPixel.R - (int)currentPixel.G);
                    yb_mean += Math.Abs(0.5 * ((int)currentPixel.R + (int)currentPixel.G) - currentPixel.B);
                }
            rg_mean /= totalPixels;
            yb_mean /= totalPixels;
            double mean = Math.Sqrt(Math.Pow(rg_mean, 2) + Math.Pow(yb_mean, 2));
            // compute standard deviation
            double rg_stddev = 0;
            double yb_stddev = 0;
            for (int y = 0; y < sourceImage.Height; y++)
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Color currentPixel = sourceImage.GetPixel(x, y);
                    double rg = Math.Abs((int)currentPixel.R - (int)currentPixel.G);
                    double yb = Math.Abs(0.5 * ((int)currentPixel.R + (int)currentPixel.G) - currentPixel.B);
                    rg_stddev += Math.Pow(rg - rg_mean, 2);
                    yb_stddev += Math.Pow(yb - yb_mean, 2);
                }
            rg_stddev /= totalPixels;
            rg_stddev = Math.Sqrt(rg_stddev);
            yb_stddev /= totalPixels;
            yb_stddev = Math.Sqrt(yb_stddev);
            double stddev = Math.Sqrt(Math.Pow(rg_stddev, 2) + Math.Pow(yb_stddev, 2));

            return 0.3 * mean + stddev;
        }
    }
}