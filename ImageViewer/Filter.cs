using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageViewer
{
    class Filter
    {
        public Bitmap Grayscale (Bitmap image)
        {
            int rgb;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    rgb = Convert.ToInt32(0.3 * currentPixel.R + 0.587 * currentPixel.G + 0.114 * currentPixel.B);
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(rgb, rgb, rgb));
                }
            }
            return image;
        }

        public Bitmap Sepia (Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    int r = Convert.ToInt32(0.393 * currentPixel.R + 0.769 * currentPixel.G + 0.189 * currentPixel.B);
                    int g = Convert.ToInt32(0.349 * currentPixel.R + 0.686 * currentPixel.G + 0.168 * currentPixel.B);
                    int b = Convert.ToInt32(0.272 * currentPixel.R + 0.534 * currentPixel.G + 0.131 * currentPixel.B);
                    r = r > 255 ? 255 : r;
                    g = g > 255 ? 255 : g;
                    b = g > 255 ? 255 : b;
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(r, g, b));
                }
            }
            return image;
        }

        public Bitmap Pink (Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    int r = Convert.ToInt32(0.593 * currentPixel.R + 0.569 * currentPixel.G + 0.189 * currentPixel.B);
                    r = r > 255 ? 255 : r;
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(r, currentPixel.G, currentPixel.B));
                }
            }
            return image;
        }

        public Bitmap Greeny(Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    int g = Convert.ToInt32(0.349 * currentPixel.R + 0.686 * currentPixel.G + 0.168 * currentPixel.B);
                    g = g > 255 ? 255 : g;
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(currentPixel.R, g, currentPixel.B));
                }
            }
            return image;
        }

        public Bitmap Yellow(Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    int r = Convert.ToInt32(0.7 * currentPixel.R + 0.45 * currentPixel.G + 0.1 * currentPixel.B);
                    int g = Convert.ToInt32(0.5 * currentPixel.R + 0.65 * currentPixel.G + 0.1 * currentPixel.B);
                    r = r > 255 ? 255 : r;
                    g = g > 255 ? 255 : g;
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(r, g, currentPixel.B));
                }
            }
            return image;
        }

        public Bitmap Violet(Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var currentPixel = image.GetPixel(i, j);
                    int r = Convert.ToInt32(0.45 * currentPixel.R + 0.1 * currentPixel.G + 0.5 * currentPixel.B);
                    int b = Convert.ToInt32(0.4 * currentPixel.R + 0.2 * currentPixel.G + 0.8 * currentPixel.B);
                    r = r > 255 ? 255 : r;
                    b = b > 255 ? 255 : b;
                    image.SetPixel(i, j, System.Drawing.Color.FromArgb(r, currentPixel.G, b));
                }
            }
            return image;
        }
    }
}
