using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archiver
{
    class ImageCompression
    {
        private Bitmap image1;
        private Bitmap image2;
        private Color RGBColor;
        private string path;
        private string path2;
        private Tuple<byte, byte, byte> rgb;
        private List< Tuple<byte, byte, byte> > pixels = new List< Tuple<byte, byte, byte> >();
        private Dictionary< Tuple<byte, byte, byte>, int > pixelsFrequency = 
            new Dictionary< Tuple<byte, byte, byte>, int > ();
        private Image file;

        public ImageCompression()
        {
            path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Lenna.png";
           
            file = Image.FromFile(path);
            image1 = new Bitmap(file);
            image2 = new Bitmap(file);
        }
        private List < Tuple<byte, byte, byte> > GetPix ()
        {
            for (int i = 0; i < file.Width; i++)
            {
                for (int j = 0; j < file.Height; j++)
                {
                    RGBColor = image1.GetPixel(i, j);
                    rgb = Tuple.Create(RGBColor.R, RGBColor.B, RGBColor.A);
                    pixels.Add(rgb);
                }
            }
            return pixels;
        }
        private void CountPixels (Tuple <byte, byte, byte> pixels,
             Dictionary<Tuple<byte, byte, byte>, int> pixelsFrequency)
        {
            foreach (var KeyValue in pixelsFrequency)
            {
                if (Math.Abs(KeyValue.Key.Item1 - pixels.Item1) <= 10 &&
                    Math.Abs(KeyValue.Key.Item2 - pixels.Item2) <= 10 &&
                    Math.Abs(KeyValue.Key.Item3 - pixels.Item3) <= 10)
                {
                    pixelsFrequency.Remove(KeyValue.Key);
                    pixelsFrequency.Add(KeyValue.Key, KeyValue.Value + 1);
                    return;
                }
            }
            pixelsFrequency.Add(pixels, 1);
        }
        private Dictionary <Tuple <byte, byte, byte> ,int> GetDict ()
        {
            pixels = GetPix();
            pixelsFrequency.Add(pixels[0], 0);
            for (int i = 0; i < pixels.Count; i++)
            {
                CountPixels(pixels[i], pixelsFrequency);
            }
            return pixelsFrequency;
        }
        public void Encode()
        {
            BinaryTree tree = new BinaryTree();
            int index = 0;
            var pixFreq = GetDict();
            foreach (var e in pixFreq.Keys)
            {
                tree.Add(e, index);
                index++;
            }
            var x = tree.GetPixelDict();
            
        } 
        public void SetPixels ()
        {
            var x = GetDict().Reverse();
            Color ccolor;
            for (int i = 0; i < image2.Width; i++)
            {
                for (int j = 0; j < image2.Height; j++)
                {
                    foreach (var e in x)
                    { 
                        for (int k = 0; k < pixels.Count; k++)
                        {
                            if (Math.Abs(e.Key.Item1 - pixels[k].Item1) <= 5 &&
                                Math.Abs(e.Key.Item2 - pixels[k].Item2) <= 5 &&
                                Math.Abs(e.Key.Item3 - pixels[k].Item3) <= 5)
                            {
                                ccolor = Color.FromArgb(pixels[k].Item1, pixels[k].Item2, pixels[k].Item3);
                                image2.SetPixel(i, j, ccolor);
                                break;
                                // Console.WriteLine(ccolor);
                            }
                        }
                    }
                }
            }
            image2.Save(@"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output1.png", System.Drawing.Imaging.ImageFormat.Png);

        }
    }
}
