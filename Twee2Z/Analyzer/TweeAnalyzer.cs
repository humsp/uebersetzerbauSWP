using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Twee2Z.Analyzer
{
    public static class TweeAnalyzer
    {
        public static void Run(StreamReader input)
        {
            Parse(Lex(input));
        }

        public static CommonTokenStream Lex(StreamReader input)
        {
            AntlrInputStream antlrStream = new AntlrInputStream(input.ReadToEnd());
            return new CommonTokenStream(new TweeLexer(antlrStream));
        }

        public static void Parse(CommonTokenStream input)
        {
            System.Console.WriteLine("Start Twee parsing ...");
            TweeParser.StartContext startContext = new TweeParser(input).start();

            TweeVisitor visit = new TweeVisitor();
            visit.Visit(startContext);
        }
    }
}
