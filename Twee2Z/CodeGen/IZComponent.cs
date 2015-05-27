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
        IEnumerable<IZComponent> SubComponents { get; }

        /// <summary>
        /// Converts the component to valid byte code for the story file.
        /// </summary>
        /// <returns>Byte code for the story file.</returns>
        byte[] ToBytes();

        /// <summary>
        /// Returns the size of the component. The size corresponds with the byte array length of ToBytes()./>.
        /// </summary>
        int Size { get; }
    }
}
