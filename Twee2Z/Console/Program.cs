using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Twee2Z.Analyzer;

namespace Twee2Z.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Create helloWorld story file ...");
            CodeGen.Memory.ZMemory helloWorld = new CodeGen.Memory.ZMemory();
            System.Console.WriteLine("Save story file as helloworld.z8 ...");
            File.WriteAllBytes("helloworld.z8", helloWorld.ToBytes());
            System.Console.WriteLine("The story file has been saved at:");
            System.Console.WriteLine(System.IO.Path.GetFullPath("helloworld.z8"));
            System.Console.WriteLine("");
            System.Console.WriteLine("NOTE: Zax cannot run helloworld.z8. Use Frotz instead for now.");
            System.Console.ReadKey(true);
        }
    }
}
