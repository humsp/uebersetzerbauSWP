using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Memory;
using Twee2Z.CodeGen.Instruction;
using Twee2Z.CodeGen.Instruction.Template;
using Twee2Z.CodeGen.Label;
using Twee2Z.CodeGen.Variable;

namespace Twee2Z.CodeGen
{
    public class ZStoryFile
    {
        private ZMemory _zMemory;

        public ZStoryFile()
        {
            _zMemory = new ZMemory();
        }

        public void SetupPassageNavigationDemo(ObjectTree.Tree tree)
        {
            List<ZInstruction> _mainInstructions = new List<ZInstruction>();
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo));
            _mainInstructions.Add(new Print("Twee2Z Meilenstein 3"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
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
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _mainInstructions.Add(new Print("1"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _mainInstructions.Add(new Print("Weiche vom Plan ab"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _mainInstructions.Add(new Print("2"));
            _mainInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _mainInstructions.Add(new NewLine());

            _mainInstructions.Add(new PrintUnicode('>'));
            _mainInstructions.Add(new Print(" "));
            _mainInstructions.Add(new ReadChar());
            _mainInstructions.Add(new Je(new ZLocalVariable(0), '1', new ZBranchLabel("safeCall")));
            _mainInstructions.Add(new Je(new ZLocalVariable(0), '2', new ZBranchLabel("unsafeCall")));

            _mainInstructions.Add(new Quit());

            _mainInstructions.Add(new Call1n(new ZRoutineLabel("safe")) { Label = new ZLabel("safeCall") });
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("unsafe")) { Label = new ZLabel("unsafeCall") });

            List<ZInstruction> _unsafeInstructions = new List<ZInstruction>();
            _unsafeInstructions.Add(new Print("2" + System.Environment.NewLine + System.Environment.NewLine));
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
            _safeInstructions.Add(new Print("1" + System.Environment.NewLine + System.Environment.NewLine));
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
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("1"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _safeInstructions.Add(new Print("Ok"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("2"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _safeInstructions.Add(new Print("Meh"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo | SetTextStyle.StyleFlags.FixedPitch));
            _safeInstructions.Add(new Print("3"));
            _safeInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _safeInstructions.Add(new NewLine());

            _safeInstructions.Add(new PrintUnicode('>'));
            _safeInstructions.Add(new Print(" "));
            _safeInstructions.Add(new ReadChar());
            _safeInstructions.Add(new Je(new ZLocalVariable(0), '1', new ZBranchLabel("tollCall")));
            _safeInstructions.Add(new Je(new ZLocalVariable(0), '2', new ZBranchLabel("okCall")));
            _safeInstructions.Add(new Je(new ZLocalVariable(0), '3', new ZBranchLabel("mehCall")));

            _safeInstructions.Add(new Call1n(new ZRoutineLabel("toll")) { Label = new ZLabel("tollCall") });
            _safeInstructions.Add(new Call1n(new ZRoutineLabel("ok")) { Label = new ZLabel("okCall") });
            _safeInstructions.Add(new Call1n(new ZRoutineLabel("meh")) { Label = new ZLabel("mehCall") });

            _safeInstructions.Add(new Quit());

            List<ZInstruction> _tollInstructions = new List<ZInstruction>();
            _tollInstructions.Add(new Print("1" + System.Environment.NewLine + System.Environment.NewLine));
            _tollInstructions.Add(new Print("Toll!"));
            _tollInstructions.Add(new NewLine());
            _tollInstructions.Add(new Quit());

            List<ZInstruction> _okInstructions = new List<ZInstruction>();
            _okInstructions.Add(new Print("2" + System.Environment.NewLine + System.Environment.NewLine));
            _okInstructions.Add(new Print("Ok."));
            _okInstructions.Add(new NewLine());
            _okInstructions.Add(new Quit());

            List<ZInstruction> _mehInstructions = new List<ZInstruction>();
            _mehInstructions.Add(new Print("3" + System.Environment.NewLine + System.Environment.NewLine));
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

        public void SetupHelloWorldDemo(string input)
        {
            List<ZInstruction> _mainInstructions = new List<ZInstruction>();
            _mainInstructions.Add(new Print("Startseite:"));
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new Print("Link A" + System.Environment.NewLine));
            _mainInstructions.Add(new Print("Link B" + System.Environment.NewLine));
            _mainInstructions.Add(new Print("Link C" + System.Environment.NewLine));
            _mainInstructions.Add(new PrintUnicode('>'));
            _mainInstructions.Add(new Print(" "));
            _mainInstructions.Add(new ReadChar());
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new Je(new ZLocalVariable(0), 'A', new ZBranchLabel("callLinkA")));
            _mainInstructions.Add(new Je(new ZLocalVariable(0), 'B', new ZBranchLabel("callLinkB")));
            _mainInstructions.Add(new Je(new ZLocalVariable(0), 'C', new ZBranchLabel("callLinkC")));

            _mainInstructions.Add(new Print("Unbekannte Eingabe!" + System.Environment.NewLine));
            _mainInstructions.Add(new Quit());

            _mainInstructions.Add(new Call1n(new ZRoutineLabel("linkA")) { Label = new ZLabel("callLinkA") });
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("linkB")) { Label = new ZLabel("callLinkB") });
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("linkC")) { Label = new ZLabel("callLinkC") });
            _mainInstructions.Add(new Quit());

            List<ZInstruction> _linkAInstructions = new List<ZInstruction>();
            _linkAInstructions.Add(new Print("Link A:"));
            _linkAInstructions.Add(new NewLine());
            _linkAInstructions.Add(new Print("Sie haben Link A ausgewaehlt!" + System.Environment.NewLine));
            _linkAInstructions.Add(new Quit());

            List<ZInstruction> _linkBInstructions = new List<ZInstruction>();
            _linkBInstructions.Add(new Print("Link B:"));
            _linkBInstructions.Add(new NewLine());
            _linkBInstructions.Add(new Print("Sie haben Link B ausgewaehlt!" + System.Environment.NewLine));
            _linkBInstructions.Add(new Quit());

            List<ZInstruction> _linkCInstructions = new List<ZInstruction>();
            _linkCInstructions.Add(new Print("Link C:"));
            _linkCInstructions.Add(new NewLine());
            _linkCInstructions.Add(new Print("Sie haben Link C ausgewaehlt!" + System.Environment.NewLine));
            _linkCInstructions.Add(new Quit());

            _zMemory.SetRoutines(new ZRoutine[] { new ZRoutine(_mainInstructions) { Label = new ZRoutineLabel("main") },
                new ZRoutine(_linkAInstructions, 1) { Label = new ZRoutineLabel("linkA") },
                new ZRoutine(_linkBInstructions) { Label = new ZRoutineLabel("linkB") },
                new ZRoutine(_linkCInstructions) { Label = new ZRoutineLabel("linkC") }});

            /*_mainInstructions.Add(new Print("Ist 1 gleich 2? "));
            //_mainInstructions.Add(new Jump(new ZJumpLabel("ja")));
            _mainInstructions.Add(new Je((ushort)1, (ushort)2, new ZBranchLabel("ja")));

            _mainInstructions.Add(new Print("Nein!" + System.Environment.NewLine) { Label = new ZLabel("nein") });
            _mainInstructions.Add(new Quit());

            _mainInstructions.Add(new Print("Ja!" + System.Environment.NewLine) { Label = new ZLabel("ja") });
            _mainInstructions.Add(new Quit() { Label = new ZLabel("quit") });

            _zMemory.SetRoutines(new ZRoutine[] { new ZRoutine(_mainInstructions) { Label = new ZRoutineLabel("main") } });*/

            /*_mainInstructions.Add(new Print("main:" + System.Environment.NewLine));
            _mainInstructions.Add(new Print("Teste neue Speicherverwaltung mit call_1n ..."));
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new NewLine());
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("2ndRoutine")));
            
            _mainInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _mainInstructions.Add(new Quit());

            List<ZInstruction> _helloWorldInstructions = new List<ZInstruction>();
            _helloWorldInstructions.Add(new Print("helloworldRoutine:" + System.Environment.NewLine));


            StringBuilder splitInput = new StringBuilder();
            foreach (char c in input)
            {
                if (c < 128)
                    splitInput.Append(c);
                else
                {
                    _helloWorldInstructions.Add(new Print(splitInput.ToString()));
                    _helloWorldInstructions.Add(new PrintUnicode(c));
                    splitInput.Clear();
                }
            }
            _helloWorldInstructions.Add(new Print(splitInput.ToString()));

            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Bold));
            _helloWorldInstructions.Add(new Print("Fetter Text" + Environment.NewLine));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Italic));
            _helloWorldInstructions.Add(new Print("Kursiver Text" + Environment.NewLine));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.FixedPitch));
            _helloWorldInstructions.Add(new Print("Monospace Text" + Environment.NewLine));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Roman));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.ReverseVideo));
            _helloWorldInstructions.Add(new Print("ReverseVideo Text" + Environment.NewLine));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Bold | SetTextStyle.StyleFlags.FixedPitch | SetTextStyle.StyleFlags.Italic | SetTextStyle.StyleFlags.ReverseVideo));
            _helloWorldInstructions.Add(new Print("Alles zusammen Text" + Environment.NewLine));
            _helloWorldInstructions.Add(new Quit());

            List<ZInstruction> _2ndRoutineInstructions = new List<ZInstruction>();
            _2ndRoutineInstructions.Add(new Print("2ndRoutine:" + System.Environment.NewLine));
            _2ndRoutineInstructions.Add(new Print("Aufruf hat geklappt!" + System.Environment.NewLine));

            _2ndRoutineInstructions.Add(new Print("Ist 1 gleich 2? "));
            _2ndRoutineInstructions.Add(new Je(1, 2, new ZBranchLabel("printJa")));
            _2ndRoutineInstructions.Add(new Print("Nein!" + System.Environment.NewLine));
            _2ndRoutineInstructions.Add(new Jump(new ZJumpLabel("weiter")));
            _2ndRoutineInstructions.Add(new Print("Ja!" + System.Environment.NewLine) { Label = new ZLabel("printJa") });

            _2ndRoutineInstructions.Add(new Print("Rufe nun normale Hello World-Routine auf ...") { Label = new ZLabel("weiter") });
            _2ndRoutineInstructions.Add(new NewLine());
            _2ndRoutineInstructions.Add(new NewLine());
            
            _2ndRoutineInstructions.Add(new Jump(new ZJumpLabel("helloworldCall")));
            _2ndRoutineInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _2ndRoutineInstructions.Add(new Quit());

            _2ndRoutineInstructions.Add(new Call1n(new ZRoutineLabel("helloworldRoutine")) { Label = new ZLabel("helloworldCall") });
            
            _2ndRoutineInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _2ndRoutineInstructions.Add(new Quit());

            _zMemory.SetRoutines(new ZRoutine[] { new ZRoutine(_mainInstructions) { Label = new ZRoutineLabel("main") },
                new ZRoutine(_2ndRoutineInstructions) { Label = new ZRoutineLabel("2ndRoutine") },
                new ZRoutine(_helloWorldInstructions) { Label = new ZRoutineLabel("helloworldRoutine") } });*/
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
