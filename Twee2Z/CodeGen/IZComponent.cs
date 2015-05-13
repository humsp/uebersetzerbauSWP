using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen
{
    /// <summary>
    /// Interface that every component of a story file has to implement.
    /// </summary>
    interface IZComponent
    {
        /// <summary>
        /// Converts the component to valid byte code for the story file.
        /// </summary>
        /// <returns>Byte code for the story file.</returns>
        byte[] ToBytes();
    }
}
