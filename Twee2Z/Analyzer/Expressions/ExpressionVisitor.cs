using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.ObjectTree.Expressions;
using Twee2Z.ObjectTree.Expressions.Base;
using Twee2Z.ObjectTree.Expressions.Base.Values;
using Twee2Z.ObjectTree.Expressions.Base.Values.Functions;
using Twee2Z.ObjectTree.Expressions.Base.Ops;
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
            string text = context.GetText();
            Logger.LogAnalyzer("Value: " + text);
            ExpressionParser.FunctionContext function = context.function();
            if (function != null)
            {
                return VisitFunction(function);
            }
            else if (context.GetToken(ExpressionParser.DIGITS, 0) != null)
            {
                if (context.GetToken(ExpressionParser.DIGITS, 1) != null)
                {
                    // TODO float
                    return new IntValue(Convert.ToInt32(context.GetToken(ExpressionParser.DIGITS, 0).GetText()));
                }
                return new IntValue(Convert.ToInt32(text));
            }
            else if (context.GetToken(ExpressionParser.VAR_NAME, 0) != null)
            {
                return new VariableValue(text.Substring(1));
            }
            else if (context.GetToken(ExpressionParser.STRING, 0) != null)
            {
                new StringValue(text);
            }
            Logger.LogWarning("Counlt not evaluate: " + text);
            return null;
        }

        public override Expression VisitOpUnary(ExpressionParser.OpUnaryContext context)
        {
            if (context.GetToken(ExpressionParser.OP_ADD, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Add);
            }
            else if (context.GetToken(ExpressionParser.OP_SUB, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Sub);
            }
            else if (context.GetToken(ExpressionParser.OP_LOG_NOT, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Not);
            }
            Logger.LogWarning("Can not use operation " + context.GetText());
            return null;
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
            string name = context.GetChild(0).GetText();
            Logger.LogAnalyzer("Function name: " + name);

            FunctionValue function;
            switch (name.ToLower())
            {
                case "random":
                    function = new RandomFunction();
                    break;
                default:
                    Logger.LogWarning(name + " ist not supported");
                    return null;
            }


            IReadOnlyList<ExpressionParser.FunctionArgContext> args = context.functionArg();
            foreach (ExpressionParser.FunctionArgContext arg in args)
            {
                function.AddArg(VisitFunctionArg(arg));
            }
            return function;
        }

        public override Expression VisitFunctionArg(ExpressionParser.FunctionArgContext context)
        {
            return base.VisitFunctionArg(context);
        }
    }
}
