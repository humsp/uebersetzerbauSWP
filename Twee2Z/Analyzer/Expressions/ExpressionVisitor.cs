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
            return VisitS0(context.s0());
        }


        public BaseExpression VisitS0(ExpressionParser.S0Context context)
        {
            ExpressionParser.S0Context s0 = context.s0();
            ExpressionParser.S1Context s1 = context.s1();

            if (s0 != null)
            {
                Assign assign = VisitAssign(context.assign());
                BaseExpression exprS0 = VisitS0(s0);

                var nameToken = context.GetToken(ExpressionParser.VAR_NAME, 0);
                Logger.LogAnalyzer("Variable: " + context.GetChild(0).GetText());
                VariableValue variable = new VariableValue(nameToken.GetText().Substring(1));

                assign.Variable = variable;
                assign.Expr = exprS0;
                return assign;
            }
            else if (s1 != null)
            {
                return VisitS1(s1);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

 

        public BaseExpression VisitS1(ExpressionParser.S1Context context)
        {
            ExpressionParser.S1Context s1 = context.s1();
            ExpressionParser.S2Context s2 = context.s2();

            if (s1 != null && s2 != null)
            {
                BaseExpression exprS2 = VisitS2(s2);
                BaseExpression exprS1 = VisitS1(s1);
                BaseOp op = VisitOpAnd(context.opAnd());

                op.LeftExpr = exprS2;
                op.RightExpr = exprS1;
                return op;
            }
            else if (s2 != null)
            {
                return VisitS2(s2);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS2(ExpressionParser.S2Context context)
        {
            ExpressionParser.S2Context s2 = context.s2();
            ExpressionParser.S3Context s3 = context.s3();

            if (s2 != null && s3 != null)
            {
                BaseExpression exprS3 = VisitS3(s3);
                BaseExpression exprS2 = VisitS2(s2);
                BaseOp op = VisitOpOr(context.opOr());

                op.LeftExpr = exprS3;
                op.RightExpr = exprS2;
                return op;
            }
            else if (s3 != null)
            {
                return VisitS3(s3);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS3(ExpressionParser.S3Context context)
        {
            ExpressionParser.S3Context s3 = context.s3();
            ExpressionParser.S4Context s4 = context.s4();

            if (s3 != null && s4 != null)
            {
                BaseExpression exprS4 = VisitS4(s4);
                BaseExpression exprS3 = VisitS3(s3);
                BaseOp op = VisitOpXor(context.opXor());

                op.LeftExpr = exprS4;
                op.RightExpr = exprS3;
                return op;
            }
            else if (s4 != null)
            {
                return VisitS4(s4);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS4(ExpressionParser.S4Context context)
        {
            ExpressionParser.S4Context s4 = context.s4();
            ExpressionParser.S5Context s5 = context.s5();

            if (s4 != null && s5 != null)
            {
                BaseExpression exprS5 = VisitS5(s5);
                BaseExpression exprS4 = VisitS4(s4);
                BaseOp op = VisitOpCompare(context.opCompare());

                op.LeftExpr = exprS5;
                op.RightExpr = exprS4;
                return op;
            }
            else if (s5 != null)
            {
                return VisitS5(s5);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS5(ExpressionParser.S5Context context)
        {
            ExpressionParser.S5Context s5 = context.s5();
            ExpressionParser.S6Context s6 = context.s6();

            if (s5 != null && s6 != null)
            {
                BaseExpression exprS6 = VisitS6(s6);
                BaseExpression exprS5 = VisitS5(s5);
                BaseOp op = VisitOpMod(context.opMod());

                op.LeftExpr = exprS6;
                op.RightExpr = exprS5;
                return op;
            }
            else if (s6 != null)
            {
                return VisitS6(s6);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS6(ExpressionParser.S6Context context)
        {
            ExpressionParser.S6Context s6 = context.s6();
            ExpressionParser.S7Context s7 = context.s7();

            if (s6 != null && s7 != null)
            {
                BaseExpression exprS7 = VisitS7(s7);
                BaseExpression exprS6 = VisitS6(s6);
                BaseOp op = VisitOpPrio6(context.opPrio6());
                
                op.LeftExpr = exprS7;
                op.RightExpr = exprS6;
                return op;
            }
            else if (s7 != null)
            {
                return VisitS7(s7);
            }

            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS7(ExpressionParser.S7Context context)
        {
            ExpressionParser.S7Context s7 = context.s7();
            ExpressionParser.S8Context s8 = context.s8();

            if (s7 != null && s8 != null)
            {
                BaseExpression exprS8 = VisitS8(s8);
                BaseExpression exprS7 = VisitS7(s7);
                BaseOp op = VisitOpPrio7(context.opPrio7());

                op.LeftExpr = exprS8;
                op.RightExpr = exprS7;
                return op;
            }
            else if(s8 != null){
                return VisitS8(s8);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS8(ExpressionParser.S8Context context)
        {
            ExpressionParser.S8Context s8 = context.s8();
            ExpressionParser.S9Context s9 = context.s9();
            if (s8 != null)
            {
                BaseOp op = VisitOpPrio8Not(context.opPrio8Not());
                BaseExpression s8Expr = VisitS8(s8);
                op.RightExpr = s8Expr;
                return op;
                    
            }
            else if(context.s9() != null){
                return VisitS9(context.s9());
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseExpression VisitS9(ExpressionParser.S9Context context)
        {
            if (context.value() != null)
            {
                return VisitValue(context.value());
            }
            else if(context.s0() != null){
                return VisitS0(context.s0());
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }


        public BaseOp VisitOpCompare(ExpressionParser.OpCompareContext context)
        {
            if (context.GetToken(ExpressionParser.NEQ, 0) != null)
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
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpAnd(ExpressionParser.OpAndContext context)
        {
            if (context.GetToken(ExpressionParser.OP_LOG_AND, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.And);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpOr(ExpressionParser.OpOrContext context)
        {
            if (context.GetToken(ExpressionParser.OP_LOG_OR, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Or);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpXor(ExpressionParser.OpXorContext context)
        {
            if (context.GetToken(ExpressionParser.OP_LOG_XOR, 0) != null)
            {
                return new LogicalOp(LogicalOp.LocicalOpEnum.Xor);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpMod(ExpressionParser.OpModContext context)
        {
            if (context.GetToken(ExpressionParser.OP_MOD, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Mod);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpPrio6(ExpressionParser.OpPrio6Context context)
        {
            if (context.GetToken(ExpressionParser.OP_MUL, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Mul);
            }
            else if (context.GetToken(ExpressionParser.OP_DIV, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Div);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpPrio7(ExpressionParser.OpPrio7Context context)
        {
            if (context.GetToken(ExpressionParser.OP_ADD, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Add);
            }
            else if (context.GetToken(ExpressionParser.OP_SUB, 0) != null)
            {
                return new NormalOp(NormalOp.NormalOpEnum.Sub);
            }
            Logger.LogWarning("Can not parse " + context.GetText());
            return null;
        }

        public BaseOp VisitOpPrio8Not(ExpressionParser.OpPrio8NotContext context)
        {
            return new LogicalOp(LogicalOp.LocicalOpEnum.Not);
        }

       

        /*
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
        }*/

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

        /*
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
        }*/

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
            else if (context.GetToken(ExpressionParser.FCN_TURNS, 0) != null)
            {
                return new TurnsFunction();
            }
            else if (context.GetToken(ExpressionParser.FCN_PREVIOUS, 0) != null)
            {
                return new PreviousFunction();
            }
            else if (context.GetToken(ExpressionParser.FCN_VISITED, 0) != null)
            {
                return new VisitedFunction();
            }
            else if (context.GetToken(ExpressionParser.FCN_CONFIRM, 0) != null)
            {
                return new ConfirmFunction();
            }
            Logger.LogWarning(context.GetText() + " ist not supported");

            return null;
        }

        public BaseExpression VisitFunctionArg(ExpressionParser.FunctionArgContext context)
        {
            return VisitS0(context.s0());
        }
    }
}
