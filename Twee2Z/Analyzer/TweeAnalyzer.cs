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
        
        public static ObjectTree.Tree Parse(StreamReader input)
        {
            return Parse2(Lex(input));
        }
        
        public static ObjectTree.Tree Parse2(CommonTokenStream input)
        {
            System.Console.WriteLine("Parse twee file ...");
            Twee.StartContext startContext = new Twee(input).start();

            TweeVisitor visit = new TweeVisitor();
            visit.Visit(startContext);
            System.Console.WriteLine("Convert parse tree into object tree ...");
            return visit.Tree;
        }         

        public static CommonTokenStream Lex(StreamReader input)
        {
            System.Console.WriteLine("Lex twee file ...");
            AntlrInputStream antlrStream = new AntlrInputStream(input.ReadToEnd());
            return new CommonTokenStream(new LEX(antlrStream));
        }
    }
}
