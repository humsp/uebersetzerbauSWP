using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Memory;
using Twee2Z.CodeGen.Instruction;
using Twee2Z.CodeGen.Instruction.Template;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen
{
    public class ZStoryFile
    {
        private ZMemory _zMemory;

        public ZStoryFile()
        {
            _zMemory = new ZMemory();
        }

        public void SetupHelloWorldDemo(string input)
        {
            List<ZInstruction> _mainInstructions = new List<ZInstruction>();
            _mainInstructions.Add(new Print("main:" + System.Environment.NewLine));
            _mainInstructions.Add(new Print("Teste neue Speicherverwaltung mit call_1n ..." + System.Environment.NewLine + System.Environment.NewLine));
            _mainInstructions.Add(new Call1n(new ZRoutineLabel("2ndRoutine")));
            
            _mainInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _mainInstructions.Add(new Quit());

            List<ZInstruction> _helloWorldInstructions = new List<ZInstruction>();
            _helloWorldInstructions.Add(new Print("helloworldRoutine:" + System.Environment.NewLine) { Label = new ZLabel("intro") });

            _helloWorldInstructions.Add(new Print(input));

            _helloWorldInstructions.Add(new Print(Environment.NewLine));
            _helloWorldInstructions.Add(new SetTextStyle(SetTextStyle.StyleFlags.Bold) { Label = new ZLabel("teil2") });
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
            _2ndRoutineInstructions.Add(new Print("Aufruf hat geklappt! Rufe nun normale Hello World-Routine auf ..." + System.Environment.NewLine + System.Environment.NewLine));
            
            _2ndRoutineInstructions.Add(new Jump(new ZJumpLabel("helloworldCall")));
            _2ndRoutineInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _2ndRoutineInstructions.Add(new Quit());

            _2ndRoutineInstructions.Add(new Call1n(new ZRoutineLabel("helloworldRoutine")) { Label = new ZLabel("helloworldCall") });
            
            _2ndRoutineInstructions.Add(new Print("Fehler: Dieser Text duerfte nicht zu lesen sein!"));
            _2ndRoutineInstructions.Add(new Quit());

            _zMemory.SetRoutines(new ZRoutine[] { new ZRoutine(_mainInstructions) { Label = new ZRoutineLabel("main") },
                new ZRoutine(_2ndRoutineInstructions) { Label = new ZRoutineLabel("2ndRoutine") },
                new ZRoutine(_helloWorldInstructions) { Label = new ZRoutineLabel("helloworldRoutine") } });
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
