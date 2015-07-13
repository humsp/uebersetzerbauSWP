using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Memory;
using Twee2Z.CodeGen.Instruction;
using Twee2Z.CodeGen.Instruction.Opcode;
using Twee2Z.CodeGen.Instruction.Template;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;
using Twee2Z.ObjectTree;
using Twee2Z.ObjectTree.PassageContents;
using Twee2Z.ObjectTree.PassageContents.Macro;
using Twee2Z.ObjectTree.PassageContents.Macro.Branch;
using Twee2Z.ObjectTree.Expressions;
using Twee2Z.ObjectTree.Expressions.Base;
using Twee2Z.ObjectTree.Expressions.Base.Values;
using Twee2Z.ObjectTree.Expressions.Base.Values.Functions;
using Twee2Z.ObjectTree.Expressions.Base.Ops;

namespace Twee2Z.CodeGen
{
    public class ZStoryFile
    {
        private ZSymbolTable _symbolTable;
        private ZMemory _zMemory;
        private IEnumerable<Passage> _passages;

        public ZStoryFile()
        {
            _zMemory = new ZMemory();
        }

        public void ImportObjectTree(Tree tree)
        {
            _symbolTable = new ZSymbolTable();
            Passage startPassage = null;
            IEnumerable<Passage> passages = null;
            try
            {
                _passages = tree.Passages.Select(entry => entry.Value);
                startPassage = tree.StartPassage;
                passages = tree.Passages.Where(passage => passage.Key.ToLower() != "start" &&
                                               passage.Key.ToLower() != "storyauthor" &&
                                               passage.Key.ToLower() != "storytitle").Select(entry => entry.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Start passage not found.", ex);
            }

            string storyTitle = tree.StoryTitle.PassageContentList.First().PassageText.Text.Trim();
            string storyAuthor = tree.StoryAuthor.PassageContentList.First().PassageText.Text.Trim();


            List<ZRoutine> routines = new List<ZRoutine>();
            
            routines.Add(new ZRoutine(new ZInstruction[] {
                new Store (_symbolTable.GetSymbol("%turns%"), -1), // sets the turns counter to -1
                new Call1n(new ZRoutineLabel(startPassage.Name))
            }) { Label = new ZRoutineLabel("main") });
            
            try
            {
                routines.Add(ConvertPassageToRoutine(startPassage, storyTitle, storyAuthor));

                foreach (Passage passage in passages)
                {
                    routines.Add(ConvertPassageToRoutine(passage, storyTitle, storyAuthor));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not convert passage into routine.", ex);
            }

            _zMemory.SetRoutines(routines);
        }

        private ZRoutine ConvertPassageToRoutine(Passage passage, string storyTitle, string storyAuthor)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();
            int currentLink = 0;
            var links = new List<Tuple<string, Assign>>();

            instructions.Add(new EraseWindow(0));
            instructions.Add(new Add(_symbolTable.GetSymbol("%turns%"), 1, _symbolTable.GetSymbol("%turns%")));
            instructions.Add(new Store(_symbolTable.GetSymbol("%passage:" + passage.Name + "%"), 1));


            if (!String.IsNullOrWhiteSpace(storyTitle))
            {
                instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.Bold));

                instructions.Add(new Print(storyTitle));
                instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));

