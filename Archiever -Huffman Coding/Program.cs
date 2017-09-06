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
        static void Decode(Dictionary<char, string> dictDecode, bool[] boolArchiveFile, StreamWriter fileWriter)
        {
            string symbol = "";
            List<string> lines = new List<string>();
            char result = ' ';
            bool symbolWasFound = false;
            bool first = true;
            int maxLength = 0;
            foreach (var KeyValue in dictDecode)
            {
                if (KeyValue.Value.Length > maxLength)
                {
                    maxLength = KeyValue.Value.Length;
                }
            }
            for (int i = 0; i < boolArchiveFile.Length - maxLength; i++)
            {
                symbol = "";
                for (int j = i; j < i + maxLength; j++)
                {
                    if (first)
                    {
                        for (int k = 0; k < maxLength; k++)
                        {
                            symbol += (boolArchiveFile[i + k].ToString() == "True" ? 1 : 0);
                        }
                        first = false;
                    }
                    foreach (var KeyPair in dictDecode)
                    {
                        if (KeyPair.Value == symbol)
                        {
                            result = KeyPair.Key;
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
                lines.Add(result.ToString());
            }
            foreach (string line in lines)
            {
                Console.Write(line);
                fileWriter.WriteLine(line);
            }
            fileWriter.Close();
            //Console.WriteLine();
            //for (int i = 0; i < boolArchiveFile.Length; i++)
            //{
            //    symbol += (boolArchiveFile[i].ToString() == "True" ? 1 : 0);
            //}
            //Console.WriteLine(symbol);
            //foreach (var KeyPair in dictDecode)
            //{
            //    Console.WriteLine(KeyPair.Key + ": " + KeyPair.Value);
            //}
        }
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
            string path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Test.txt";
            string pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output";
            string pathDecode = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputDecode.txt";
            string line = "";
            StreamReader file = new StreamReader(path);
            StreamReader file2 = new StreamReader(path);
            StreamWriter fileWriter = new StreamWriter(pathDecode);
            BinaryTree tree = new BinaryTree();
            BitArray archiveFile = new BitArray(File.ReadAllBytes(pathWrite));
            Dictionary <char, int> symbols = new Dictionary<char, int>();
            List <char> symbolsOrdered = new List<char>();
            int indexList = 0;
            symbols.Add(' ', 0); 
            // Иначе не будет итераций в цикле foreach в методе CountSymbols
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
            bool[] boolArchiveFile = new bool[archiveFile.Length];
            archiveFile.CopyTo(boolArchiveFile, 0);
            //  Decode(tree.GetDict(), boolArchiveFile, fileWriter);
            ImageCompression image = new ImageCompression();
            image.Encode();
            Console.WriteLine("Готово!");
            Console.ReadKey();
        }
    }
}
//todo: Добавить дешефрование
