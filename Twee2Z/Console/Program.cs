using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Twee2Z.Lexer;
using System.IO;

namespace Twee2Z.Console
{
    class Program
    {
        static string rootFolder = "..\\..\\..\\tweeFiles\\";
        static string passgaeOnly = rootFolder + "passageOnly.tw";
        static string passgae = rootFolder + "passage.tw";

        static void Main(string[] args)
        {
            FileStream fs = File.Open(passgae, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader inputStream = new StreamReader(fs);
            Lexer.Analyser.LexStream(inputStream);


            System.Console.Read();
        }
    }
}
