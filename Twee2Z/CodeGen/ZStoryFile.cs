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
using Twee2Z.ObjectTree.Expr;

namespace Twee2Z.CodeGen
{
    public class ZStoryFile
    {
        private ZSymbolTable _symbolTable;
        private ZMemory _zMemory;

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
                startPassage = tree.StartPassage;
                passages = tree.Passages.Where(passage => passage.Key.ToLower() != "start" &&
                                               passage.Key.ToLower() != "storyauthor" &&
                                               passage.Key.ToLower() != "storytitle").Select(entry => entry.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Start passage not found.", ex);
            }

            List<ZRoutine> routines = new List<ZRoutine>();
            
            routines.Add(new ZRoutine(new ZInstruction[] { new Call1n(new ZRoutineLabel(startPassage.Name)) }) { Label = new ZRoutineLabel("main") });
            
            try
            {
                routines.Add(ConvertPassageToRoutine(startPassage));

                foreach (Passage passage in passages)
                {
                    routines.Add(ConvertPassageToRoutine(passage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not convert passage into routine.", ex);
            }

            _zMemory.SetRoutines(routines);
        }

        private ZRoutine ConvertPassageToRoutine(Passage passage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();
            int currentLink = 0;
            List<string> links = new List<string>();

            instructions.Add(new EraseWindow(0));

            instructions.AddRange(ConvertPassageContent(passage.PassageContentList, ref currentLink, links));

            if (currentLink > 0)
            {
                instructions.Add(new NewLine());
                instructions.Add(new PrintUnicode('>') { Label = new ZLabel("read" + passage.Name) });
                instructions.Add(new ReadChar(new ZLocalVariable(0)));

                for (int i = 0; i < links.Count(); i++)
                {
                    // So this casting and converting looks aweful
                    // First get the number for this link: i + 1
                    // Then hard cast it into a string
                    // Now convert it into a char
                    // Finally hard cast it into short for the ZOperand
                    instructions.Add(new Je(new ZLocalVariable(0), (short)Convert.ToChar((i + 1).ToString()), new ZBranchLabel(links[i] + "Call")));
                }

                instructions.Add(new NewLine());
                instructions.Add(new Print("Unbekannte Eingabe!"));
                instructions.Add(new NewLine());
                instructions.Add(new Jump(new ZJumpLabel("read" + passage.Name)));

                for (int i = 0; i < links.Count(); i++)
                {
                    instructions.Add(new Call1n(new ZRoutineLabel(links[i])) { Label = new ZLabel(links[i] + "Call") });
                }
            }
            else
            {
                instructions.Add(new Print(" "));
                instructions.Add(new Quit());
            }
            
            return new ZRoutine(instructions, 1) { Label = new ZRoutineLabel(passage.Name) };
        }

        private List<ZInstruction> ConvertPassageContent(IEnumerable<PassageContent> contentList, ref int currentLink, List<string> links)
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

                    if (currentLink > 9)
                        throw new Exception("More than 9 links are not supported yet.");

                    links.Add(content.PassageLink.Target);

                    instructions.AddRange(StringToInstructions(content.PassageLink.DisplayText ?? content.PassageLink.Target));

                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
                    instructions.Add(new Print(currentLink.ToString()));
                    instructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
                }

                else if (content.Type == PassageContent.ContentType.MacroContent)
                {
                    instructions.AddRange(ConvertMacros(content, ref currentLink, links));
                }

                else
                {
                    throw new Exception("Unknown ContentType: " + content.GetType().Name);
                }
            }

            return instructions;
        }

        private List<ZInstruction> ConvertMacros(PassageContent content, ref int currentLink, List<string> links)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            PassageMacroSet setMacro = content as PassageMacroSet;
            PassageMacroBranch branchMacro = content as PassageMacroBranch;
            PassageMacroPrint printMacro = content as PassageMacroPrint;

            if (setMacro != null)
            {
                string[] splitList = setMacro.Expression.ExpressionString.Split('=');
                string name = splitList.First().Trim();
                short value = Convert.ToInt16(splitList.Last().Trim());

                _symbolTable.AddSymbol(name);
                instructions.Add(new Store(_symbolTable.GetSymbol(name), value));
            }

            else if (printMacro != null)
            {
                instructions.AddRange(StringToInstructions(printMacro.Expression.ExpressionString));
            }

