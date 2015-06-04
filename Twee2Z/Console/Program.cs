using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;

namespace Twee2Z.Console
{
    public class Program
    {
        const string tweeFile = "passage.tw";
        const string zStroyFile = "storyfile.z8";

        static void Main(string[] args)
        {
            // init logger. modify if needed
            Logger.LogAll();
            Logger.UseConsoleLogWriter();
            string logOutput = "log.txt";
            if(File.Exists(logOutput)){
                File.Delete(logOutput);
            }
            System.Console.WriteLine("See log at: " + System.IO.Path.GetFullPath(logOutput));
            Logger.AddLogWriter(new LogWriter(new StreamWriter(logOutput)));
            // end init logger


            Complie(args[0], zStroyFile);

            Logger.LogUserOutput("Done. Have a nice day!");
            System.Console.ReadKey(true);
        }


        static void Complie(string from, string output)
        {
            Logger.LogUserOutput("Open twee file: " + from);
            FileStream tweeFileStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read);
            Tree tree = AnalyseFile(tweeFileStream);
            ValidateTree(tree);
            CodeGen.ZStoryFile storyFile = GenStoryFile(tree);
            WriteStoryFile(storyFile, output);
        }

        public static Tree AnalyseFile(FileStream stream)
        {
            Logger.LogUserOutput("Start analyzer ...");
            return TweeAnalyzer.Parse(new StreamReader(stream));
        }

        public static void ValidateTree(Tree tree)
        {
            TreeValidator validator = new TreeValidator(tree);
            validator.ValidateTree();
        }

        public static CodeGen.ZStoryFile GenStoryFile(Tree tree)
        {
            Logger.LogUserOutput("Create story file ...");
            CodeGen.ZStoryFile storyFile = new CodeGen.ZStoryFile();

            Logger.LogUserOutput("Add instructions to story file ...");
            string text = tree.StartPassage.PassageContentList.ElementAt(0).PassageText.Text;
            storyFile.SetupHelloWorldDemo(text); // TODO austauschen gegen richtige MEthode

            return storyFile;
        }

        static void WriteStoryFile(CodeGen.ZStoryFile storyFile, string output)
        {
            Logger.LogUserOutput("Save story file in:\n" + System.IO.Path.GetFullPath(zStroyFile));
            File.WriteAllBytes(output, storyFile.ToBytes());
        }
    }
}
