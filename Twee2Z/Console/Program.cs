using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;

namespace Twee2Z.Console
{
    class Program
    {
        const string tweeFile = "passage.tw";
        const string zStroyFile = "storyfile.z8";

        static void Main(string[] args)
        {
            System.Console.WriteLine("Open twee file ...");
            FileStream tweeFileStream = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read);

            System.Console.WriteLine("Start analyzer ...");
            ObjectTree.Tree tree = TweeAnalyzer.Parse(new StreamReader(tweeFileStream));

            System.Console.WriteLine("Create story file ...");
            CodeGen.ZStoryFile helloWorldStoryFile = new CodeGen.ZStoryFile();

            System.Console.WriteLine("Add instructions to story file ...");
            string text = tree.StartPassage.PassageContentList.ElementAt(0).PassageText.Text;
            helloWorldStoryFile.SetupHelloWorldDemo(text);

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