            else if (branchMacro != null)
            {
                LinkedList<ZInstruction> branchInstructions = new LinkedList<ZInstruction>();

                Guid endIfGuid = Guid.NewGuid();

                //if
                PassageMacroIf ifMacro = (PassageMacroIf)branchMacro.BranchNodeList.First();
                Guid ifGuid = Guid.NewGuid();

                branchInstructions.AddLast(new Jz(0, new ZBranchLabel("if_" + ifGuid) { BranchOn = false }));
                
                // if expression is true
                foreach (var instruction in ConvertPassageContent(ifMacro.PassageContentList, ref currentLink, links))
                {
                    branchInstructions.AddLast(instruction);
                }
                branchInstructions.AddLast(new Jump(new ZJumpLabel("endIf_" + endIfGuid)));

                // if expression is false
                branchInstructions.AddLast(new Nop() { Label = new ZLabel("if_" + ifGuid) });
                
                //else if
                foreach (PassageMacroElseIf elseIfMacro in branchMacro.BranchNodeList.OfType<PassageMacroElseIf>())
                {
                    Guid elseIfGuid = Guid.NewGuid();

                    branchInstructions.AddLast(new Jz(0, new ZBranchLabel("elseIf_" + elseIfGuid) { BranchOn = false }));

                    // if expression is true
                    foreach (var instruction in ConvertPassageContent(elseIfMacro.PassageContentList, ref currentLink, links))
                    {
                        branchInstructions.AddLast(instruction);
                    }
                    branchInstructions.AddLast(new Jump(new ZJumpLabel("endIf_" + endIfGuid)));

                    // if expression is false
                    branchInstructions.AddLast(new Nop() { Label = new ZLabel("elseIf_" + elseIfGuid) });
                }

                //else
                PassageMacroElse elseMacro = branchMacro.BranchNodeList.Last() as PassageMacroElse;

                if (elseMacro != null)
                {
                    foreach (var instruction in ConvertPassageContent(elseMacro.PassageContentList, ref currentLink, links))
                    {
                        branchInstructions.AddLast(instruction);
                    }
                }

                //endif
                branchInstructions.AddLast(new Nop() { Label = new ZLabel("endIf_" + endIfGuid) });

                instructions.AddRange(branchInstructions);
            }

            else
            {
                throw new NotSupportedException("This macro is not supported yet: " + content.GetType().Name);
            }

