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
            System.Console.WriteLine("Create helloworld story file ...");
            CodeGen.ZStoryFile helloWorldStoryFile = new CodeGen.ZStoryFile();
            System.Console.WriteLine("Add instructions to story file ...");
            helloWorldStoryFile.SetupHelloWorldDemo();
            System.Console.WriteLine("Save story file as helloworld.z8 ...");
            File.WriteAllBytes("helloworld.z8", helloWorldStoryFile.ToBytes());
            System.Console.WriteLine("The story file has been saved at:");
            System.Console.WriteLine(System.IO.Path.GetFullPath("helloworld.z8"));
            System.Console.WriteLine("");
            System.Console.WriteLine("NOTE: Zax cannot run helloworld.z8. Use Frotz instead for now.");
            System.Console.ReadKey(true);
        }
    }
}