                if (!String.IsNullOrWhiteSpace(storyAuthor))
                {
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo));
                    instructions.Add(new Print(" (" + storyAuthor + ")"));
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                }
                
                instructions.Add(new NewLine());   
            }

            instructions.AddRange(ConvertPassageContent(passage.PassageContentList, ref currentLink, links, passage.Name));

            if (currentLink > 0)
            {
                instructions.Add(new NewLine());
                instructions.Add(new PrintUnicode('>') { Label = new ZLabel("read" + passage.Name) });
                instructions.Add(new ReadChar(new ZLocalVariable(0)));

                List<Guid> callGuids = new List<Guid>();

                for (int i = 0; i < links.Count(); i++)
                {
                    callGuids.Add(Guid.NewGuid());

                    char charToWrite;
                    if (i >= 0 && i < 10)
                        charToWrite = Convert.ToChar('1' + i);
                    else if (i >= 10 && i < 36)
                        charToWrite = Convert.ToChar('a' + i - 10);
                    else if (i >= 36 && i < 62)
                        charToWrite = Convert.ToChar('A' + i - 36);
                    else
                        throw new Exception();

                    instructions.Add(new Je(new ZLocalVariable(0), (short)charToWrite, new ZBranchLabel(links[i].Item1 + "Call_" + callGuids[i])));
                }

                instructions.Add(new NewLine());
                instructions.Add(new Print("Unbekannte Eingabe!"));
                instructions.Add(new NewLine());
                instructions.Add(new Jump(new ZJumpLabel("read" + passage.Name)));

                for (int i = 0; i < links.Count(); i++)
                {
                    instructions.Add(new Nop() { Label = new ZLabel(links[i].Item1 + "Call_" + callGuids[i]) });

                    if (links[i].Item2 != null)
                    {
                        instructions.AddRange(ConvertAssignExpression(links[i].Item2, passage.Name));
                    }

                    instructions.Add(new Call1n(new ZRoutineLabel(links[i].Item1)));
                }
            }
            else
            {
                instructions.Add(new Print(" "));
                instructions.Add(new Quit());
            }
            
            return new ZRoutine(instructions, 1) { Label = new ZRoutineLabel(passage.Name) };
        }

        private List<ZInstruction> ConvertPassageContent(IEnumerable<PassageContent> contentList, ref int currentLink, List<Tuple<string, Assign>> links, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            foreach (PassageContent content in contentList)
            {
                if (content.Type == PassageContent.ContentType.TextContent)
                {
                    SetTextStyle.StyleFlags flags = SetTextStyle.StyleFlags.None;

                    if (content.PassageText.ContentFormat.Bold)
                        flags |= SetTextStyle.StyleFlags.Bold;

                    if (content.PassageText.ContentFormat.Italic)
                        flags |= SetTextStyle.StyleFlags.Italic;

                    if (content.PassageText.ContentFormat.Monospace)
                        flags |= SetTextStyle.StyleFlags.FixedPitch;

                    if (flags != SetTextStyle.StyleFlags.None)
                    {
                        instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                        instructions.Add(new SetTextStyle(flags));
                    }

                    instructions.AddRange(StringToInstructions(content.PassageText.Text));

                    if (flags != SetTextStyle.StyleFlags.None)
                        instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                }

                else if (content.Type == PassageContent.ContentType.LinkContent)
                {
                    currentLink++;

                    if (currentLink > 61)
                        throw new Exception("More than 61 links are not supported yet.");

                    char charToWrite;
                    if (currentLink >= 1 && currentLink < 10)
                        charToWrite = Convert.ToChar('1' + currentLink - 1);
                    else if (currentLink >= 10 && currentLink < 36)
                        charToWrite = Convert.ToChar('a' + currentLink - 10);
                    else if (currentLink >= 36 && currentLink < 62)
                        charToWrite = Convert.ToChar('A' + currentLink - 36);
                    else
                        throw new Exception();

                    instructions.AddRange(StringToInstructions(content.PassageLink.DisplayText ?? content.PassageLink.Target));

                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
                    instructions.Add(new Print(charToWrite.ToString()));
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));

                    links.Add(new Tuple<string, Assign>(content.PassageLink.Target, (Assign)content.PassageLink.Expression));
                }

                else if (content.Type == PassageContent.ContentType.MacroContent)
                {
                    instructions.AddRange(ConvertMacros(content, ref currentLink, links, currentPassage));
                }

                else if (content.Type == PassageContent.ContentType.FunctionContent)
                {
                    instructions.AddRange(ConvertFuntions(content, ref currentLink, links));
                }

                else if (content.Type == PassageContent.ContentType.VariableContent)
                {
                    instructions.AddRange(StringToInstructions(((PassageVariable)content).Id));
                }

                else
                {
                    throw new Exception("Unknown ContentType: " + content.GetType().Name);
                }
            }

            return instructions;
        }

        private List<ZInstruction> ConvertMacros(PassageContent content, ref int currentLink, List<Tuple<string, Assign>> links, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            PassageMacroSet setMacro = content as PassageMacroSet;
            PassageMacroBranch branchMacro = content as PassageMacroBranch;
            PassageMacroPrint printMacro = content as PassageMacroPrint;
            PassageMacroDisplay displayMacro = content as PassageMacroDisplay;

            if (setMacro != null)
            {
                instructions.AddRange(ConvertAssignExpression((Assign)setMacro.Expression, currentPassage));
            }

            else if (printMacro != null)
            {
                if (printMacro.Expression is StringValue)
                    instructions.AddRange(StringToInstructions(((StringValue)(printMacro.Expression)).Value.Replace('\"', ' ').Trim()));
                else
                {
                    instructions.AddRange(ConvertValueExpression(printMacro.Expression, currentPassage));
                    instructions.Add(new PrintNum(new ZStackVariable()));
                }
            }

            else if (displayMacro != null)
            {
                instructions.AddRange(ConvertPassageContent(_passages.Single(passage =>
                    passage.Name == ((StringValue)(displayMacro.Expression)).Value.Replace('\"', ' ').Trim()).PassageContentList, ref currentLink, links, currentPassage));
            }

            else if (branchMacro != null)
            {
                Guid endIfGuid = Guid.NewGuid();
                string endIf = "endIf_" + endIfGuid;

                //if
                PassageMacroIf ifMacro = (PassageMacroIf)branchMacro.BranchNodeList.First();
                Guid ifGuid = Guid.NewGuid();
                string ifTrue = "if_True_" + ifGuid;
                string ifFalse = "if_False_" + ifGuid;

                instructions.AddRange(ConvertBranchExpression(ifMacro.Expression, ifTrue, ifFalse, currentPassage));
                
                // if expression is true
                instructions.Add(new Nop() { Label = new ZLabel(ifTrue) });
                foreach (var instruction in ConvertPassageContent(ifMacro.PassageContentList, ref currentLink, links, currentPassage))
                {
                    instructions.Add(instruction);
                }
                instructions.Add(new Jump(new ZJumpLabel(endIf)));

                // if expression is false
                instructions.Add(new Nop() { Label = new ZLabel(ifFalse) });
                
                //else if
                foreach (PassageMacroElseIf elseIfMacro in branchMacro.BranchNodeList.OfType<PassageMacroElseIf>())
                {
                    Guid elseIfGuid = Guid.NewGuid();
                    string elseIfTrue = "elseIf_True_" + elseIfGuid;
                    string elseIfFalse = "elseIf_False_" + elseIfGuid;

                    instructions.AddRange(ConvertBranchExpression(elseIfMacro.Expression, elseIfTrue, elseIfFalse, currentPassage));

                    // if expression is true
                    instructions.Add(new Nop() { Label = new ZLabel(elseIfTrue) });
                    foreach (var instruction in ConvertPassageContent(elseIfMacro.PassageContentList, ref currentLink, links, currentPassage))
                    {
                        instructions.Add(instruction);
                    }
                    instructions.Add(new Jump(new ZJumpLabel(endIf)));

                    // if expression is false
                    instructions.Add(new Nop() { Label = new ZLabel(elseIfFalse) });
                }

                //else
                PassageMacroElse elseMacro = branchMacro.BranchNodeList.Last() as PassageMacroElse;

                if (elseMacro != null)
                {
                    foreach (var instruction in ConvertPassageContent(elseMacro.PassageContentList, ref currentLink, links, currentPassage))
                    {
                        instructions.Add(instruction);
                    }
                }

                //endif
                instructions.Add(new Nop() { Label = new ZLabel(endIf) });
            }

            else
            {
                throw new NotSupportedException("This macro is not supported yet: " + content.GetType().Name);
            }

            return instructions;
        }

        private List<ZInstruction> ConvertFuntions(PassageContent content, ref int currentLink, List<Tuple<string, Assign>> links)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            PassageFunction function = content as PassageFunction;

            if (function != null)
            {

            }

            return instructions;
        }

        private List<ZInstruction> ConvertBranchExpression(Expression expression, string branchOnTrueLabel, string branchOnFalseLabel, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            // Expression has logical operand with a left and right side
            if (expression is LogicalOp)
            {
                LogicalOp log = (LogicalOp)expression;

                Guid expGuid = Guid.NewGuid();
                string expTrueLabel = "exp_True_" + expGuid;
                string expFalseLabel = "exp_False_" + expGuid;

                short? leftAsShort = null;
                ZVariable leftAsVariable = null;
                bool leftIsSubExpression = false;

                short? rightAsShort = null;
                ZVariable rightAsVariable = null;
                bool rightIsSubExpression = false;

                int subExpressionCounter = 0;

                if (log.LeftExpr is VariableValue)      // left side is a variable
                    leftAsVariable = _symbolTable.GetSymbol(((VariableValue)(log.LeftExpr.BaseValue)).Name);
                else if (log.LeftExpr is IntValue)      // left side is an integer
                    leftAsShort = Convert.ToInt16(((IntValue)(log.LeftExpr.BaseValue)).Value);
                else if (log.LeftExpr is BoolValue)     // left side is a boolean
                    leftAsShort = Convert.ToInt16(((BoolValue)(log.LeftExpr.BaseValue)).Value);
                else if (log.LeftExpr is FunctionValue) // left side is a function
                {
                    instructions.AddRange(ConvertFunctionValue((FunctionValue)log.LeftExpr, currentPassage));
                    leftAsVariable = new ZStackVariable();
                }
                else                                    // left side is in any other case a sub expression
                {
                    subExpressionCounter++;
                    leftIsSubExpression = true;

                    if (log.Type == LogicalOp.LocicalOpEnum.And)
                    {
                        // in case of AND there is no need to evaluate the right side
                        Guid andGuid = Guid.NewGuid();
                        string andBranchOnTrueLabel = "and_True_" + andGuid;
                        string andBranchOnFalseLabel = "and_False_" + andGuid;

                        instructions.AddRange(ConvertBranchExpression(log.LeftExpr, andBranchOnTrueLabel, andBranchOnFalseLabel, currentPassage));

                        //instructions.Add(new Nop() { Label = new ZLabel(andBranchOnFalseLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expFalseLabel)) { Label = new ZLabel(andBranchOnFalseLabel) });
                        instructions.Add(new Nop() { Label = new ZLabel(andBranchOnTrueLabel) });
                    }
                    else if (log.Type == LogicalOp.LocicalOpEnum.Or)
                    {
                        // if OR is used we can stop evalutating on true; on false we have to continue with the left side
                        Guid orGuid = Guid.NewGuid();
                        string orBranchOnTrueLabel = "or_True_" + orGuid;
                        string orBranchOnFalseLabel = "or_False_" + orGuid;

                        instructions.AddRange(ConvertBranchExpression(log.LeftExpr, orBranchOnTrueLabel, orBranchOnFalseLabel, currentPassage));

                        //instructions.Add(new Nop() { Label = new ZLabel(orBranchOnTrueLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)) { Label = new ZLabel(orBranchOnTrueLabel) });
                        instructions.Add(new Nop() { Label = new ZLabel(orBranchOnFalseLabel) });
                    }
                    else if (log.Type == LogicalOp.LocicalOpEnum.Xor)
                    {
                        // XOR behaves like OR on the left side
                        string xorBranchOnAnyLabel = "xor_Any_" + Guid.NewGuid();

                        instructions.AddRange(ConvertBranchExpression(log.LeftExpr, xorBranchOnAnyLabel, xorBranchOnAnyLabel, currentPassage));

                        instructions.Add(new Nop() { Label = new ZLabel(xorBranchOnAnyLabel) });
                    }
                    else
                    {
                        // logical operands are for sub expressions supported only
                        throw new InvalidOperationException("Only logical operands like AND, OR and XOR are supported on sub expressions. Given opreand: " + log.Type.ToString());
                    }
                }

                if (log.RightExpr is VariableValue)
                    rightAsVariable = _symbolTable.GetSymbol(((VariableValue)(log.RightExpr.BaseValue)).Name);
                else if (log.RightExpr is IntValue)
                    rightAsShort = Convert.ToInt16(((IntValue)(log.RightExpr.BaseValue)).Value);
                else if (log.RightExpr is BoolValue)
                    rightAsShort = Convert.ToInt16(((BoolValue)(log.RightExpr.BaseValue)).Value);
                else if (log.RightExpr is FunctionValue)
                {
                    instructions.AddRange(ConvertFunctionValue((FunctionValue)log.RightExpr, currentPassage));
                    rightAsVariable = new ZStackVariable();
                }
                else
                {
                    subExpressionCounter++;
                    rightIsSubExpression = true;

                    if (log.Type == LogicalOp.LocicalOpEnum.And)
                    {
                        // due to the AND operand we have to evaluate the right side as well
                        Guid andGuid = Guid.NewGuid();
                        string andBranchOnTrueLabel = "and_True_" + andGuid;
                        string andBranchOnFalseLabel = "and_False_" + andGuid;

                        instructions.AddRange(ConvertBranchExpression(log.RightExpr, andBranchOnTrueLabel, andBranchOnFalseLabel, currentPassage));

                        //instructions.Add(new Nop() { Label = new ZLabel(andBranchOnFalseLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expFalseLabel)) { Label = new ZLabel(andBranchOnFalseLabel) });
                        //instructions.Add(new Nop() { Label = new ZLabel(andBranchOnTrueLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)) { Label = new ZLabel(andBranchOnTrueLabel) });
                    }
                    else if (log.Type == LogicalOp.LocicalOpEnum.Or)
                    {
                        // we would have gotten here only if the left side was false
                        // and due to the OR we still have to evaluate the right side
                        Guid orGuid = Guid.NewGuid();
                        string orBranchOnTrueLabel = "or_True_" + orGuid;
                        string orBranchOnFalseLabel = "or_False_" + orGuid;

                        instructions.AddRange(ConvertBranchExpression(log.RightExpr, orBranchOnTrueLabel, orBranchOnFalseLabel, currentPassage));

                        //instructions.Add(new Nop() { Label = new ZLabel(orBranchOnFalseLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expFalseLabel)) { Label = new ZLabel(orBranchOnFalseLabel) });
                        //instructions.Add(new Nop() { Label = new ZLabel(orBranchOnTrueLabel) });
                        instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)) { Label = new ZLabel(orBranchOnTrueLabel) });
                    }
                    else if (log.Type == LogicalOp.LocicalOpEnum.Xor)
                    {
                        // for XOR we have to know the result of the left side
                        // thankfully we pushed it onto the stack (0 false; 1 true)
                        string xorBranchOnAnyLabel = "xor_Any_" + Guid.NewGuid();

                        instructions.AddRange(ConvertBranchExpression(log.RightExpr, xorBranchOnAnyLabel, xorBranchOnAnyLabel, currentPassage));

                        // if both sides were true or false then branch to false label
                        instructions.Add(new Je(new ZStackVariable(), new ZStackVariable(), new ZBranchLabel(expFalseLabel)));
                        instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                    }
                    else
                    {
                        // logical operands are for sub expressions supported only
                        throw new InvalidOperationException("Only logical operands like AND, OR and XOR are supported on sub expressions. Given opreand: " + log.Type.ToString());
                    }
                }

                if (subExpressionCounter < 2) // if both sides are sub expressions then the following is not needed
                {
                    switch (log.Type)
                    {
                        case LogicalOp.LocicalOpEnum.And:
                            if (leftAsShort != null && rightAsShort != null)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsShort != null && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsShort != null && rightIsSubExpression)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightAsShort != null)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightIsSubExpression)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftIsSubExpression && rightAsShort != null)
                            {
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftIsSubExpression && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else
                                throw new Exception("Something went wrong while converting an expression with " + LogicalOp.LocicalOpEnum.And.ToString() + " operand.");

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Or:
                            if (leftAsShort != null && rightAsShort != null)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsShort != null && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsShort != null && rightIsSubExpression)
                            {
                                instructions.Add(new Jg(leftAsShort.Value, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightAsShort != null)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftAsVariable != null && rightIsSubExpression)
                            {
                                instructions.Add(new Jg(leftAsVariable, 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftIsSubExpression && rightAsShort != null)
                            {
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsShort.Value, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else if (leftIsSubExpression && rightAsVariable != null)
                            {
                                instructions.Add(new Jg(new ZStackVariable(), 0, new ZBranchLabel(expTrueLabel) { BranchOn = true }));
                                instructions.Add(new Jg(rightAsVariable, 0, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            }
                            else
                                throw new Exception("Something went wrong while converting an expression with " + LogicalOp.LocicalOpEnum.Or.ToString() + " operand.");

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Xor:
                            throw new NotImplementedException("The operand " + LogicalOp.LocicalOpEnum.Xor.ToString() + " is not implemented yet.");

                        case LogicalOp.LocicalOpEnum.Not:
                            throw new NotImplementedException("The operand " + LogicalOp.LocicalOpEnum.Not.ToString() + " is not implemented yet.");

                        case LogicalOp.LocicalOpEnum.Neq:
                            if (leftIsSubExpression || rightIsSubExpression)
                                throw new InvalidOperationException("Arithmetic operands are not supported on sub expressions. Given operand: " + LogicalOp.LocicalOpEnum.Neq.ToString());

                            if (leftAsShort != null && rightAsShort != null)
                                instructions.Add(new Je(leftAsShort.Value, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = true }));
                            else if (leftAsShort != null && rightAsVariable != null)
                                instructions.Add(new Je(leftAsShort.Value, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = true }));
                            else if (leftAsVariable != null && rightAsShort != null)
                                instructions.Add(new Je(leftAsVariable, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = true }));
                            else if (leftAsVariable != null && rightAsVariable != null)
                                instructions.Add(new Je(leftAsVariable, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = true }));

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Eq:
                            if (leftIsSubExpression || rightIsSubExpression)
                                throw new InvalidOperationException("Arithmetic operands are not supported on sub expressions. Given operand: " + LogicalOp.LocicalOpEnum.Eq.ToString());

                            if (leftAsShort != null && rightAsShort != null)
                                instructions.Add(new Je(leftAsShort.Value, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsShort != null && rightAsVariable != null)
                                instructions.Add(new Je(leftAsShort.Value, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsShort != null)
                                instructions.Add(new Je(leftAsVariable, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsVariable != null)
                                instructions.Add(new Je(leftAsVariable, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Gt:
                            if (leftIsSubExpression || rightIsSubExpression)
                                throw new InvalidOperationException("Arithmetic operands are not supported on sub expressions. Given operand: " + LogicalOp.LocicalOpEnum.Gt.ToString());

                            if (leftAsShort != null && rightAsShort != null)
                                instructions.Add(new Jg(leftAsShort.Value, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsShort != null && rightAsVariable != null)
                                instructions.Add(new Jg(leftAsShort.Value, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsShort != null)
                                instructions.Add(new Jg(leftAsVariable, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsVariable != null)
                                instructions.Add(new Jg(leftAsVariable, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Ge:
                            throw new NotImplementedException("The operand " + LogicalOp.LocicalOpEnum.Ge.ToString() + " is not implemented yet.");

                        case LogicalOp.LocicalOpEnum.Lt:
                            if (leftIsSubExpression || rightIsSubExpression)
                                throw new InvalidOperationException("Arithmetic operands are not supported on sub expressions. Given operand: " + LogicalOp.LocicalOpEnum.Lt.ToString());

                            if (leftAsShort != null && rightAsShort != null)
                                instructions.Add(new Jl(leftAsShort.Value, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsShort != null && rightAsVariable != null)
                                instructions.Add(new Jl(leftAsShort.Value, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsShort != null)
                                instructions.Add(new Jl(leftAsVariable, rightAsShort.Value, new ZBranchLabel(expFalseLabel) { BranchOn = false }));
                            else if (leftAsVariable != null && rightAsVariable != null)
                                instructions.Add(new Jl(leftAsVariable, rightAsVariable, new ZBranchLabel(expFalseLabel) { BranchOn = false }));

                            instructions.Add(new Jump(new ZJumpLabel(expTrueLabel)));
                            break;

                        case LogicalOp.LocicalOpEnum.Le:
                            throw new NotImplementedException("The operand " + LogicalOp.LocicalOpEnum.Le.ToString() + " is not implemented yet.");

                        default:
                            throw new Exception("Unknown operand used: " + log.Type.ToString());
                    }
                }

                // the expression above was true
                instructions.Add(new Push(1) { Label = new ZLabel(expTrueLabel) });
                instructions.Add(new Jump(new ZJumpLabel(branchOnTrueLabel)));
                // the expression above was false
                instructions.Add(new Push(0) { Label = new ZLabel(expFalseLabel) });
                instructions.Add(new Jump(new ZJumpLabel(branchOnFalseLabel)));
            }

            else if (expression is BoolValue)
            {
                BoolValue boolean = (BoolValue)expression;

                instructions.Add(new Je(1, Convert.ToInt16(boolean.Value), new ZBranchLabel(branchOnFalseLabel) { BranchOn = false }));
            }

            else if (expression is VariableValue)
            {
                instructions.Add(new Je(1, _symbolTable.GetSymbol(((VariableValue)expression).Name), new ZBranchLabel(branchOnFalseLabel) { BranchOn = false }));
            }

            else if (expression is FunctionValue)
            {
                instructions.AddRange(ConvertFunctionValue((FunctionValue)expression, currentPassage));
            }

            else
                throw new Exception("Unknown expression type " + expression.GetType().Name + " for converting branches.");

            return instructions;
        }

        private List<ZInstruction> ConvertFunctionValue(FunctionValue function, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            TurnsFunction turns = function as TurnsFunction;
            VisitedFunction visited = function as VisitedFunction;
            ConfirmFunction confirm = function as ConfirmFunction;
            RandomFunction random = function as RandomFunction;

            if (turns != null)
            {
                instructions.Add(new Push(_symbolTable.GetSymbol("%turns%")));
            }
            else if (visited != null)
            {
                instructions.Add(new Push(_symbolTable.GetSymbol("%passage:" + currentPassage + "%")));
            }
            else if (confirm != null)
            {
                string text = ((StringValue)confirm.Args.First()).Value;

                instructions.AddRange(StringToInstructions(text));
                instructions.Add(new Print("Yes No"));
                instructions.Add(new ReadChar(new ZStackVariable()));
                instructions.Add(new Je(new ZStackVariable(), (short)'y', new ZBranchLabel("yes")));
                instructions.Add(new Push(0));
                instructions.Add(new Push(1) { Label = new ZLabel("yes") });
            }
            else if (random != null)
            {
                short min = (short)((IntValue)random.Args[0].BaseExpression).Value;
                short max = (short)((IntValue)random.Args[1].BaseExpression).Value;

                if (min < 1)
                {
                    instructions.Add(new Twee2Z.CodeGen.Instruction.Template.Random((short)(max + (-1 * min) + 1), new ZStackVariable()));
                    instructions.Add(new Sub(new ZStackVariable(), (short)((-1 * min) + 1), new ZStackVariable()));
                }
                else if (min > 1)
                {
                    instructions.Add(new Twee2Z.CodeGen.Instruction.Template.Random((short)(max + (-1 * min) + 1), new ZStackVariable()));
                    instructions.Add(new Sub(new ZStackVariable(), (short)(min - 1), new ZStackVariable()));
                }
                else
                    instructions.Add(new Twee2Z.CodeGen.Instruction.Template.Random(max, new ZStackVariable()));
            }
            else
                throw new NotImplementedException("The given function " + function.GetType().Name + " is not implemented yet.");

            return instructions;
        }

        private List<ZInstruction> ConvertAssignExpression(Assign assign, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            string name = assign.Variable.Name;
            _symbolTable.AddSymbol(name);

            instructions.AddRange(ConvertValueExpression(assign.Expr, currentPassage));

            switch (assign.AssignType)
            {
                case Assign.AssignTypeEnum.AssignEq:
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                case Assign.AssignTypeEnum.AssignAdd:
                    instructions.Add(new Add(_symbolTable.GetSymbol(name), new ZStackVariable(), new ZStackVariable()));
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                case Assign.AssignTypeEnum.AssignSub:
                    instructions.Add(new Sub(_symbolTable.GetSymbol(name), new ZStackVariable(), new ZStackVariable()));
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                case Assign.AssignTypeEnum.AssignMul:
                    instructions.Add(new Mul(_symbolTable.GetSymbol(name), new ZStackVariable(), new ZStackVariable()));
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                case Assign.AssignTypeEnum.AssignDiv:
                    instructions.Add(new Div(_symbolTable.GetSymbol(name), new ZStackVariable(), new ZStackVariable()));
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                case Assign.AssignTypeEnum.AssignMod:
                    instructions.Add(new Mod(_symbolTable.GetSymbol(name), new ZStackVariable(), new ZStackVariable()));
                    instructions.Add(new Store(_symbolTable.GetSymbol(name), new ZStackVariable()));
                    break;

                default:
                    throw new NotImplementedException();
            }


            return instructions;
        }

        private List<ZInstruction> ConvertValueExpression(Expression expression, string currentPassage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            IntValue intValue = expression as IntValue;
            BoolValue boolValue = expression as BoolValue;
            VariableValue variableValue = expression as VariableValue;
            FunctionValue functionValue = expression as FunctionValue;
            NormalOp operandValue = expression as NormalOp;

            if (intValue != null)
            {
                short value = Convert.ToInt16((short)intValue.Value);
                instructions.Add(new Push(value));
            }
            else if (boolValue != null)
            {
                short value = boolValue.Value ? (short)1 : (short)0;
                instructions.Add(new Push(value));
            }
            else if (variableValue != null)
            {
                instructions.Add(new Push(_symbolTable.GetSymbol(variableValue.Name)));
            }
            else if (functionValue != null)
            {
                instructions.AddRange(ConvertFunctionValue(functionValue, currentPassage));
            }
            else if (operandValue != null)
            {
                short? leftAsShort = null;
                ZVariable leftAsVariable = null;
                bool leftIsSubExpression = false;

                short? rightAsShort = null;
                ZVariable rightAsVariable = null;
                bool rightIsSubExpression = false;

                if (operandValue.RightExpr is VariableValue)
                    rightAsVariable = _symbolTable.GetSymbol(((VariableValue)(operandValue.RightExpr.BaseValue)).Name);
                else if (operandValue.RightExpr is IntValue)
                    rightAsShort = Convert.ToInt16(((IntValue)(operandValue.RightExpr.BaseValue)).Value);
                else if (operandValue.RightExpr is BoolValue)
                    rightAsShort = Convert.ToInt16(((BoolValue)(operandValue.RightExpr.BaseValue)).Value);
                else if (operandValue.RightExpr is FunctionValue)
                {
                    instructions.AddRange(ConvertFunctionValue((FunctionValue)operandValue.RightExpr, currentPassage));
                    rightAsVariable = new ZStackVariable();
                }
                else
                {
                    rightIsSubExpression = true;
                    instructions.AddRange(ConvertValueExpression(operandValue.RightExpr, currentPassage));
                }

                if (operandValue.LeftExpr is VariableValue)
                    leftAsVariable = _symbolTable.GetSymbol(((VariableValue)(operandValue.LeftExpr.BaseValue)).Name);
                else if (operandValue.LeftExpr is IntValue)
                    leftAsShort = Convert.ToInt16(((IntValue)(operandValue.LeftExpr.BaseValue)).Value);
                else if (operandValue.LeftExpr is BoolValue)
                    leftAsShort = Convert.ToInt16(((BoolValue)(operandValue.LeftExpr.BaseValue)).Value);
                else if (operandValue.LeftExpr is FunctionValue)
                {
                    instructions.AddRange(ConvertFunctionValue((FunctionValue)operandValue.LeftExpr, currentPassage));
                    leftAsVariable = new ZStackVariable();
                }
                else
                {
                    leftIsSubExpression = true;
                    instructions.AddRange(ConvertValueExpression(operandValue.LeftExpr, currentPassage));
                }

                switch (operandValue.Type)
                {
                    case NormalOp.NormalOpEnum.Add:
                        if (leftAsShort != null && rightAsShort != null)
                            instructions.Add(new Add(leftAsShort.Value, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsShort != null && rightAsVariable != null)
                            instructions.Add(new Add(leftAsShort.Value, rightAsVariable, new ZStackVariable()));
                        else if (leftAsShort != null && rightIsSubExpression != null)
                            instructions.Add(new Add(leftAsShort.Value, new ZStackVariable(), new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsShort != null)
                            instructions.Add(new Add(leftAsVariable, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsVariable != null)
                            instructions.Add(new Add(leftAsVariable, rightAsVariable, new ZStackVariable()));
                        else if (leftAsVariable != null && rightIsSubExpression != null)
                            instructions.Add(new Add(leftAsVariable, new ZStackVariable(), new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsShort != null)
                            instructions.Add(new Add(new ZStackVariable(), rightAsShort.Value, new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsVariable != null)
                            instructions.Add(new Add(new ZStackVariable(), rightAsVariable, new ZStackVariable()));
                        else if (leftIsSubExpression && rightIsSubExpression)
                            instructions.Add(new Add(new ZStackVariable(), new ZStackVariable(), new ZStackVariable()));
                        break;

                    case NormalOp.NormalOpEnum.Sub:
                        if (leftAsShort != null && rightAsShort != null)
                            instructions.Add(new Sub(leftAsShort.Value, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsShort != null && rightAsVariable != null)
                            instructions.Add(new Sub(leftAsShort.Value, rightAsVariable, new ZStackVariable()));
                        else if (leftAsShort != null && rightIsSubExpression != null)
                            instructions.Add(new Sub(leftAsShort.Value, new ZStackVariable(), new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsShort != null)
                            instructions.Add(new Sub(leftAsVariable, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsVariable != null)
                            instructions.Add(new Sub(leftAsVariable, rightAsVariable, new ZStackVariable()));
                        else if (leftAsVariable != null && rightIsSubExpression != null)
                            instructions.Add(new Sub(leftAsVariable, new ZStackVariable(), new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsShort != null)
                            instructions.Add(new Sub(new ZStackVariable(), rightAsShort.Value, new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsVariable != null)
                            instructions.Add(new Sub(new ZStackVariable(), rightAsVariable, new ZStackVariable()));
                        else if (leftIsSubExpression && rightIsSubExpression)
                            instructions.Add(new Sub(new ZStackVariable(), new ZStackVariable(), new ZStackVariable()));
                        break;

                    case NormalOp.NormalOpEnum.Mul:
                        if (leftAsShort != null && rightAsShort != null)
                            instructions.Add(new Mul(leftAsShort.Value, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsShort != null && rightAsVariable != null)
                            instructions.Add(new Mul(leftAsShort.Value, rightAsVariable, new ZStackVariable()));
                        else if (leftAsShort != null && rightIsSubExpression != null)
                            instructions.Add(new Mul(leftAsShort.Value, new ZStackVariable(), new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsShort != null)
                            instructions.Add(new Mul(leftAsVariable, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsVariable != null)
                            instructions.Add(new Mul(leftAsVariable, rightAsVariable, new ZStackVariable()));
                        else if (leftAsVariable != null && rightIsSubExpression != null)
                            instructions.Add(new Mul(leftAsVariable, new ZStackVariable(), new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsShort != null)
                            instructions.Add(new Mul(new ZStackVariable(), rightAsShort.Value, new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsVariable != null)
                            instructions.Add(new Mul(new ZStackVariable(), rightAsVariable, new ZStackVariable()));
                        else if (leftIsSubExpression && rightIsSubExpression)
                            instructions.Add(new Mul(new ZStackVariable(), new ZStackVariable(), new ZStackVariable()));
                        break;

                    case NormalOp.NormalOpEnum.Div:
                        if (leftAsShort != null && rightAsShort != null)
                            instructions.Add(new Div(leftAsShort.Value, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsShort != null && rightAsVariable != null)
                            instructions.Add(new Div(leftAsShort.Value, rightAsVariable, new ZStackVariable()));
                        else if (leftAsShort != null && rightIsSubExpression != null)
                            instructions.Add(new Div(leftAsShort.Value, new ZStackVariable(), new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsShort != null)
                            instructions.Add(new Div(leftAsVariable, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsVariable != null)
                            instructions.Add(new Div(leftAsVariable, rightAsVariable, new ZStackVariable()));
                        else if (leftAsVariable != null && rightIsSubExpression != null)
                            instructions.Add(new Div(leftAsVariable, new ZStackVariable(), new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsShort != null)
                            instructions.Add(new Div(new ZStackVariable(), rightAsShort.Value, new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsVariable != null)
                            instructions.Add(new Div(new ZStackVariable(), rightAsVariable, new ZStackVariable()));
                        else if (leftIsSubExpression && rightIsSubExpression)
                            instructions.Add(new Div(new ZStackVariable(), new ZStackVariable(), new ZStackVariable()));
                        break;

                    case NormalOp.NormalOpEnum.Mod:
                        if (leftAsShort != null && rightAsShort != null)
                            instructions.Add(new Mod(leftAsShort.Value, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsShort != null && rightAsVariable != null)
                            instructions.Add(new Mod(leftAsShort.Value, rightAsVariable, new ZStackVariable()));
                        else if (leftAsShort != null && rightIsSubExpression != null)
                            instructions.Add(new Mod(leftAsShort.Value, new ZStackVariable(), new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsShort != null)
                            instructions.Add(new Mod(leftAsVariable, rightAsShort.Value, new ZStackVariable()));
                        else if (leftAsVariable != null && rightAsVariable != null)
                            instructions.Add(new Mod(leftAsVariable, rightAsVariable, new ZStackVariable()));
                        else if (leftAsVariable != null && rightIsSubExpression != null)
                            instructions.Add(new Mod(leftAsVariable, new ZStackVariable(), new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsShort != null)
                            instructions.Add(new Mod(new ZStackVariable(), rightAsShort.Value, new ZStackVariable()));
                        else if (leftIsSubExpression && rightAsVariable != null)
                            instructions.Add(new Mod(new ZStackVariable(), rightAsVariable, new ZStackVariable()));
                        else if (leftIsSubExpression && rightIsSubExpression)
                            instructions.Add(new Mod(new ZStackVariable(), new ZStackVariable(), new ZStackVariable()));
                        break;

                    default:
                        throw new Exception("Unknown operand: " + operandValue.Type.ToString());
                }
            }
            else
                throw new NotImplementedException("The given expression " + expression.GetType().Name + " for assign is not implemented yet.");

            return instructions;
        }

        private IEnumerable<ZInstruction> StringToInstructions(string input)
        {
            List<ZInstruction> list = new List<ZInstruction>();
            StringBuilder splitInput = new StringBuilder();
            foreach (char c in input)
            {
                if (Text.TextHelper.IsZSCII(c) || c == '\r' || c == '\n')
                    splitInput.Append(c);
                else
                {
                    if (splitInput.Length > 0)
                        list.Add(new Print(splitInput.ToString()));
                    list.Add(new PrintUnicode(c));
                    splitInput.Clear();
                }
            }
            if (splitInput.Length > 0)
                list.Add(new Print(splitInput.ToString()));

            return list;
        }
        
        public Byte[] ToBytes()
        {
            List<byte> byteList = new List<byte>();

            byteList.AddRange(_zMemory.ToBytes());

            return byteList.ToArray();
        }

        public int Size
        {
            get
            {
                return _zMemory.Size;
            }
        }
    }
}
