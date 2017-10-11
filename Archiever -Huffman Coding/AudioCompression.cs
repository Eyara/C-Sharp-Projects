using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Archiver
{
    class AudioCompression
    {
        private string path;
        private string pathWrite;
        Stream waveFileStream;
        BinaryReader reader;
        byte[] header;
        public AudioCompression()
        {
            path = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\Test.wav";
            pathWrite = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\OutputAudio.txt";
            waveFileStream = File.Open(path, FileMode.Open);
            reader = new BinaryReader(waveFileStream);
        }

        public void ReadWAVFile()
        {
            header = reader.ReadBytes(44);
            reader.Close();

            Stream stream = File.Open(path, FileMode.Open);
            reader = new BinaryReader(stream);
            int chunkID = reader.ReadInt32();
            int fileSize = reader.ReadInt32();
            int riffType = reader.ReadInt32();
            int fmtID = reader.ReadInt32();
            int fmtSize = reader.ReadInt32();
            int fmtCode = reader.ReadInt16();
            int channels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            int fmtAvgBPS = reader.ReadInt32();
            int fmtBlockAlign = reader.ReadInt16();
            int bitDepth = reader.ReadInt16();

            if (fmtSize == 18)
            {
                // Read any extra values
                int fmtExtraSize = reader.ReadInt16();
                reader.ReadBytes(fmtExtraSize);
            }

            int dataID = reader.ReadInt32();
            int dataSize = reader.ReadInt32();

            byte[] byteArray = reader.ReadBytes(dataSize);

            int bytesForSamp = bitDepth / 8;
            int samps = dataSize / bytesForSamp;


            ushort[] asFloat = new ushort[samps];
            Buffer.BlockCopy(byteArray, 0, asFloat, 0, dataSize);

            // Rice coding
            // Представление числа n как n = 2^k + r

            int k;
            StringBuilder builder = new StringBuilder();
            StringBuilder subBuilder = new StringBuilder();
            for (int i = 0; i < asFloat.Length; i++)
            {
                asFloat[i] = (asFloat[i] == 0) ? (ushort) 1 : asFloat[i];
                k = Convert.ToInt32(Math.Floor(Math.Log(asFloat[i]) / Math.Log(2)));
                var z = Convert.ToInt16(asFloat[i] % Math.Pow(2, k));
                if (Convert.ToString(Convert.ToByte(k), 2).Length < 4)
                {
                    subBuilder.Append('0', 4 - Convert.ToString(Convert.ToByte(k), 2).Length);
                }
                subBuilder.Append(Convert.ToString(Convert.ToByte(k), 2));  
                builder.Append(subBuilder);
                subBuilder.Clear();
                //       Console.WriteLine(subBuilder);
                int addLength = (k > Convert.ToString(z, 2).Length) ? Math.Abs(k - Convert.ToString(z, 2).Length) : 0;
                subBuilder.Append('0', addLength);
                subBuilder.Append(Convert.ToString(z, 2));
                if (k != 0) builder.Append(subBuilder);
                subBuilder.Clear();
            }

            bool[] boolArchive = new bool[builder.Length];

            string resBuild = builder.ToString();
            for (int i = 0; i < builder.Length; i++)
            {
                boolArchive[i] = (resBuild[i] == '1') ? true : false;
            }
            BitArray bit_array = new BitArray(boolArchive);
            byte[] bytes = new byte[bit_array.Length / 8 + (bit_array.Length % 8 == 0 ? 0 : 1)];
            bit_array.CopyTo(bytes, 0);
            File.WriteAllBytes(pathWrite, bytes);

        }
        public void Decode()
        {
            BitArray archiveFile = new BitArray(File.ReadAllBytes(pathWrite));
            string pathDecode = @"C:\Users\Eyara\Desktop\Программирование\C#\Starter\Archiver\Archiver\DecAudio.wav";
            Stream stream = File.Open(pathDecode, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);

            List<ushort> result = new List<ushort>();
            bool[] encodeFile = new bool[archiveFile.Length];
            archiveFile.CopyTo(encodeFile, 0);

            int len = 0;
            int k = 0;
            int power = 0;
            bool start = true;
            StringBuilder num = new StringBuilder();
            StringBuilder lengthNum = new StringBuilder();
            for (int i = 0; i < encodeFile.Length; i++)
            {         
                if (start)
                {
                    len++;
                    lengthNum.Append(encodeFile[i] == true ? '1' : '0');
                }
                else
                {
                    k--;
                    num.Append(encodeFile[i] == true ? '1' : '0');
                    if (k == 0)
                    {
                        result.Add(Convert.ToUInt16(Convert.ToUInt16(num.ToString(), 2) +
                            Convert.ToUInt16(Math.Pow(2, power))));
                        num.Clear();
                        start = true;
                    }
                }
                if (len == 4)
                {
                    start = false;
                    k = Convert.ToInt32(lengthNum.ToString(), 2);
                    power = k;
                    if (k == 0)
                    {
                        result.Add(0);
                        start = true;
                    }
                    lengthNum.Clear();
                    len = 0;
                }
            }

            writer.Write(header);


            foreach(var e in result)
            {
                writer.Write(e);
            }
            writer.Close();
        } 
    } 
}


// Реализовано кодирование-декодирование данных
// Подобрать оптимальный коэффициент k для сжатия данных
