using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen.Text
{
    static class TextHelper
    {
        private const byte ZCharSpaceNumber = 0;
        private const byte ZCharShiftUpperCaseNumber = 4;
        private const byte ZCharShiftPunctuationNumber = 5;

        /// <summary>
        /// lower case
        /// </summary>
        private static char[] tableA0 = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };

        /// <summary>
        /// upper case
        /// </summary>
        private static char[] tableA1 = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// punctuation
        /// </summary>
        private static char[] tableA2 = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.',
            ',', '!', '?', '_', '#', '\'','"', '/', '\\','-', ':', '(', ')'
        };

        private static IDictionary<char, ZChar> charDictionary = new Dictionary<char, ZChar>();

        private static IDictionary<ZControlCharKind, ZControlChar> controlCharDictionary = new Dictionary<ZControlCharKind, ZControlChar>();

        static TextHelper()
        {
            byte counter = 0x06;
            foreach (char entry in tableA0)
            {
                charDictionary.Add(entry, new ZChar() { ascii = entry, number = counter, table = 0 });
                counter++;
            }

            counter = 0x06;
            foreach (char entry in tableA1)
            {
                charDictionary.Add(entry, new ZChar() { ascii = entry, number = counter, table = 1 });
                counter++;
            }

            counter = 0x08;
            foreach (char entry in tableA2)
            {
                charDictionary.Add(entry, new ZChar() { ascii = entry, number = counter, table = 2 });
                counter++;
            }

            controlCharDictionary.Add(ZControlCharKind.NewLine, new ZControlChar() { kind = ZControlCharKind.NewLine, number = 7, table = 2 });
            controlCharDictionary.Add(ZControlCharKind.TenBitZCharacter, new ZControlChar() { kind = ZControlCharKind.TenBitZCharacter, number = 6, table = 2 });
        }

        public static bool IsZSCII(char input)
        {
            return input == ' ' || tableA0.Contains(input) || tableA1.Contains(input) || tableA2.Contains(input);
        }

        public static int Measure(String input)
        {
            int zCharCounter = 0;
            int i = 0;

            while (i < input.Length)
            {
                if (input[i] == ' ')
                {
                    zCharCounter++;
                }
                else if (input[i].ToString() == System.Environment.NewLine)
                {
                    zCharCounter++;
                }
                else if (i + 1 < input.Length && input.Substring(i, 2) == System.Environment.NewLine)
                {
                    zCharCounter += 2;
                    i++;
                }
                else if (input[i] == '\r' || input[i] == '\n')
                {
                    i++;
                }
                else
                {
                    if (tableA0.Contains(input[i]))
                        zCharCounter++;
                    else if (tableA1.Contains(input[i]) || tableA2.Contains(input[i]))
                        zCharCounter += 2;
                    else
                        throw new Exception(String.Format("The given char is not a valid ZChar: '{0}'", input[i]));
                }

                i++;
            }

            // Three chars fit into two bytes. Two Bytes are used for only one or two chars as well.
            return 2 * ((int)Math.Ceiling(zCharCounter / 3.0));
        }

        public static UInt16[] Convert(String input)
        {
            IList<UInt16> output = new List<UInt16>();
            int i = 0;
            byte zCharCount = 0;

            while (i < input.Length)
            {
                if (input[i] == ' ')
                {                    
                    AddCharToArray(ref output, ZCharSpaceNumber, 0, ref zCharCount);
                }
                else if (input[i].ToString() == System.Environment.NewLine)
                {
                    ZControlChar zControlChar;
                    if (!controlCharDictionary.TryGetValue(ZControlCharKind.NewLine, out zControlChar))
                        throw new Exception("NewLine not found in dictionary");

                    AddCharToArray(ref output, zControlChar.number, zControlChar.table, ref zCharCount);
                }
                else if (i+1 < input.Length && input.Substring(i, 2) == System.Environment.NewLine)
                {
                    ZControlChar zControlChar;
                    if (!controlCharDictionary.TryGetValue(ZControlCharKind.NewLine, out zControlChar))
                        throw new Exception("NewLine not found in dictionary");

                    AddCharToArray(ref output, zControlChar.number, zControlChar.table, ref zCharCount);
                    i++;
                }
                else if (input[i] == '\r' || input[i] == '\n')
                {
                    ZControlChar zControlChar;
                    if (!controlCharDictionary.TryGetValue(ZControlCharKind.NewLine, out zControlChar))
                        throw new Exception("NewLine not found in dictionary");

                    AddCharToArray(ref output, zControlChar.number, zControlChar.table, ref zCharCount);
                }
                else
                {
                    ZChar zChar;
                    if (!charDictionary.TryGetValue(input[i], out zChar))
                        throw new Exception("The given char is not a valid ZChar!");

                    AddCharToArray(ref output, zChar.number, zChar.table, ref zCharCount);
                }
                
                i++;
                
                if (i == input.Length)
                    FinishArray(ref output, ref zCharCount);
            }

            return output.ToArray();
        }

        private static void AddCharToArray(ref IList<UInt16> zCharList, byte number, byte table, ref byte zCharCount)
        {
            if (number >= 32)
                throw new ArgumentOutOfRangeException("number", "A ZChar must have a value between 0 and 31.");

            if (table > 2)
                throw new ArgumentOutOfRangeException("table", "Only alphabet tables between 0 and 2 are supported.");

            UInt16 currentWord;

            if (zCharCount % 3 == 0)
            {
                currentWord = new UInt16();
                zCharList.Add(currentWord);
            }
            else
            {
                currentWord = zCharList.Last();
            }

            if (table == 1)
            {
                AddCharToWord(ref currentWord, ZCharShiftUpperCaseNumber, ref zCharCount);
                zCharList[zCharList.Count - 1] = currentWord;
                AddCharToArray(ref zCharList, number, 0, ref zCharCount);
            }
            else if (table == 2)
            {
                AddCharToWord(ref currentWord, ZCharShiftPunctuationNumber, ref zCharCount);
                zCharList[zCharList.Count - 1] = currentWord;
                AddCharToArray(ref zCharList, number, 0, ref zCharCount);
            }
            else
            {
                AddCharToWord(ref currentWord, number, ref zCharCount);
                zCharList[zCharList.Count - 1] = currentWord;
            }
        }

        private static void AddCharToWord(ref UInt16 word, byte number, ref byte zCharCount)
        {
            switch (zCharCount % 3)
            {
                case 0:
                    word += (UInt16)(number << 10);
                    break;
                case 1:
                    word += (UInt16)(number << 5);
                    break;
                default:
                    word += number;
                    break;
            }

            zCharCount++;
        }

        private static void FinishArray(ref IList<UInt16> zCharList, ref byte zCharCount)
        {
            UInt16 word = zCharList.Last();

            word += 1 << 15;

            if (zCharCount % 3 == 1)
            {
                word += (UInt16)(ZCharShiftPunctuationNumber << 5);
                word += ZCharShiftPunctuationNumber;
                zCharCount += 2;
            }
            else if (zCharCount % 3 == 2)
            {
                word += ZCharShiftPunctuationNumber;
                zCharCount++;
            }

            zCharList[zCharList.Count - 1] = word;
        }

        private struct ZChar
        {
            public char ascii;
            public byte number;
            public byte table;
        }

        private struct ZControlChar
        {
            public ZControlCharKind kind;
            public byte number;
            public byte table;
        }

        private enum ZControlCharKind
        {
            Space,
            NewLine,
            TenBitZCharacter
        }
    }
}
