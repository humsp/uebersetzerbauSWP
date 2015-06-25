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

namespace Twee2Z.CodeGen
{
    public class ZStoryFile
    {
        private ZMemory _zMemory;

        public ZStoryFile()
        {
            _zMemory = new ZMemory();
        }

        public void ImportObjectTree(Tree tree)
        {
            // Tree.StartPassage not working still
            Passage startPassage = null;
            IEnumerable<Passage> passages = null;
            try
            {
                startPassage = tree.StartPassage;//tree.Passages.Single(entry => entry.Key.ToLower() == "start").Value;
                // Enumerate passages without the start passage
                passages = tree.Passages/*.Where(passage => passage.Key.ToLower() != "start")*/.Select(entry => entry.Value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " -> start-Passage fehlt");
                return;
            }
            List<ZRoutine> routines = new List<ZRoutine>();
            
            routines.Add(new ZRoutine(new ZInstruction[] { new Call1n(new ZRoutineLabel(startPassage.Name)) }) { Label = new ZRoutineLabel("main") });
            try
                {
            routines.Add(ConvertPassageToRoutine(startPassage));
                }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); 
                return;
            }
            foreach (Passage passage in passages)
            {
                try
                {
                    routines.Add(ConvertPassageToRoutine(passage));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            _zMemory.SetRoutines(routines);
        }

        private ZRoutine ConvertPassageToRoutine(Passage passage)
        {
            List<ZInstruction> instructions = new List<ZInstruction>();
            int currentLink = 0;
            List<string> links = new List<string>();

            instructions.Add(new EraseWindow(0));

            foreach (PassageContent content in passage.PassageContentList)
            {
                if (content.Type == PassageContent.ContentType.TextContent)
                {
                    SetTextStyle.StyleFlags flags = SetTextStyle.StyleFlags.None;
                    /* TODO flags neu einbauen
                    if (content.PassageText._bold)
                        flags |= SetTextStyle.StyleFlags.Bold;

                    if (content.PassageText._italic)
                        flags |= SetTextStyle.StyleFlags.Italic;

                    if (content.PassageText._monospace)
                        flags |= SetTextStyle.StyleFlags.FixedPitch;
                    */
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
                else
                {
                    throw new Exception("\n\nException - ConvertPassageToRoutine: I can't handle " + content.Type.ToString() + " yet. :(\n\n");
                }
            }

            if (currentLink > 0)
            {
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
                instructions.Add(new Quit());

                for (int i = 0; i < links.Count(); i++)
                {
                    instructions.Add(new Call1n(new ZRoutineLabel(links[i])) { Label = new ZLabel(links[i] + "Call") });
                }
            }
            else
            {
                instructions.Add(new Quit());
            }
            
            return new ZRoutine(instructions, 1) { Label = new ZRoutineLabel(passage.Name) };
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
