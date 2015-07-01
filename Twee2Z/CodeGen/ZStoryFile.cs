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

            string storyTitle = tree.StoryTitle.PassageContentList.First().PassageText.Text;
            string storyAuthor = tree.StoryAuthor.PassageContentList.First().PassageText.Text;


            List<ZRoutine> routines = new List<ZRoutine>();
            
            routines.Add(new ZRoutine(new ZInstruction[] { new Call1n(new ZRoutineLabel(startPassage.Name)) }) { Label = new ZRoutineLabel("main") });
            
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
            var links = new List<Tuple<string, string>>();

            instructions.Add(new EraseWindow(0));

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

            instructions.AddRange(ConvertPassageContent(passage.PassageContentList, ref currentLink, links));

            if (currentLink > 0)
            {
                instructions.Add(new NewLine());
                instructions.Add(new PrintUnicode('>') { Label = new ZLabel("read" + passage.Name) });
                instructions.Add(new ReadChar(new ZLocalVariable(0)));

                List<Guid> callGuids = new List<Guid>();

                for (int i = 0; i < links.Count(); i++)
                {
                    callGuids.Add(Guid.NewGuid());

                    // So this casting and converting looks aweful
                    // First get the number for this link: i + 1
                    // Then hard cast it into a string
                    // Now convert it into a char
                    // Finally hard cast it into short for the ZOperand
                    char charToWrite;
                    if (i >= 0 && i < 10)
                        charToWrite = Convert.ToChar('1' + i);
                    else if (i >= 10 && i < 36)
                        charToWrite = Convert.ToChar('a' + i - 10);
                    else if (i >= 36 && i < 62)
                        charToWrite = Convert.ToChar('A' + i - 36);
                    else
                        throw new Exception();

                    instructions.Add(new Je(new ZLocalVariable(0), (short)charToWrite, new ZBranchLabel(links[i] + "Call_" + callGuids[i])));
                }

                instructions.Add(new NewLine());
                instructions.Add(new Print("Unbekannte Eingabe!"));
                instructions.Add(new NewLine());
                instructions.Add(new Jump(new ZJumpLabel("read" + passage.Name)));

                for (int i = 0; i < links.Count(); i++)
                {
                    if (!String.IsNullOrEmpty(links[i].Item2))
                    {
                        string[] splitList = links[i].Item2.Split('=');
                        string name = splitList.First().Trim();
                        short value = Convert.ToInt16(splitList.Last().Trim());

                        _symbolTable.AddSymbol(name);
                        instructions.Add(new Store(_symbolTable.GetSymbol(name), value));
                    }

                    instructions.Add(new Call1n(new ZRoutineLabel(links[i].Item1)) { Label = new ZLabel(links[i] + "Call_" + callGuids[i]) });
                }
            }
            else
            {
                instructions.Add(new Print(" "));
                instructions.Add(new Quit());
            }
            
            return new ZRoutine(instructions, 1) { Label = new ZRoutineLabel(passage.Name) };
        }

        private List<ZInstruction> ConvertPassageContent(IEnumerable<PassageContent> contentList, ref int currentLink, List<Tuple<string, string>> links)
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

                    links.Add(new Tuple<string, string>(content.PassageLink.Target, content.PassageLink.Expression));
                }

                else if (content.Type == PassageContent.ContentType.MacroContent)
                {
                    instructions.AddRange(ConvertMacros(content, ref currentLink, links));
                }

                else if (content.Type == PassageContent.ContentType.FunctionContent)
                {
                    instructions.AddRange(ConvertFuntions(content, ref currentLink, links));
                }

                else
                {
                    throw new Exception("Unknown ContentType: " + content.GetType().Name);
                }
            }

            return instructions;
        }

        private List<ZInstruction> ConvertMacros(PassageContent content, ref int currentLink, List<Tuple<string, string>> links)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            PassageMacroSet setMacro = content as PassageMacroSet;
            PassageMacroBranch branchMacro = content as PassageMacroBranch;
            PassageMacroPrint printMacro = content as PassageMacroPrint;
            PassageMacroDisplay displayMacro = content as PassageMacroDisplay;

            if (setMacro != null)
            {
                string[] splitList = setMacro.Expression.ToString().Split('=');
                string name = splitList.First().Trim();
                short value = Convert.ToInt16(splitList.Last().Trim());

                _symbolTable.AddSymbol(name);
                instructions.Add(new Store(_symbolTable.GetSymbol(name), value));
            }

            else if (printMacro != null)
            {
                instructions.AddRange(StringToInstructions(printMacro.Expression.ToString()));
            }

            else if (displayMacro != null)
            {
                instructions.AddRange(ConvertPassageContent(null, ref currentLink, links));
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

        private List<ZInstruction> ConvertFuntions(PassageContent content, ref int currentLink, List<Tuple<string, string>> links)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();

            PassageFunction function = content as PassageFunction;

            if (function != null)
            {

            }

            return instructions;
        }

        /*private void ConvertExpression(Expression expression, ref List<ZInstruction> instructions)
        {
            
        }*/

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
