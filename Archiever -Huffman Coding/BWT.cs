using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Archiver
{
    class BWT
    {
        public void Encode()
        {
            StreamReader reader = new StreamReader
                (@"C:\Users\Eyara\Desktop\Программирование\web-site\style.css");
            StreamWriter writer = new StreamWriter
                (@"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\TestMTF.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                StringBuilder result = new StringBuilder();
                string[] rotateLines = new string[line.Length];
                for (int i = 0; i < line.Length; i++)
                {
                    line = (i != 0) ? line.Substring(1, line.Length - 1) + line.Substring(0, 1) : line;
                    rotateLines[i] = line;
                }
                Array.Sort(rotateLines);
                foreach (var e in rotateLines)
                {
                    result.Append(e[e.Length - 1]);
                }
                writer.WriteLine(result);
            }
            reader.Close();
            writer.Close();
        }
    }
}

// кодирование работает
// написать декодер
