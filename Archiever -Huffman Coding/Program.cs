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
        static List<bool> GetBoolList(Dictionary <char, string> dictDecoder,StreamReader file, string line) 
        {
            List<bool> symbols = new List<bool> ();
            while ((line = file.ReadLine()) != null)
            {
                foreach (char ch in line)
                {
                    foreach (var KeyValue in dictDecoder)
                    {
                        if (KeyValue.Key == ch)
                        {
                            foreach (var e in KeyValue.Value)
                            {
                                symbols.Add(e == '1' ? true : false);
                            }
                            break;
                        }
                    }
                }
            }
            return symbols;
        } 
        static List <char> GetCharList (Dictionary <char, int> symbols, List <char> symbolsOrdered)
        {
            foreach (KeyValuePair<char, int> KeyValue in symbols.OrderBy(pair => pair.Value))
            {
                symbolsOrdered.Add(KeyValue.Key);
            }
            symbolsOrdered.Reverse();
            return symbolsOrdered;
        }
        static void ReadingFile (string line, StreamReader file, Dictionary <char, int> symbols)
        {
            while ((line = file.ReadLine()) != null)
            {
                foreach (char ch in line)
                {
                    CountSymbols(ch, symbols);
                }
            }
        }
        static void CountSymbols(char symb, Dictionary<char, int> symbols)
        {
            foreach (KeyValuePair <char, int> keyValue in symbols)
            {
                if (keyValue.Key == symb)
                {
                    symbols.Remove(keyValue.Key);
                    symbols.Add(symb, keyValue.Value + 1);
                    return;
                }
            }
            symbols.Add(symb, 1);
        }
        static void Main(string[] args)
        {
            string path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output1";
            string pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output2";
            string line = "";
            StreamReader file = new StreamReader(path);
            StreamReader file2 = new StreamReader(path);
            BinaryTree tree = new BinaryTree();
            Dictionary <char, int> symbols = new Dictionary<char, int>();
            List <char> symbolsOrdered = new List<char>();
            int indexList = 0;
            symbols.Add(' ', 0); // Иначе не будет итераций в цикле foreach в методе CountSymbols
            ReadingFile(line, file, symbols);
            symbolsOrdered = GetCharList(symbols, symbolsOrdered);
            foreach (var e in symbolsOrdered)
            {
                tree.Add(e, indexList);
                indexList++;
            }
            List <bool> symBytes = GetBoolList(tree.GetDict(), file2, line);
            BitArray bit_array = new BitArray(symBytes.ToArray());
            byte[] bytes = new byte[bit_array.Length / 8 + (bit_array.Length % 8 == 0 ? 0 : 1)];
            bit_array.CopyTo(bytes, 0);
            File.WriteAllBytes(pathWrite, bytes);
            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
