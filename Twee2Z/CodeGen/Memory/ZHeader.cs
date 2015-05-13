using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Twee2Z.CodeGen.Memory
{
    /// <summary>
    /// See "11. The format of the header" on page 61 from "The Z-Machine Standards Document (Version 1.0)".
    /// </summary>
    public class ZHeader
    {
        private Byte _versionNumber;
        private AvailabilityFlags _flags1;
        private UInt16 _releaseNumber;
        private UInt16 _highMemoryAddr;
        private UInt16 _initProgramCounterAddr;
        private UInt16 _dictionaryAddr;
        private UInt16 _objectTableAddr;
        private UInt16 _globalVariablesTableAddr;
        private UInt16 _staticMemoryAddr;
        private RuntimeFlags _flags2;
        private Byte[] _serialNumber;
        private UInt16 _abbreviationsTableAddr;
        private UInt16 _lengthOfStoryFile;
        private UInt16 _checksumOfStoryFile;
        private Byte _interpreterNumber;
        private Byte _interpreterVersion;
        private Byte _screenHeightInLines;
        private Byte _screenWidthInChars;
        private UInt16 _screenWidthInUnits;
        private UInt16 _screenHeightInUnits;
        private Byte _fontHeightInUnits;
        private Byte _fontWidthInUnits;
        private UInt16 _routinesOffset;
        private UInt16 _staticStringsOffset;
        private Byte _defaultBackgroundColor;
        private Byte _defaultForegroundColor;
        private UInt16 _terminatingCharsTableAddr;
        private UInt16 _textWidthForOutputStream3;
        private UInt16 _standardRevisionNumber;
        private UInt16 _alphabetTableAddr;
        private UInt16 _headerExtensionTableAddr;

        public ZHeader()
        {
            _versionNumber = 0x08;
            _flags1 = AvailabilityFlags.None;
            _releaseNumber = 0x0000;
            _highMemoryAddr = 0xFFFF;
            _initProgramCounterAddr = 0x2000;
            _dictionaryAddr = 0x4000;
            _objectTableAddr = 0x0048;
            _globalVariablesTableAddr = 0x0048;
            _staticMemoryAddr = 0x4000;
            _flags2 = RuntimeFlags.None;
            _serialNumber = new Byte[6];
            _abbreviationsTableAddr = 0x0000;
            _lengthOfStoryFile = 0x0000;
            _checksumOfStoryFile = 0x0000;
            _interpreterNumber = 0x00;
            _interpreterVersion = 0x00;
            _screenHeightInLines = 0x00;
            _screenWidthInChars = 0x00;
            _screenWidthInUnits = 0x0000;
            _screenHeightInUnits = 0x0000;
            _fontHeightInUnits = 0x00;
            _fontWidthInUnits = 0x00;
            _routinesOffset = 0x0000;
            _staticStringsOffset = 0x0000;
            _defaultBackgroundColor = 0x00;
            _defaultForegroundColor = 0x00;
            _terminatingCharsTableAddr = 0x0000;
            _textWidthForOutputStream3 = 0x0000;
            _standardRevisionNumber = 0x0000;
            _alphabetTableAddr = 0x0000;
            _headerExtensionTableAddr = 0x0040;
        }

        public Byte[] ToBytes()
        {
            Byte[] byteArray = new Byte[64];

            byteArray[0x00] = _versionNumber;

            byteArray[0x01] = (Byte)_flags1;

            byteArray[0x02] = (Byte)(_releaseNumber >> 8);
            byteArray[0x03] = (Byte)_releaseNumber;

            byteArray[0x04] = (Byte)(_highMemoryAddr >> 8);
            byteArray[0x05] = (Byte)_highMemoryAddr;

            byteArray[0x06] = (Byte)(_initProgramCounterAddr >> 8);
            byteArray[0x07] = (Byte)_initProgramCounterAddr;

            byteArray[0x08] = (Byte)(_dictionaryAddr >> 8);
            byteArray[0x09] = (Byte)_dictionaryAddr;

            byteArray[0x0A] = (Byte)(_objectTableAddr >> 8);
            byteArray[0x0B] = (Byte)_objectTableAddr;

            byteArray[0x0C] = (Byte)(_globalVariablesTableAddr >> 8);
            byteArray[0x0D] = (Byte)_globalVariablesTableAddr;

            byteArray[0x0E] = (Byte)(_staticMemoryAddr >> 8);
            byteArray[0x0F] = (Byte)_staticMemoryAddr;

            byteArray[0x34] = (Byte)(_alphabetTableAddr >> 8);
            byteArray[0x35] = (Byte)_alphabetTableAddr;

            byteArray[0x36] = (Byte)(_headerExtensionTableAddr >> 8);
            byteArray[0x37] = (Byte)_headerExtensionTableAddr;

            return byteArray;
        }

        public UInt32 Size { get { return 64; } }

        [Flags]
        public enum AvailabilityFlags
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

        [Flags]
        public enum RuntimeFlags
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
