using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Archiver
{
    class MTF
    {
        private string line;
        private StreamReader reader;
        private StreamWriter writer;
        public MTF(string path, string pathWrite)
        {
            reader = new StreamReader(path);
            writer = new StreamWriter(pathWrite);
        }

        public void Encode()
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                List<char> dictChar = new List<char>();
                List<byte> result = new List<byte>();

                foreach (var ch in line.ToString())
                {
                    if (!dictChar.Contains(ch)) dictChar.Add(ch);
                }
                for (int i = 0; i < line.Length; i++)
                {
                    result.Add((byte)dictChar.IndexOf(line[i]));
                    if (dictChar.IndexOf(line[i]) != 0)
                    {
                        int j = dictChar.IndexOf(line[i]);
                        while (j != 0)
                        {
                            Swap(dictChar, j, j - 1);
                            j--;
                        }
                    }
                }
                foreach (var e in result)
                {
                    writer.Write(e);
                }
                writer.WriteLine();
            }
            writer.Close();
        }
        private void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }
}

// Кодирование работает
// todo: Написать декодер
