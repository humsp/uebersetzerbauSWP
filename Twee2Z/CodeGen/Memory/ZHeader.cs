using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Twee2Z.CodeGen.Address;
using Twee2Z.CodeGen.Label;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// Represents the first 64K of a story file.
    /// See also "11. The format of the header" on page 61 for reference.
    /// </summary>
    class ZHeader : ZComponent
    {
        private const int HeaderSize = 64;

        private byte _versionNumber;
        private InterpreterFlags _interpreterFlags;
        private ushort _releaseNumber;
        private ushort _highMemoryAddr;
        private ushort _initProgramCounterAddr;
        private ushort _dictionaryAddr;
        private ushort _objectTableAddr;
        private ushort _globalVariablesTableAddr;
        private ushort _staticMemoryAddr;
        private GameFlags _gameFlags;
        private byte[] _serialNumber;
        private ushort _abbreviationsTableAddr;
        private ushort _lengthOfStoryFile;
        private ushort _checksumOfStoryFile;
        private byte _interpreterNumber;
        private byte _interpreterVersion;
        private byte _screenHeightInLines;
        private byte _screenWidthInChars;
        private ushort _screenWidthInUnits;
        private ushort _screenHeightInUnits;
        private byte _fontHeightInUnits;
        private byte _fontWidthInUnits;
        private ushort _routinesOffset;
        private ushort _staticStringsOffset;
        private byte _defaultBackgroundColor;
        private byte _defaultForegroundColor;
        private ushort _terminatingCharsTableAddr;
        private ushort _textWidthForOutputStream3;
        private ushort _standardRevisionNumber;
        private ushort _alphabetTableAddr;
        private ushort _headerExtensionTableAddr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticMemoryAddr">Base of static memory.</param>
        /// <param name="highMemoryAddr">Base of high memory.</param>
        /// <param name="initProgramCounterAddr">Initial address of the program counter.</param>
        /// <param name="dictionaryAddr">Address of the dictionary table.</param>
        /// <param name="objectTableAddr">Address of the object table.</param>
        /// <param name="globalVariablesTableAddr">Address of the global variables table.</param>
        /// <param name="headerExtensionTableAddr">Address of the header extension table.</param>
        public ZHeader(ushort staticMemoryAddr, ushort highMemoryAddr, ushort initProgramCounterAddr, ushort dictionaryAddr, ushort objectTableAddr, ushort globalVariablesTableAddr, ushort headerExtensionTableAddr)
            : this(staticMemoryAddr, highMemoryAddr, initProgramCounterAddr, dictionaryAddr, objectTableAddr, globalVariablesTableAddr, headerExtensionTableAddr, 0x0000)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticMemoryAddr">Base of static memory.</param>
        /// <param name="highMemoryAddr">Base of high memory.</param>
        /// <param name="initProgramCounterAddr">Initial address of the program counter.</param>
        /// <param name="dictionaryAddr">Address of the dictionary table.</param>
        /// <param name="objectTableAddr">Address of the object table.</param>
        /// <param name="globalVariablesTableAddr">Address of the global variables table.</param>
        /// <param name="headerExtensionTableAddr">Address of the header extension table.</param>
        /// <param name="releaseNumber">Release number given to this story file.</param>
        public ZHeader(ushort staticMemoryAddr, ushort highMemoryAddr, ushort initProgramCounterAddr, ushort dictionaryAddr, ushort objectTableAddr, ushort globalVariablesTableAddr, ushort headerExtensionTableAddr, ushort releaseNumber)
        {
            _versionNumber = 0x08;                                  // CodeGen supports version 8 only.
            _interpreterFlags = InterpreterFlags.None;              // Flags set by the interpreter. Let it be zero for a new story file.
            _releaseNumber = releaseNumber;                         // Release Number of this story file.
            _highMemoryAddr = highMemoryAddr;                       // Base of high memory. 0xFFFF
            _initProgramCounterAddr = initProgramCounterAddr;       // Initial value of program counter. 0x2000
            _dictionaryAddr = dictionaryAddr;                       // Points to the Dictionary Table. 0x4000
            _objectTableAddr = objectTableAddr;                     // Points to the Object Table. 0x0048
            _globalVariablesTableAddr = globalVariablesTableAddr;   // Points to the Global Variables Table. 0x0048
            _staticMemoryAddr = staticMemoryAddr;                   // Base of static memory. 0x4000
            _gameFlags = GameFlags.None;                            // Flags set by the game. Zero at start.
            _serialNumber = new byte[6];                            // Let's ignore the serial number for now.
            _abbreviationsTableAddr = 0x0000;                       // Not supported yet.
            _lengthOfStoryFile = 0x0000;                            // Not supported yet.
            _checksumOfStoryFile = 0x0000;                          // Not supported yet.
            _interpreterNumber = 0x00;                              // Set by interpreter.
            _interpreterVersion = 0x00;                             // Set by interpreter.
            _screenHeightInLines = 0x00;                            // Set by interpreter.
            _screenWidthInChars = 0x00;                             // Set by interpreter.
            _screenWidthInUnits = 0x0000;                           // Set by interpreter.
            _screenHeightInUnits = 0x0000;                          // Set by interpreter.
            _fontHeightInUnits = 0x00;                              // Set by interpreter.
            _fontWidthInUnits = 0x00;                               // Set by interpreter.
            _routinesOffset = 0x0000;                               // Is this version 6 only?
            _staticStringsOffset = 0x0000;                          // Is this version 6 only?
            _defaultBackgroundColor = 0x00;                         // Set by interpreter.
            _defaultForegroundColor = 0x00;                         // Set by interpreter.
            _terminatingCharsTableAddr = 0x0000;                    // Not supported yet.
            _textWidthForOutputStream3 = 0x0000;                    // Is this version 6 only?
            _standardRevisionNumber = 0x0000;                       // Set by interpreter.
            _alphabetTableAddr = 0x0000;                            // Not supported yet.
            _headerExtensionTableAddr = headerExtensionTableAddr;   // Points to Header Extension Table. 0x0040
        }

        /// <summary>
        /// Used version for this story file.
        /// </summary>
        public byte Version { get { return _versionNumber; } }

        /// <summary>
        /// Release number given to this story file.
        /// </summary>
        public ushort ReleaseNumber { get { return _releaseNumber; } }

        /// <summary>
        /// Base of static memory.
        /// </summary>
        public ushort StaticMemoryBase { get { return _staticMemoryAddr; } }

        /// <summary>
        /// Base of high memory.
        /// </summary>
        public ushort HighMemoryBase { get { return _highMemoryAddr; } }

        /// <summary>
        /// Initial address of the program counter.
        /// </summary>
        public ushort InitProgramCounterAddr { get { return _initProgramCounterAddr; } }

        /// <summary>
        /// Address of the dictionary table.
        /// </summary>
        public ushort DictionaryAddr { get { return _dictionaryAddr; } }

        /// <summary>
        /// Address of the object table.
        /// </summary>
        public ushort ObjectTableAddr { get { return _objectTableAddr; } }

        /// <summary>
        /// Address of the global variables table.
        /// </summary>
        public ushort GlobalVariablesTableAddr { get { return _globalVariablesTableAddr; } }

        /// <summary>
        /// Address of the header extension table.
        /// </summary>
        public ushort HeaderExtensionTableAddr { get { return _headerExtensionTableAddr; } }
            
        public override byte[] ToBytes()
        {
            byte[] byteArray = new byte[HeaderSize];

            byteArray[0x00] = _versionNumber;

            byteArray[0x01] = (byte)_interpreterFlags;

            byteArray[0x02] = (byte)(_releaseNumber >> 8);
            byteArray[0x03] = (byte)_releaseNumber;

            byteArray[0x04] = (byte)(_highMemoryAddr >> 8);
            byteArray[0x05] = (byte)_highMemoryAddr;

            byteArray[0x06] = (byte)(_initProgramCounterAddr >> 8);
            byteArray[0x07] = (byte)_initProgramCounterAddr;

            byteArray[0x08] = (byte)(_dictionaryAddr >> 8);
            byteArray[0x09] = (byte)_dictionaryAddr;

            byteArray[0x0A] = (byte)(_objectTableAddr >> 8);
            byteArray[0x0B] = (byte)_objectTableAddr;

            byteArray[0x0C] = (byte)(_globalVariablesTableAddr >> 8);
            byteArray[0x0D] = (byte)_globalVariablesTableAddr;

            byteArray[0x0E] = (byte)(_staticMemoryAddr >> 8);
            byteArray[0x0F] = (byte)_staticMemoryAddr;

            byteArray[0x34] = (byte)(_alphabetTableAddr >> 8);
            byteArray[0x35] = (byte)_alphabetTableAddr;

            byteArray[0x36] = (byte)(_headerExtensionTableAddr >> 8);
            byteArray[0x37] = (byte)_headerExtensionTableAddr;

            return byteArray;
        }

        public override int Size { get { return HeaderSize; } }

        protected override void SetLabel(int absoluteAddr, string name)
        {
            if (_componentLabel == null)
                _componentLabel = new ZLabel(new ZByteAddress(absoluteAddr), name);
            else if (_componentLabel.TargetAddress == null)
            {
                _componentLabel.TargetAddress = new ZByteAddress(absoluteAddr);
                _componentLabel.Name = name;
            }
            else
            {
                _componentLabel.TargetAddress.Absolute = absoluteAddr;
                _componentLabel.Name = name;
            }
        }

        /// <summary>
        /// Flags set by the interpreter to indicate support for certain features. We do not care about these but we are aware of it.
        /// </summary>
        [Flags]
        public enum InterpreterFlags
        {
            None = 0,
            /// <summary>
            /// Colors available
            /// </summary>
            Colors = 1,
            /// <summary>
            /// Picture displaying available
            /// </summary>
            PictureDisplaying = 2,
            /// <summary>
            /// Boldface available
            /// </summary>
            Boldface = 4,
            /// <summary>
            /// Italic available
            /// </summary>
            Italic = 8,
            /// <summary>
            /// Fixed-space font available
            /// </summary>
            FixedSpaceFont = 16,
            /// <summary>
            /// Sound effects available
            /// </summary>
            SoundEffects = 32,
            /// <summary>
            /// Timed keyboard input available
            /// </summary>
            TimedKeyboardInput = 128
        }

        /// <summary>
        /// Flags set by the game at runtime. We do not need them for now.
        /// </summary>
        [Flags]
        public enum GameFlags
        {
            None = 0,
            /// <summary>
            /// Set when transcripting is on
            /// </summary>
            TranscriptingEnabled = 1,
            /// <summary>
            /// Game sets to force printing in fixed-pitch font
            /// </summary>
            FixedPitchPrintingEnabled = 2,
            /// <summary>
            /// If set, game wants to use pictures
            /// </summary>
            UsePictures = 4,
            /// <summary>
            /// If set, game wants to use the UNDO opcodes
            /// </summary>
            UseUndoOpcodes = 8,
            /// <summary>
            /// If set, game wants to use a mouse
            /// </summary>
            UseMouse = 16,
            /// <summary>
            /// If set, game wants to use colors
            /// </summary>
            UseColors = 32,
            /// <summary>
            ///  If set, game wants to use sound effects
            /// </summary>
            UseSoundEffects = 128,
            /// <summary>
            ///  If set, game wants to use menus
            /// </summary>
            UseMenus = 256
        }
    }
}
