using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Instructions;
using Twee2Z.CodeGen.Instructions.Templates;

namespace Twee2Z.CodeGen.Memory
{
    public class ZHighMemory
    {
        //hard coded hello world for testing
        private ZRoutine _helloWorldRoutine;

        public ZHighMemory()
        {
            List<ZInstruction> _helloWorldInstructions = new List<ZInstruction>();
            _helloWorldInstructions.Add(new Print("Hallo Welt!" + System.Environment.NewLine + "Dies ist eine neue Zeile." + System.Environment.NewLine));
            _helloWorldInstructions.Add(new Quit());

            _helloWorldRoutine = new ZRoutine(_helloWorldInstructions);
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[0x70000];
            _helloWorldRoutine.ToBytes().CopyTo(byteArray, 0x0000);
            return byteArray;
        }
    }
}
