using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Twee2Z.CodeGen
{
    public class CodeGen
    {
        public static void generate(ObjectTree.Root tree, string zFile)
        {
            ZMemory p = new ZMemory();

            File.Delete(zFile);
            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(zFile);
            fileWriter.Write(p.export());
            fileWriter.Close();
        }
    }
}
