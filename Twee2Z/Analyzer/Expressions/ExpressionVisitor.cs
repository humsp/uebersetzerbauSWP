using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;
using Twee2Z.ObjectTree.Expressions.Base;
using Twee2Z.ObjectTree.Expressions.Base.Values;
using Twee2Z.ObjectTree.Expressions.Base.Values.Functions;
using Twee2Z.Utils;

namespace Twee2Z.Analyzer.Expressions
{
    class ExpressionVisitor : ExpressionParserBaseVisitor<Expression>
    {
        private Expression _expression;

        public Expression Expression
        {
            get { return _expression; }
        }


        public override Expression VisitExpression(ExpressionParser.ExpressionContext context)
        {
            Logger.LogAnalyzer("-----------------------");
            Logger.LogAnalyzer("Expression: " + context.GetText());
            return base.VisitExpression(context);
        }

        public override Expression VisitExpr(ExpressionParser.ExprContext context)
        {
            if (context.ChildCount > 1)
            {
                Logger.LogAnalyzer("Variable: " + context.GetChild(0).GetText());
            }
            return base.VisitExpr(context);
        }

        public override Expression VisitExprR(ExpressionParser.ExprRContext context)
        {
            return base.VisitExprR(context);
        }

        public override Expression VisitExprROpUnary(ExpressionParser.ExprROpUnaryContext context)
        {
            return base.VisitExprROpUnary(context);
        }

        public override Expression VisitExprROp(ExpressionParser.ExprROpContext context)
        {
            return base.VisitExprROp(context);
        }

        public override Expression VisitExprRContent(ExpressionParser.ExprRContentContext context)
        {
            return base.VisitExprRContent(context);
        }

        public override Expression VisitExprRBracket(ExpressionParser.ExprRBracketContext context)
        {
            return base.VisitExprRBracket(context);
        }

        public override Expression VisitValue(ExpressionParser.ValueContext context)
        {
            Logger.LogAnalyzer("Value: " + context.GetText());
            Expression e = Visit(context.function());
            //Expression expr = context.GetChild<ExpressionParser.FunctionContext>(0).Vis;
            //if()
            //Expression e = base.VisitValue(context);
            return base.VisitValue(context);
        }

        public override Expression VisitOpUnary(ExpressionParser.OpUnaryContext context)
        {
            Logger.LogAnalyzer("OP Unary: " + context.GetText());
            return base.VisitOpUnary(context);
        }

        public override Expression VisitOp(ExpressionParser.OpContext context)
        {
            Logger.LogAnalyzer("OP: " + context.GetText());
            return base.VisitOp(context);
        }

        public override Expression VisitAssign(ExpressionParser.AssignContext context)
        {
            Logger.LogAnalyzer("Assign: " + context.GetText());
            return base.VisitAssign(context);
        }

        public override Expression VisitFunction(ExpressionParser.FunctionContext context)
        {
            Logger.LogAnalyzer("Function: " + context.GetText());
            RandomFunction f = new RandomFunction();
            return f;
            //return base.VisitFunction(context);
        }

        public override Expression VisitFunctionArgs(ExpressionParser.FunctionArgsContext context)
        {
            return base.VisitFunctionArgs(context);
        }

        public override Expression VisitFunctionArg(ExpressionParser.FunctionArgContext context)
        {
            Logger.LogAnalyzer("Function Arg: " + context.GetText());
            return base.VisitFunctionArg(context);
        }
    }
}
