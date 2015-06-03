﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twee2Z.CodeGen.Address;

namespace Twee2Z.CodeGen
{
    /// <summary>
    /// Interface that every component of a story file has to implement.
    /// </summary>
    interface IZComponent
    {
        List<IZComponent> SubComponents { get; }

        /// <summary>
        /// Converts the component to valid byte code for the story file.
        /// </summary>
        /// <returns>Byte code for the story file.</returns>
        byte[] ToBytes();

        /// <summary>
        /// Gets the size of the component. The size equals the byte array length of ToBytes()./>.
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Gets the absolute address of the component within the story file.
        /// </summary>
        ZAddress Address { get; set; }
    }
}
