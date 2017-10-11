using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
namespace Archiver
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\TestMTF.txt";
            //string pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputMTF.txt";
            //BWT bwt = new BWT();
            //bwt.Encode();
            //MTF mtf = new MTF(path, pathWrite);
            //mtf.Encode();
            AudioCompression audio = new AudioCompression();
            audio.ReadWAVFile();
            //TextCompression text = new TextCompression();
            //text.Encode();
            Console.WriteLine("Кодирование завершено.");
            audio.Decode();
            //text.Decode();
            Console.WriteLine("Декодирование завершено.");
            // ImageCompression image = new ImageCompression();
            // image.Encode();
            // image.Decode();
            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