            return instructions;
        }

        private void ConvertExpression(Expression expression, ref List<ZInstruction> instructions)
        {
            instructions.Add(new Je(_symbolTable.GetSymbol("$variable"), (byte)42, new ZBranchLabel("nop") { BranchOn = false }));
            instructions.Add(new Nop() { Label = new ZLabel("nop") });
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

        public void SetupPassageNavigationDemo(ObjectTree.Tree tree)
        {
            List<ZInstruction> _mainInstructions = new List<ZInstruction>();
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo));
            _mainInstructions.Add(new Print("Twee2Z Meilenstein 3"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _mainInstructions.Add(new Print(" (Twee2Z-Team)"));
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new Print("Du pr"));
            _mainInstructions.Add(new PrintUnicode('ä'));
            _mainInstructions.Add(new Print("sentierst den Meilenstein 3 deines "));
            _mainInstructions.Add(new PrintUnicode('Ü'));
            _mainInstructions.Add(new Print("bersetzerbau-Projekts. Die anwesenden Teilnehmer sind skeptisch. Du wei"));
            _mainInstructions.Add(new PrintUnicode('ß'));
            _mainInstructions.Add(new Print("t, dass der Compiler noch nicht 100"));
            _mainInstructions.Add(new PrintUnicode('%'));
            _mainInstructions.Add(new Print("ig stabil ist."));
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _mainInstructions.Add(new Print("Halte dich genau an die einge"));
            _mainInstructions.Add(new PrintUnicode('ü'));
            _mainInstructions.Add(new Print("bte Pr"));
            _mainInstructions.Add(new PrintUnicode('ä'));
            _mainInstructions.Add(new Print("sentation"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _mainInstructions.Add(new Print("1"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _mainInstructions.Add(new Print("Weiche vom Plan ab"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _mainInstructions.Add(new Print("2"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new PrintUnicode('>'));
            _mainInstructions.Add(new Print(" "));
            _mainInstructions.Add(new ReadChar(new ZLocalVariable(0)));
            _mainInstructions.Add(new Je(new ZLocalVariable(0), (short)'1', new ZBranchLabel("safeCall")));
            _mainInstructions.Add(new Je(new ZLocalVariable(0), (short)'2', new ZBranchLabel("unsafeCall")));
            _mainInstructions.Add(new Quit());

            _mainInstructions.Add(new Call1n(new ZRoutineLabel("safe")) { Label = new ZLabel("safeCall") });
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("unsafe")) { Label = new ZLabel("unsafeCall") });

            List<ZInstruction> _unsafeInstructions = new List<ZInstruction>();
            _unsafeInstructions.Add(new EraseWindow(0));
            _unsafeInstructions.Add(new Print("Du wirst "));
            _unsafeInstructions.Add(new PrintUnicode('ü'));
            _unsafeInstructions.Add(new Print("berm"));
            _unsafeInstructions.Add(new PrintUnicode('ü'));
            _unsafeInstructions.Add(new Print("tig und machst etwas, dass den Compiler zum Absturz bringt."));
            _unsafeInstructions.Add(new NewLine());
            _unsafeInstructions.Add(new NewLine());
            _unsafeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Bold));
            _unsafeInstructions.Add(new Print("Die Schande brennt tief in dir." + System.Environment.NewLine));
            _unsafeInstructions.Add(new Quit());

            List<ZInstruction> _safeInstructions = new List<ZInstruction>();
            _safeInstructions.Add(new EraseWindow(0));
            _safeInstructions.Add(new Print("Alles l"));
            _safeInstructions.Add(new PrintUnicode('ä'));
            _safeInstructions.Add(new Print("uft nach Plan. Die Navigation funktioniert und die Menge tobt. (Anmerkung der Autors: "));
            _safeInstructions.Add(new PrintUnicode('Ü'));
            _safeInstructions.Add(new Print("bertreibung als Stilmittel)"));
            _safeInstructions.Add(new NewLine());
            _safeInstructions.Add(new NewLine());
            _safeInstructions.Add(new Print("Auf den zweiten Blick gibt es aber diese Reaktion:"));
            _safeInstructions.Add(new NewLine());
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _safeInstructions.Add(new Print("Toll"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("1"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _safeInstructions.Add(new Print("Ok"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("2"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _safeInstructions.Add(new Print("Meh"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("3"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.None));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new PrintUnicode('>'));
            _safeInstructions.Add(new Print(" "));
            _safeInstructions.Add(new ReadChar(new ZLocalVariable(0)));
            _safeInstructions.Add(new Je(new ZLocalVariable(0), (short)'1', new ZBranchLabel("tollCall")));
            _safeInstructions.Add(new Je(new ZLocalVariable(0), (short)'2', new ZBranchLabel("okCall")));
            _safeInstructions.Add(new Je(new ZLocalVariable(0), (short)'3', new ZBranchLabel("mehCall")));

            _safeInstructions.Add(new Call1n(new ZRoutineLabel("toll")) { Label = new ZLabel("tollCall") });
            _safeInstructions.Add(new Call1n(new ZRoutineLabel("ok")) { Label = new ZLabel("okCall") });
            _safeInstructions.Add(new Call1n(new ZRoutineLabel("meh")) { Label = new ZLabel("mehCall") });

            _safeInstructions.Add(new Quit());

            List<ZInstruction> _tollInstructions = new List<ZInstruction>();
            _tollInstructions.Add(new EraseWindow(0));
            _tollInstructions.Add(new Print("Toll!"));
            _tollInstructions.Add(new NewLine());
            _tollInstructions.Add(new Quit());

            List<ZInstruction> _okInstructions = new List<ZInstruction>();
            _okInstructions.Add(new EraseWindow(0));
            _okInstructions.Add(new Print("Ok."));
            _okInstructions.Add(new NewLine());
            _okInstructions.Add(new Quit());

            List<ZInstruction> _mehInstructions = new List<ZInstruction>();
            _mehInstructions.Add(new EraseWindow(0));
            _mehInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Bold));
            _mehInstructions.Add(new Print("Meh."));
            _mehInstructions.Add(new NewLine());
            _mehInstructions.Add(new Quit());


            _zMemory.SetRoutines(new ZRoutine[] {
                new ZRoutine(_mainInstructions, 1) { Label = new ZRoutineLabel("main") },
                new ZRoutine(_unsafeInstructions, 1) { Label = new ZRoutineLabel("unsafe") },
                new ZRoutine(_safeInstructions, 1) { Label = new ZRoutineLabel("safe") },
                new ZRoutine(_tollInstructions, 0) { Label = new ZRoutineLabel("toll") },
                new ZRoutine(_okInstructions, 0) { Label = new ZRoutineLabel("ok") },
                new ZRoutine(_mehInstructions, 0) { Label = new ZRoutineLabel("meh") }
            });
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
