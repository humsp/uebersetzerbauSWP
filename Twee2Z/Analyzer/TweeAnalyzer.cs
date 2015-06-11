using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Twee2Z.Utils;

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
            Logger.LogAnalyzer("Parse twee file ...");
            Twee.StartContext startContext = new Twee(input).start();

            TweeVisitor visit = new TweeVisitor();
            visit.Visit(startContext);
            Logger.LogAnalyzer("Convert parse tree into object tree ...");
            return visit.Tree;
        }

        public static CommonTokenStream Lex(StreamReader input)
        {
            Logger.LogAnalyzer("Lex twee file ...");
            AntlrInputStream antlrStream = new AntlrInputStream(input.ReadToEnd());
            return new CommonTokenStream(new LEX(antlrStream));
        }
    }
}
