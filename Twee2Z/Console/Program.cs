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

                if (Pattern.Link.IsMatch(input))
                    System.Console.WriteLine("Link\n");
                else if (Pattern.Macro.IsMatch(input))
                    System.Console.WriteLine("Macro\n");
                else if (Pattern.Image.IsMatch(input))
                    System.Console.WriteLine("Image\n");
                else if (Pattern.HtmlBlock.IsMatch(input))
                    System.Console.WriteLine("HtmlBlock\n");
                else if (Pattern.Html.IsMatch(input))
                    System.Console.WriteLine("Html\n");
                else if (Pattern.InlineStyle.IsMatch(input))
                    System.Console.WriteLine("InlineStyle\n");
                else if (Pattern.Mono.IsMatch(input))
                    System.Console.WriteLine("Mono\n");
                else if (Pattern.Comment.IsMatch(input))
                    System.Console.WriteLine("Comment\n");
                else
                    System.Console.WriteLine("Unknown token\n");
            }
        }
    }
}
