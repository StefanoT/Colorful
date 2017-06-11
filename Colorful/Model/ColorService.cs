/*
Project: Colorful
Author: Stefano Tommesani
Website: http://www.tommesani.com
Microsoft Public License (MS-PL) [OSI Approved License]
This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.
2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
*/

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