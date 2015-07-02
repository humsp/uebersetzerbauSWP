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

        public override Expression VisitExpression(ExpressionParser.ExpressionContext context)
        {
            Logger.LogAnalyzer("Expression: " + context.GetText());
            return VisitExpr(context.expr());
        }

        public BaseExpression VisitExpr(ExpressionParser.ExprContext context)
        {
            ExpressionParser.ExprRContentContext exprRContentContext = context.exprRContent();

            ExpressionParser.ExprROpUnaryContext exprROpUnaryContext = context.exprROpUnary();

            var nameToken = context.GetToken(ExpressionParser.VAR_NAME, 0);
            ExpressionParser.AssignContext assignContext = context.assign();
            ExpressionParser.OpContext opContext = context.op();
            IEnumerable<ExpressionParser.ExprContext> exprContext = context.expr();

            if (exprRContentContext != null)
            {
                return VisitExprRContent(exprRContentContext);
            }
            else if (exprROpUnaryContext != null)
            {
                return VisitExprROpUnary(exprROpUnaryContext);
            }
            else if (nameToken != null &&
                 assignContext != null &&
                 exprContext != null)
            {
                Logger.LogAnalyzer("Variable: " + context.GetChild(0).GetText());

                VariableValue variable = new VariableValue(nameToken.GetText().Substring(1));
                Assign assign = VisitAssign(assignContext);
                BaseExpression expr = VisitExpr(exprContext.Last());

                assign.Variable = variable;
                assign.Expr = expr;
                return assign;
            }
            else if (opContext != null &&
                     exprContext != null)
            {
                Logger.LogAnalyzer("Branch: " + context.GetText());

                BaseOp op = VisitOp(opContext);
                BaseExpression leftExpr = VisitExpr(exprContext.First());
                BaseExpression rightExpr = VisitExpr(exprContext.Last());

                op.LeftExpr = leftExpr;
                op.RightExpr = rightExpr;
                return op;
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitExprR(ExpressionParser.ExprRContext context)
        {
            if (context.exprROpUnary() != null)
            {
                return VisitExprROpUnary(context.exprROpUnary());
            }
            else if (context.exprROp() != null)
            {
                return VisitExprROp(context.exprROp());
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitExprROpUnary(ExpressionParser.ExprROpUnaryContext context)
        {
            IReadOnlyList<ExpressionParser.OpUnaryContext> opUnaries = context.opUnary();
            BaseOp itarateOp = null;

            foreach (ExpressionParser.OpUnaryContext opUnary in opUnaries)
            {
                if (itarateOp == null)
                {
                    itarateOp = VisitOpUnary(opUnary);
                }
                else
                {
                    BaseOp tempOp = VisitOpUnary(opUnary);
                    itarateOp.RightExpr = tempOp;
                    itarateOp = tempOp;
                }
            }

            BaseExpression expr = VisitExprRContent(context.exprRContent());

            if (itarateOp == null)
            {
                Logger.LogWarning("Can not parse " + context.GetText());
                return null;
            }

            itarateOp.RightExpr = expr;
            return itarateOp;
        }

        public BaseOp VisitExprROp(ExpressionParser.ExprROpContext context)
        {
            BaseOp op = VisitOp(context.op());
            BaseExpression expr = VisitExprROpUnary(context.exprROpUnary());

            op.RightExpr = expr;

            return op;
        }

        public BaseExpression VisitExprRContent(ExpressionParser.ExprRContentContext context)
        {
            if (context.exprRBracket() != null)
            {
                BaseOp op = VisitExprR(context.exprR());
                BaseExpression expr = VisitExprRBracket(context.exprRBracket());
                op.LeftExpr = expr;
                return op;
            }
            else if (context.value() != null && context.exprR() != null)
            {
                BaseValue value = VisitValue(context.value());
                BaseOp op = VisitExprR(context.exprR());
                op.LeftExpr = value;
                return op;
            }
            else if (context.value() != null)
            {
                return VisitValue(context.value());
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitExprRBracket(ExpressionParser.ExprRBracketContext context)
        {
            return VisitExpr(context.expr());
        }

        public BaseValue VisitValue(ExpressionParser.ValueContext context)
        {
            string text = context.GetText();
            Logger.LogAnalyzer("Value: " + text);
            ExpressionParser.FunctionContext function = context.function();

            if (context.GetToken(ExpressionParser.TRUE, 0) != null)
            {
                return new BoolValue(true);
            }
            else if (context.GetToken(ExpressionParser.FALSE, 0) != null)
            {
                return new BoolValue(false);
            }
            else if (function != null)
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
            else if (context.GetToken(ExpressionParser.DOT, 0) != null)
            {
                // TODO float
                return new IntValue(0);
            }
            else if (context.GetToken(ExpressionParser.VAR_NAME, 0) != null)
            {
                return new VariableValue(text.Substring(1));
            }
            else if (context.GetToken(ExpressionParser.STRING, 0) != null)
            {
                return new StringValue(text);
            }
            Logger.LogWarning("Counlt not evaluate: " + text);
            return null;
        }

        public BaseOp VisitOpUnary(ExpressionParser.OpUnaryContext context)
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

        public BaseOp VisitOp(ExpressionParser.OpContext context)
        {
            Logger.LogAnalyzer("OP: " + context.GetText());

            if (context.GetToken(ExpressionParser.OP_LOG_AND, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.And);
            }
            else if (context.GetToken(ExpressionParser.OP_LOG_OR, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Or);
            }
            else if (context.GetToken(ExpressionParser.OP_LOG_XOR, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Xor);
            }
            else if (context.GetToken(ExpressionParser.NEQ, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Neq);
            }
            else if (context.GetToken(ExpressionParser.EQ, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Eq);
            }
            else if (context.GetToken(ExpressionParser.GT, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Gt);
            }
            else if (context.GetToken(ExpressionParser.GE, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Ge);
            }
            else if (context.GetToken(ExpressionParser.LT, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Lt);
            }
            else if (context.GetToken(ExpressionParser.LE, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Le);
            }
            else if (context.GetToken(ExpressionParser.OP_MUL, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Mul);
            }
            else if (context.GetToken(ExpressionParser.OP_DIV, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Div);
            }
            else if (context.GetToken(ExpressionParser.OP_MOD, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Mod);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public Assign VisitAssign(ExpressionParser.AssignContext context)
        {
            Logger.LogAnalyzer("Assign: " + context.GetText());

            if (context.GetToken(ExpressionParser.ASSIGN_EQ, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignEq);
            }
            else if (context.GetToken(ExpressionParser.ASSIGN_SUB, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignSub);
            }
            else if (context.GetToken(ExpressionParser.ASSIGN_ADD, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignAdd);
            }
            else if (context.GetToken(ExpressionParser.ASSIGN_MUL, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignMul);
            }
            else if (context.GetToken(ExpressionParser.ASSIGN_DIV, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignDiv);
            }
            else if (context.GetToken(ExpressionParser.ASSIGN_MOD, 0) != null)
            {
                return new Assign(Assign.AssignTypeEnum.AssignMod);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public FunctionValue VisitFunction(ExpressionParser.FunctionContext context)
        {
            string name = context.GetChild(0).GetText();
            Logger.LogAnalyzer("Function name: " + name);

            FunctionValue function = VisitFunctionName(context.functionName());

            IReadOnlyList<ExpressionParser.FunctionArgContext> args = context.functionArg();
            foreach (ExpressionParser.FunctionArgContext arg in args)
            {
                function.AddArg(VisitFunctionArg(arg));
            }
            return function;
        }

        public FunctionValue VisitFunctionName(ExpressionParser.FunctionNameContext context)
        {
            if (context.GetToken(ExpressionParser.FCN_RANDOM, 0) != null)
            {
                return new RandomFunction();
            }
            Logger.LogWarning(context.GetText() + " ist not supported");

            return null;
        }

        public BaseExpression VisitFunctionArg(ExpressionParser.FunctionArgContext context)
        {
            return VisitExpr(context.expr());
        }
    }
}
