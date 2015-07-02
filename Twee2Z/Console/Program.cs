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

        enum RunCase
        {
            Tw2Z,
            Help
        }

        static void Main(string[] args)
        {
            // init logger. modify if needed
            Logger.AddLogEvent(Logger.LogEvent.UserOutput);
            Logger.AddLogEvent(Logger.LogEvent.Warning);
            Logger.AddLogEvent(Logger.LogEvent.Error);
            Logger.UseConsoleLogWriter();
            string logOutput = "defaultLog.txt";
            if (File.Exists(logOutput))
            {
                File.Delete(logOutput);
            }
            Logger.LogUserOutput("See default log at: " + System.IO.Path.GetFullPath(logOutput));
            Logger.AddLogWriter(new LogWriter(new StreamWriter(logOutput)));
            // end init logger


            int argCounter = 0;
            string arg0 = null;
            string arg1 = null;
            RunCase runCase = RunCase.Help;


            if (args.Length == 0)
            {
                PrintHelp();
            }
            else
            {
                while (argCounter < args.Length)
                {
                    switch (args[argCounter++].ToLower())
                    {
                        case "-tw2z":
                            if (argCounter + 2 > args.Length)
                            {
                                Logger.LogError("Invalid arguments. '-tw2z' needs 2 argements.");
                                runCase = RunCase.Help;
                            }
                            else
                            {
                                runCase = RunCase.Tw2Z;
                                arg0 = args[argCounter++];
                                arg1 = args[argCounter++];
                            }
                            break;
                        case "-help":
                            runCase = RunCase.Help;
                            break;
                        case "-logall":
                            Logger.LogAll();
                            break;
                        case "-log":
                            while (argCounter < args.Length && args[argCounter].Substring(0, 1) != "-")
                            {
                                switch (args[argCounter++].ToLower())
                                {
                                    case "all":
                                        Logger.LogAll();
                                        break;
                                    case "analyzer":
                                        Logger.AddLogEvent(Logger.LogEvent.Analyzer);
                                        break;
                                    case "codegen":
                                        Logger.AddLogEvent(Logger.LogEvent.CodeGen);
                                        break;
                                    case "debug":
                                        Logger.AddLogEvent(Logger.LogEvent.Debug);
                                        break;
                                    case "objecttree":
                                        Logger.AddLogEvent(Logger.LogEvent.ObjectTree);
                                        break;
                                    case "validation":
                                        Logger.AddLogEvent(Logger.LogEvent.Validation);
                                        break;
                                    default:
                                        PrintHelp();
                                        argCounter = args.Length;
                                        break;
                                }
                            }
                            break;
                        default:
                            Logger.LogError("Invalid argument: " + args[argCounter - 1]);
                            runCase = RunCase.Help;
                            argCounter = args.Length;
                            break;
                    }
                }
            }
            
            switch (runCase)
            {
                case RunCase.Tw2Z:
                    Compile(arg0, arg1);
                    break;
                case RunCase.Help:
                    PrintHelp();
                    break;
            }
            Logger.LogUserOutput("Press any key to contiune");
            System.Console.ReadKey(true);
        }


        static void Compile(string from, string output)
        {
            Logger.LogUserOutput("Open twee file: " + from);
            FileStream tweeFileStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read);

            Tree tree = AnalyseFile(tweeFileStream);
            if(ValidateTree(tree))
            {
                File.WriteAllBytes(output, GenStoryFile(tree).ToBytes());
            }
            else
            {
                Logger.LogError("Stop compiling");
            }
        }

        public static Tree AnalyseFile(FileStream stream)
        {
            Logger.LogUserOutput("Start analyzer ...");
            /*Add a new line at the beginnig of the Stream, because of LEX passage starting rule PASS*/
            String str = new StreamReader(stream).ReadToEnd();
            str = "\r\n" + str;

            Stream s = new MemoryStream();
            StreamWriter writer = new StreamWriter(s);
            writer.Write(str);
            writer.Flush();
            s.Position = 0;

            System.Console.WriteLine();

            return TweeAnalyzer.Parse(new StreamReader(s));
        }

        public static bool ValidateTree(Tree tree)
        {
            TreeValidator validator = new TreeValidator(tree);
            return validator.ValidateTree();
        }

        public static CodeGen.ZStoryFile GenStoryFile(Tree tree)
        {
            Logger.LogUserOutput("Create story file ...");
            CodeGen.ZStoryFile storyFile = new CodeGen.ZStoryFile();

            Logger.LogUserOutput("Add instructions to story file ...");
            storyFile.ImportObjectTree(tree);

            return storyFile;
        }

        public static void PrintHelp()
        {

            Logger.LogUserOutput("");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("                            **** Help ****");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-tw2z     The code from the source language Twee will be translated to Z8 code.");
            Logger.LogUserOutput("          Input   : -tw2z <source> <destination>");
            Logger.LogUserOutput("          Example : -tw2z myTwee.tw zfile.z8");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-logAll   Activate all logs.");
            Logger.LogUserOutput("-log      Activate specific logs");
            Logger.LogUserOutput("          Possible arguments:");
            Logger.LogUserOutput("          all, analyzer, codegen, debug, objecttree, validation");
            Logger.LogUserOutput("          Input   : -log <arg1> <arg2> ... ");
            Logger.LogUserOutput("          Example : -log analyzer debug");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-help     Display help message.");
            Logger.LogUserOutput("          Example usage: -tw2z myTwee.tw zfile.z8 -logAll");
        }
    }
}
