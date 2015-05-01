using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen
{
    class ZText
    {
        private static char[] letters = new char[]{
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',

            '\0', '^', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.',
            ',', '!', '?', '_', '#', '\'','"', '/', '\\','-', ':', '(', ')'
        };

        private static IDictionary<char, int> letterDict = new Dictionary<char, int>();

        public ZText()
        {
            for (int i = 0; i < letters.Length; i++)
            {
                letterDict.Add(letters[i], i);
            }
        }


        public UInt16[] encode(String text)
        {
            List<UInt16> result = new List<UInt16>();
            List<int> newText = convertLetters(text);

            return mergeLetters(newText).ToArray();
        }


        private List<UInt16> mergeLetters(List<int> text)
        {
            List<UInt16> result = new List<UInt16>((text.Count() * 2) / 3);

            String s = "";
            int j = 0;
            for (int i = 0; i < text.Count(); i++)
            {
                j++;
                s += toByteString(text[i]);
                if (j == 3)
                {
                    j = 0;
                    result.Add(bitsToBytes('0' + s));
                    s = "";
                }
                else if (i + 1 == text.Count())
                {
                    for (; j < 3; j++)
                    {
                        s += toByteString(5);
                    }
                    result.Add(bitsToBytes('1' + s));
                }
            }

            return result;
        }

        private UInt16 bitsToBytes(String bits)
        {
            String fst = bits.Substring(0, 8);
            String snd = bits.Substring(8);

            byte[] byteArray = new byte[2];
            byteArray[0] = System.Convert.ToByte(snd, 2);
            byteArray[1] = System.Convert.ToByte(fst, 2);

            return (UInt16)System.BitConverter.ToUInt16(byteArray, 0);
        }

        private string toByteString(int i)
        {
            return System.Convert.ToString(i, 2).PadLeft(5, '0');
        }


        private List<int> convertLetters(String text)
        {
            List<int> tempRes = new List<int>();

            foreach (char c in text)
            {
                if (letterDict.ContainsKey(c))
                {
                    int x = letterDict[c];
                    int a = x / 32;
                    if (a == 1)
                    {
                        tempRes.Add(4);
                    }
                    else if (a == 2)
                    {
                        tempRes.Add(5);
                    }
                    tempRes.Add(x + 6);
                }
            }
            return tempRes;
        }
    }
}
