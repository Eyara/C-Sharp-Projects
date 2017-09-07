using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Archiver
{
    class ImageCompression
    {
        private Bitmap image1;
        private Color RGBColor;
        private string path;
        private string pathWrite;
        private Tuple<byte, byte, byte> rgb;
        private List< Tuple<byte, byte, byte> > pixels = new List< Tuple<byte, byte, byte> >();
        private Dictionary< Tuple<byte, byte, byte>, int > pixelsFrequency = 
            new Dictionary< Tuple<byte, byte, byte>, int > ();
        private Image file;

        public ImageCompression()
        {
            path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Test1.jpg";
            pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputImage.txt";
            file = Image.FromFile(path);
            image1 = new Bitmap(file);
        }
        private List < Tuple<byte, byte, byte> > GetPix ()
        {
            for (int i = 0; i < file.Width; i++)
            {
                for (int j = 0; j < file.Height; j++)
                {
                    RGBColor = image1.GetPixel(i, j);
                    rgb = Tuple.Create(RGBColor.R, RGBColor.G, RGBColor.B);
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
                if (Math.Abs(KeyValue.Key.Item1 - pixels.Item1) <= 15 &&
                    Math.Abs(KeyValue.Key.Item2 - pixels.Item2) <= 15 &&
                    Math.Abs(KeyValue.Key.Item3 - pixels.Item3) <= 15)
                {
                    pixelsFrequency.Remove(KeyValue.Key);
                    pixelsFrequency.Add(KeyValue.Key, KeyValue.Value + 1);
                    return;
                }
            }
            pixelsFrequency.Add(pixels, 1);
        }
        private Dictionary <Tuple <byte, byte, byte>, int> GetDict ()
        {
            pixels = GetPix();
            pixelsFrequency.Add(pixels[0], 0);
            for (int i = 1; i < pixels.Count; i++)
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
            var pixelDict = tree.GetPixelDict();
            List <string> pixelsCodes = new List<string>();
            foreach (var e in pixels)
            {
                foreach (var KeyValue in pixelDict)
                {
                    if (Math.Abs(e.Item1 - KeyValue.Key.Item1) <= 15 &&
                        Math.Abs(e.Item2 - KeyValue.Key.Item2) <= 15 &&
                        Math.Abs(e.Item3 - KeyValue.Key.Item3) <= 15)
                    {
                        pixelsCodes.Add(KeyValue.Value);
                        break;
                    }
                }
            }
            List<bool> pixBytes = new List<bool>();
            foreach (var e in pixelsCodes)
            {
               foreach (char symb in e)
                {
                    pixBytes.Add(symb == '1' ? true : false);
                }
            }
            BitArray bit_array = new BitArray(pixBytes.ToArray());
            byte[] bytes = new byte[bit_array.Length / 8 + (bit_array.Length % 8 == 0 ? 0 : 1)];
            bit_array.CopyTo(bytes, 0);
            File.WriteAllBytes(pathWrite, bytes);
        } 
        public void Decode()
        {
            BinaryTree tree = new BinaryTree();
            Bitmap newImage = new Bitmap(image1.Width, image1.Height);
            int index = 0;
            var pixFreq = GetDict();
            foreach (var e in pixFreq.Keys)
            {
                tree.Add(e, index);
                index++;
            }
            var pixelDict = tree.GetPixelDict();
            BitArray archiveFile = new BitArray(File.ReadAllBytes(pathWrite));
            int codeMaxLength = 0;
            // ищем максимальную длины кодированной строки
            foreach (var KeyValue in pixelDict)
            {
                codeMaxLength = codeMaxLength < KeyValue.Value.Length ? KeyValue.Value.Length : codeMaxLength;
            }

            // Декодирование
            List<Tuple<byte, byte, byte>> result = new List<Tuple<byte, byte, byte>>(); 
            string symbol;
            bool[] encodeFile = new bool[archiveFile.Length];
            archiveFile.CopyTo(encodeFile, 0);
            bool symbolWasFound = false;
            bool first = true;
            for (int i = 0; i < encodeFile.Length - codeMaxLength; i++)
            {
                symbol = "";
                for (int j = i; j < i + codeMaxLength; j++)
                {
                    if (first)
                    {
                        for (int k = 0; k < codeMaxLength; k++)
                        {
                            symbol += (encodeFile[i + k].ToString() == "True" ? 1 : 0);
                        }
                        first = false;
                    }
                    foreach (var KeyPair in pixelDict)
                    {
                        if (KeyPair.Value == symbol)
                        {
                            result.Add(KeyPair.Key);
                            symbolWasFound = true;
                            break;
                        }
                    }
                    if (symbolWasFound)
                    {
                        i += symbol.Length - 1;
                        first = true;
                        symbolWasFound = false;
                        break;
                    }
                    else
                    {
                        try
                        {
                            symbol = symbol.Substring(0, symbol.Length - 1);
                        }
                        catch (Exception e)
                        {
                            ;
                        }
                    }
                }
            }

            // Установка пикселей и сохранение файла
            Color ccolor;
            int countSetPixel = 0;
            for (int i = 0; i < newImage.Width; i++)
            {
                for (int j = 0; j < newImage.Height; j++)
                {
                    try
                    {
                        ccolor = Color.FromArgb(255, result[countSetPixel].Item1,
                            result[countSetPixel].Item2, result[countSetPixel].Item3);
                        countSetPixel++;
                        newImage.SetPixel(i, j, ccolor);
                    }
                    catch (Exception e)
                    {
                        ccolor = Color.FromArgb(255, 0, 0, 0);
                        countSetPixel++;
                        newImage.SetPixel(i, j, ccolor);
                    }
                }
            }
            newImage.Save(@"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output1.jpg", 
                System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
