using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.IO;

namespace Twee2Z.Lexer
{
    public class Lex
    {
        public static void LexStream(StreamReader streamReader)
        {
            AntlrInputStream input = new AntlrInputStream(streamReader.ReadToEnd());
            TweeLexer lexer = new TweeLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            TweeParser parser = new TweeParser(tokens);
            TweeParser.StartContext sc = parser.start();
            Console.WriteLine(sc.GetText());

        }
    }
}
