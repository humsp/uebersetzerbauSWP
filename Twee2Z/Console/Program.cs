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
        const string tweeFile = "passage.tw";
        const string zStroyFile = "storyfile.z8";

        static void Main(string[] args)
        {
            System.Console.WriteLine("Open twee file ...");
            FileStream tweeFileStream = new FileStream(tweeFile, FileMode.Open, FileAccess.Read, FileShare.Read);

            System.Console.WriteLine("Start analyzer ...");
            new ObjectTree.Tree();
            TreeBuilder treeBuilder = TweeAnalyzer.RunHelloWorldDemo(new StreamReader(tweeFileStream));

            System.Console.WriteLine("Create story file ...");
            CodeGen.ZStoryFile helloWorldStoryFile = new CodeGen.ZStoryFile();

            System.Console.WriteLine("Add instructions to story file ...");
            helloWorldStoryFile.SetupHelloWorldDemo(treeBuilder.liste.First(passage => passage.name == "start").text);

            System.Console.WriteLine("Save story file ...");
            File.WriteAllBytes(zStroyFile, helloWorldStoryFile.ToBytes());

            System.Console.WriteLine("The story file has been saved at:");
            System.Console.WriteLine(System.IO.Path.GetFullPath(zStroyFile));
            System.Console.WriteLine("");
            System.Console.WriteLine("Done. Have a nice day!");
            System.Console.ReadKey(true);
        }
    }
}
