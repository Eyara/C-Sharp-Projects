using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// todo: Проверка победы

namespace Barley_Break
{
    class Program
    {
        static int findIndex(string x, int n, List <string> listNumbers)
        {
            for (int i = 0; i < n; i++)
                if (listNumbers[i] == x)
                    return i;
            return 0;
        }
        static void printField (int n, List <string> listNumbers)
        {
            int count = 0;
            for (int i = 0; i< Math.Sqrt(n); i++)
            {
                for (int j = 0; j<Math.Sqrt(n); j++)
                {
                    Console.Write(listNumbers[count] + " ");
                    count++;
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            int n = 9;
            Random rnd = new Random();
            int emptyBB = rnd.Next(0, n - 1);
            List<BarleyBreak> bbStorage = new List<BarleyBreak>(n);
            bool[] used = new bool[n];
            List<string> listNumbers = new List<string>(n);
            string playerChoice;
            for (int i = 0; i < n; i++)
            {
                used[i] = false;
            }
            int count = 0;
            for (int i = 0; i < n; i++)
            {
                int tmp = rnd.Next(0, n);
                while (used[tmp])
                {
                    tmp = rnd.Next(0, n);
                }
                if (tmp == emptyBB) listNumbers.Add(" ");
                else listNumbers.Add(Convert.ToString(tmp));
                used[tmp] = true;
            }
            for (int i = 0; i < Math.Sqrt(n); i++)
            {
                for (int j = 0; j < Math.Sqrt(n); j++)
                {
                    bbStorage.Add(new BarleyBreak(i, j, listNumbers[count]));
                    count++;
                }
            }
            while (true)
            {
                printField(n, listNumbers);
                playerChoice = Console.ReadLine();
                int playerChoiceIndex = findIndex(playerChoice, n, listNumbers);
                emptyBB = findIndex(" ", n, listNumbers);
                if (bbStorage[emptyBB].CanSwitch(bbStorage[emptyBB].index_x, bbStorage[emptyBB].index_y,
                    bbStorage[playerChoiceIndex].index_x, bbStorage[playerChoiceIndex].index_y))
                {
                    var tmp = listNumbers[emptyBB];
                    listNumbers[emptyBB] = listNumbers[playerChoiceIndex];
                    listNumbers[playerChoiceIndex] = tmp;
                }
                else Console.WriteLine("doesn't work!");
            }
            Console.ReadKey();
        }
    }
    class BarleyBreak
    {
        public int index_x;
        public int index_y;
        public string number;
        public BarleyBreak(int index_x, int index_y, string number)
        {
            this.index_x = index_x;
            this.index_y = index_y;
            this.number = number;
        }
        public bool CanSwitch(int fIndex_x, int fIndex_y, int sIndex_x, int sIndex_y)
        {
            int diffirent = Math.Abs(fIndex_x - sIndex_x) + Math.Abs(fIndex_y - sIndex_y);
            return (diffirent == 1) ? true : false;
        }
        public void Switch(int fIndex_x, int fIndex_y, int sIndex_x, int sIndex_y, string fNumber, string sNumber)
        {
            Swap(ref fIndex_x, ref sIndex_x);
            Swap(ref fIndex_y, ref sIndex_y);
            Swap(ref fNumber, ref sNumber);
        }
        public void Swap<T>(ref T x, ref T y)
        {
            T tmp = x;
            x = y;
            y = tmp;
        }
    }
}
