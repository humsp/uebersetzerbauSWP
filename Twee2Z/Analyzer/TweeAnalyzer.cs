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

        public static TreeBuilder RunHelloWorldDemo(StreamReader input)
        {
            System.Console.WriteLine("Lex twee file ...");
            CommonTokenStream tokenStream = Lex(input);
            System.Console.WriteLine("Parse twee file ...");
            TweeParser.StartContext startContext = new TweeParser(tokenStream).start();
            TweeVisitor visit = new TweeVisitor();
            visit.Visit(startContext);
            System.Console.WriteLine("Convert parse tree into object tree ...");
            TreeBuilder treeBuilder = new TreeBuilder(startContext);
            treeBuilder.BaumDurchlauf();
            return treeBuilder;
        }
    }
}
