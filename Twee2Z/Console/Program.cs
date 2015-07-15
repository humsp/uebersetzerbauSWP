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
            System.Console.WriteLine("Twee2Z 1.0");
            System.Console.WriteLine("");

            // init logger. modify if needed
            Logger.AddLogEvent(Logger.LogEvent.UserOutput);
            Logger.AddLogEvent(Logger.LogEvent.Warning);
            Logger.AddLogEvent(Logger.LogEvent.Error);
            Logger.UseConsoleLogWriter();
            string logOutput = "log.txt";
            if (File.Exists(logOutput))
            {
                File.Delete(logOutput);
            }
            //Logger.LogUserOutput("See log at: " + System.IO.Path.GetFullPath(logOutput));
            Logger.AddLogWriter(new LogWriter(new StreamWriter(logOutput)));
            // end init logger
            
            int argCounter = 0;
            string arg = null;
            string tweeFile = null;
            string storyFile = null;
            RunCase runCase = RunCase.Tw2Z;

            try
            {
                tweeFile = args[0];
                storyFile = args[1];
                argCounter = 2;

                while (argCounter < args.Length)
                {
                    switch (args[argCounter++].ToLower())
                    {
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
            catch (Exception)
            {
                runCase = RunCase.Help;
            }
            
            switch (runCase)
            {
                case RunCase.Tw2Z:
                    Compile(tweeFile, storyFile);
                    break;
                case RunCase.Help:
                    PrintHelp();
                    break;
            }

            if (System.Diagnostics.Debugger.IsAttached)
            { 
                Logger.LogUserOutput("Press any key to continue ...");
                System.Console.ReadKey(true);
            }
        }


        static void Compile(string from, string output)
        {
            Logger.LogUserOutput("Open twee file: " + from);
            FileStream tweeFileStream = new FileStream(from, FileMode.Open);

            Tree tree = AnalyseFile(tweeFileStream);
            if(ValidateTree(tree))
            {
                try
                {
                    byte[] storyfile = GenStoryFile(tree).ToBytes();
                    Logger.LogUserOutput("Save story file: " + from);
                    File.WriteAllBytes(output, storyfile);
                }
                catch (Exception)
                {
                }
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
            Logger.LogUserOutput("-logAll   Activate all logs.");
            Logger.LogUserOutput("");
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
