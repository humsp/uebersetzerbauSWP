using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;
using Twee2Z.Utils;

namespace Twee2Z.Analyzer.Expressions
{
    class ExpressionVisitor : ExpressionParserBaseVisitor<object>
    {
        private Expression _expression;

        public Expression Expression
        {
            get { return _expression; }
        }


        public override object VisitExpression(ExpressionParser.ExpressionContext context)
        {
            Logger.LogAnalyzer("-----------------------");
            Logger.LogAnalyzer("Expression: " + context.GetText());
            return base.VisitExpression(context);
        }

        public override object VisitExpr(ExpressionParser.ExprContext context)
        {
            if (context.ChildCount > 1)
            {
                Logger.LogAnalyzer("Variable: " + context.GetChild(0).GetText());
            }
            return base.VisitExpr(context);
        }

        public override object VisitExprR(ExpressionParser.ExprRContext context)
        {
            return base.VisitExprR(context);
        }

        public override object VisitExprROpUnary(ExpressionParser.ExprROpUnaryContext context)
        {
            return base.VisitExprROpUnary(context);
        }

        public override object VisitExprROp(ExpressionParser.ExprROpContext context)
        {
            return base.VisitExprROp(context);
        }

        public override object VisitExprRContent(ExpressionParser.ExprRContentContext context)
        {
            return base.VisitExprRContent(context);
        }

        public override object VisitExprRBracket(ExpressionParser.ExprRBracketContext context)
        {
            return base.VisitExprRBracket(context);
        }

        public override object VisitValue(ExpressionParser.ValueContext context)
        {
            Logger.LogAnalyzer("Value: " + context.GetText());
            return base.VisitValue(context);
        }

        public override object VisitOpUnary(ExpressionParser.OpUnaryContext context)
        {
            Logger.LogAnalyzer("OP Unary: " + context.GetText());
            return base.VisitOpUnary(context);
        }

        public override object VisitOp(ExpressionParser.OpContext context)
        {
            Logger.LogAnalyzer("OP: " + context.GetText());
            return base.VisitOp(context);
        }

        public override object VisitAssign(ExpressionParser.AssignContext context)
        {
            Logger.LogAnalyzer("Assign: " + context.GetText());
            return base.VisitAssign(context);
        }

        public override object VisitFunction(ExpressionParser.FunctionContext context)
        {
            Logger.LogAnalyzer("Function: " + context.GetText());
            return base.VisitFunction(context);
        }

        public override object VisitFunctionArgs(ExpressionParser.FunctionArgsContext context)
        {
            return base.VisitFunctionArgs(context);
        }

        public override object VisitFunctionArg(ExpressionParser.FunctionArgContext context)
        {
            Logger.LogAnalyzer("Function Arg: " + context.GetText());
            return base.VisitFunctionArg(context);
        }
    }
}
