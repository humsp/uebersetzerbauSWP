using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Twee2Z.Lexer;

namespace Twee2Z.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
                else if (Regex.IsMatch(input, Pattern.Variable))
                    System.Console.WriteLine("Variable\n");
                else if (Regex.IsMatch(input, Pattern.Function))
                    System.Console.WriteLine("Function\n");
                else
                    System.Console.WriteLine("Unknown token\n");
            }
        }
    }
}
