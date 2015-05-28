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
    public class Program
    {
        const string tweeFile = "passage.tw";
        const string zStroyFile = "storyfile.z8";

        static void Main(string[] args)
        {
            Complie(args[0], zStroyFile);
            
            System.Console.WriteLine("");
            System.Console.WriteLine("Done. Have a nice day!");
            System.Console.ReadKey(true);
        }


        static void Complie(string from, string output)
        {
            System.Console.WriteLine("Open twee file ...");
            FileStream tweeFileStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read);
            Tree tree = AnalyseFile(tweeFileStream);
            ValidateTree(tree);
            CodeGen.ZStoryFile storyFile = GenStoryFile(tree);
            WriteStoryFile(storyFile, output);
        }

        public static Tree AnalyseFile(FileStream stream)
        {
            System.Console.WriteLine("Start analyzer ...");
            return TweeAnalyzer.Parse(new StreamReader(stream));
        }

        public static void ValidateTree(Tree tree)
        {
            TreeValidator validator = new TreeValidator(tree);
            validator.ValidateTree();
        }

        public static CodeGen.ZStoryFile GenStoryFile(Tree tree)
        {
            System.Console.WriteLine("Create story file ...");
            CodeGen.ZStoryFile storyFile = new CodeGen.ZStoryFile();

            System.Console.WriteLine("Add instructions to story file ...");
            string text = tree.StartPassage.PassageContentList.ElementAt(0).PassageText.Text;
            storyFile.SetupHelloWorldDemo(text); // TODO austauschen gegen richtige MEthode

            return storyFile;
        }

        static void WriteStoryFile(CodeGen.ZStoryFile storyFile, string output)
        {
            System.Console.WriteLine("Save story file ...");
            File.WriteAllBytes(output, storyFile.ToBytes());
            System.Console.WriteLine("The story file has been saved at:");
            System.Console.WriteLine(System.IO.Path.GetFullPath(zStroyFile));
        }
    }
}
