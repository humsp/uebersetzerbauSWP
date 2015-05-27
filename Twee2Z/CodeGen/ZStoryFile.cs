﻿using System;
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
