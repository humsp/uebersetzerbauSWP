using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.CodeGen
{
    class ZMemory
    {
        List<UInt16> itemList = new List<UInt16>();

        public ZMemory() : this(8)
        {
        }

        private ZMemory(UInt16 version)
        {
            itemList.Add(version);

        }

        public void add(UInt16 value)
        {
            itemList.Add(value);
        }

        public void set(UInt16 value, int pos)
        {
            fill(pos);
            add(value);
        }

        private void fill(int to)
        {
            for (int i = itemList.Count(); i < to; i++)
            {
                itemList.Add(0);
            }
        }

        public void print()
        {
            foreach (UInt16 x in itemList)
            {
                Console.WriteLine(x);
            }
        }


        public char[] export()
        {
            List<char> exportList = new List<char>();

            int result = 0;
            int count = 2;
            foreach (UInt16 x in itemList)
            {
                result = result << count;
                result += x;
                count-=2;
                if (count == 0)
                {
                    count = 2;
                    exportList.Add((char)result);
                    result = 0;
                }
            }
            return exportList.ToArray();
        }
    }
}
