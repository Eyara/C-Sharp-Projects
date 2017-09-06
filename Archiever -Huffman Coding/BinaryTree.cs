using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Archiver
{
    class BinaryNode
    {
        public BinaryNode left { get; set; }
        public BinaryNode right { get; set; }
        public string value;
        public BinaryNode(string value)
        {
            this.value = value;
            right = null;
            left = null;
        }
    }

    class BinaryTree
    {
        public BinaryNode root;
        private Dictionary<char, string> charDict = new Dictionary<char, string>();
        private Dictionary<Tuple<byte, byte, byte>, string> pixelDict = new 
            Dictionary<Tuple<byte, byte, byte>, string>();
        public BinaryTree()
        {
            root = new BinaryNode(" ");
            root.right = new BinaryNode(" ");
            root.left = new BinaryNode(" ");
        }
        public void Add(char value, int index)
        {
            AddTo(root, value, "", index);
        }
        public void Add(Tuple<byte, byte, byte> pixel, int index) {
            AddTo(root, pixel, "", index);
        }
        private void AddTo(BinaryNode node, char value, string code, int index)
        {
            if (index % 2 == 0)
            {
                if (node.right == null)
                {
                    code += "0";
                    node.right = new BinaryNode(code);
                    charDict.Add(value, code);
                    node.left = new BinaryNode("*");
                }
                else
                {
                    AddTo(node.left, value, code + "1", index);
                }
            }
            else
            {
                if (node.left == null)
                {
                    code += "1";
                    node.left = new BinaryNode(code);
                    charDict.Add(value, code);
                    node.right = new BinaryNode("*");
                }
                else
                {
                    AddTo(node.right, value, code + "0", index);
                }
            }
        }
        private void AddTo(BinaryNode node, Tuple<byte, byte, byte> pixel, string code, int index)
        {
            if (index % 2 == 0)
            {
                if (node.right == null)
                {
                    code += "0";
                    node.right = new BinaryNode(code);
                    pixelDict.Add(pixel, code);
                    node.left = new BinaryNode("*");
                }
                else
                {
                    AddTo(node.left, pixel, code + "1", index);
                }
            }
            else
            {
                if (node.left == null)
                {
                    code += "1";
                    node.left = new BinaryNode(code);
                    pixelDict.Add(pixel, code);
                    node.right = new BinaryNode("*");
                }
                else
                {
                    AddTo(node.right, pixel, code + "0", index);
                }
            }
        }
        public void PrintTree(BinaryNode node)
        {
            if (node.left != null)
            {
                if (node.left.value != "*")
                    Console.WriteLine(node.left.value);
                PrintTree(node.left);
            }
            if (node.right != null)
            {
                if (node.right.value != "*")
                    Console.WriteLine(node.right.value);
                PrintTree(node.right);
            }
        }
        public Dictionary<char, string> GetDict()
        {
            return charDict;
        }
        public Dictionary<Tuple<byte, byte, byte>, string> GetPixelDict()
        {
            return pixelDict;
        }
    }
}
