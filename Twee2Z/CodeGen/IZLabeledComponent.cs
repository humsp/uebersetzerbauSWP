using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen
{
    /// <summary>
    /// Interface that every component of a story file has to implement.
    /// </summary>
    interface IZLabeledComponent : IZComponent
    {
        ZLabel Label { get; set; }

        void SetLabel(int absoluteAddr, string name);
    }
}
