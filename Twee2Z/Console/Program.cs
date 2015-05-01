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
        static string rootFolder = "..\\..\\..\\TestFiles\\";
        static string twee = rootFolder  + "twee\\";
        static string zCode = rootFolder + "zcode\\";

        static string passgaeOnly = twee + "passageOnly.tw";
        static string passgae = twee + "passage.tw";

        static string zHelloWorld = zCode + "hw.z8"; // Datei funktioniert noch nicht

        static void Main(string[] args)
        {
            FileStream fs = File.Open(passgae, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader inputStream = new StreamReader(fs);
            Lexer.Analyser.LexStream(inputStream);

            //ZCodeInterpreter.Interpreter.Run(zHelloWorld);
            System.Console.Read();
        }
    }
}
