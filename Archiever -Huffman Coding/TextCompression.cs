using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Archiver
{
    class TextCompression
    {
        public string path;
        public string pathWrite;
        public string pathDecode;
        private Dictionary<char, string> dictDecode = new Dictionary<char, string>();
        private bool[] boolArchiveFile;
        StreamReader file;
        StreamReader file2;
        StreamWriter fileWriter;
        public TextCompression()
        {
            path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputAudio.txt";
            pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Output.txt";
            pathDecode = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputDecode.txt";
            file = new StreamReader(path);
            file2 = new StreamReader(path);
        }

        public void Encode()
        {
            BinaryTreeHuffman tree = new BinaryTreeHuffman();
            Dictionary<char, int> symbols = new Dictionary<char, int>();
            List<char> symbolsOrdered = new List<char>();
            string line = "";
            int indexList = 0;

            // Иначе не будет итераций в цикле foreach в методе CountSymbols
            symbols.Add(' ', 0);
            
            // Чтение файла, построение дерева Хаффмана, получение словаря.
            ReadingFile(line, file, symbols);
            symbolsOrdered = GetCharList(symbols, symbolsOrdered);
            foreach (var e in symbolsOrdered)
            {
                tree.Add(e, indexList);
                indexList++;
            }
            dictDecode = tree.GetDict();
            // Запись в файл    
            BitArray bit_array = new BitArray(GetBoolList(dictDecode, file2, line).ToArray());
            byte[] bytes = new byte[bit_array.Length / 8 + (bit_array.Length % 8 == 0 ? 0 : 1)];
            bit_array.CopyTo(bytes, 0);
            File.WriteAllBytes(pathWrite, bytes);
        }
        public void Decode()
        {
            BitArray archiveFile = new BitArray(File.ReadAllBytes(pathWrite));
            boolArchiveFile = new bool[archiveFile.Length];
            archiveFile.CopyTo(boolArchiveFile, 0);
            Stream stream = File.Open(pathDecode, FileMode.Open);
            fileWriter = new StreamWriter(stream);

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
                fileWriter.Write(line);
            }
            fileWriter.Close();
        }
        private List<bool> GetBoolList(Dictionary<char, string> dictDecoder, StreamReader file, string line)
        {
            List<bool> symbols = new List<bool>();
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
        private List<char> GetCharList(Dictionary<char, int> symbols, List<char> symbolsOrdered)
        {
            foreach (KeyValuePair<char, int> KeyValue in symbols.OrderBy(pair => pair.Value))
            {
                symbolsOrdered.Add(KeyValue.Key);
            }
            symbolsOrdered.Reverse();
            return symbolsOrdered;
        }
        private void ReadingFile(string line, StreamReader file, Dictionary<char, int> symbols)
        {
            while ((line = file.ReadLine()) != null)
            {
                foreach (char ch in line)
                {
                    CountSymbols(ch, symbols);
                }
            }
        }
        private void CountSymbols(char symb, Dictionary<char, int> symbols)
        {
            foreach (KeyValuePair<char, int> keyValue in symbols)
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
    }
}

// todo: Исправить декодирование (отстуствует возможность декодировать многострочные файлы)
// todo: Засунуть MTF и BWT в TextCompression