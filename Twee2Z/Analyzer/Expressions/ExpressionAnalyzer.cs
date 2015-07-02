using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.IO;
using Twee2Z.Utils;
using Twee2Z.ObjectTree.Expressions;

namespace Twee2Z.Analyzer.Expressions
{
    class ExpressionAnalyzer
    {
        public static Expression Parse(string input)
        {
            Logger.LogAnalyzer("Parse Expression ...");
            return ParseTokens(Lex(input));
        }

        private static Expression ParseTokens(CommonTokenStream input)
        {
            ExpressionParser.ExpressionContext startContext = new ExpressionParser(input).expression();

            ExpressionVisitor visit = new ExpressionVisitor();
            Expression expr = visit.Visit(startContext);
            return expr;
        }

        public static CommonTokenStream Lex(string input)
        {
            AntlrInputStream antlrStream = new AntlrInputStream(input);
            return new CommonTokenStream(new ExpressionLexer(antlrStream));
        }
    }
}
