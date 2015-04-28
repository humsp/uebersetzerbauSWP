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
            FileStream fs = File.Open(passgaeOnly, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader inputStream = new StreamReader(fs);
            Lexer.Lex.LexStream(inputStream);


            System.Console.Read();
            /*
            System.Console.WriteLine("Welcome to Twee2Z");
            System.Console.WriteLine("Try tokens like '<<display Page>>', '[[Place]]' or '<html><br></html>'");
            System.Console.WriteLine("");
            while (true)
            {
                System.Console.Write("Please enter a token: ");
                string input = System.Console.ReadLine();

                if (Regex.IsMatch(input, Pattern.Link))
                    System.Console.WriteLine("Link\n");
                else if (Regex.IsMatch(input, Pattern.Macro))
                    System.Console.WriteLine("Macro\n");
                else if (Regex.IsMatch(input, Pattern.Image))
                    System.Console.WriteLine("Image\n");
                else if (Regex.IsMatch(input, Pattern.HtmlBlock))
                    System.Console.WriteLine("HtmlBlock\n");
                else if (Regex.IsMatch(input, Pattern.Html))
                    System.Console.WriteLine("Html\n");
                else if (Regex.IsMatch(input, Pattern.InlineStyle))
                    System.Console.WriteLine("InlineStyle\n");
                else if (Regex.IsMatch(input, Pattern.Mono))
                    System.Console.WriteLine("Mono\n");
                else if (Regex.IsMatch(input, Pattern.Comment))
                    System.Console.WriteLine("Comment\n");
                else
                    System.Console.WriteLine("Unknown token\n");
            }*/
        }
    }
}
