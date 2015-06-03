using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Memory;
using Twee2Z.CodeGen.Instructions;
using Twee2Z.CodeGen.Instructions.Templates;

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
            List<ZInstruction> _helloWorldInstructions = new List<ZInstruction>();

            _helloWorldInstructions.Add(new Print(input));

            _helloWorldInstructions.Add(new Print(Environment.NewLine));
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

            _zMemory.SetRoutines(new ZRoutine[] { new ZRoutine("main", _helloWorldInstructions) });
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[ZMemory.MaxMemorySize];

            _zMemory.ToBytes().CopyTo(byteArray, 0);

            return byteArray;
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
