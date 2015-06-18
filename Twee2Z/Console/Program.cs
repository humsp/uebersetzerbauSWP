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
            RunZ,
            RunTw,
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
                        case "-runz":
                            if (argCounter + 1 > args.Length)
                            {
                                Logger.LogError("Invalid arguments. '-run' needs 1 argement.");
                                runCase = RunCase.Help;
                            }
                            else
                            {
                                runCase = RunCase.RunZ;
                                arg0 = args[argCounter++];
                            }
                            break;
                        case "-runtw":
                            runCase = RunCase.RunTw;
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
                case RunCase.RunTw:
                    //System.Console.WriteLine("Run twee-code");
                    break;
                case RunCase.RunZ:
                    //System.Console.WriteLine("Run ZCode");
                    ZCodeInterpreter.Interpreter.Run(arg0);
                    break;
                case RunCase.Help:
                    PrintHelp();
                    break;
            }

            //Logger.LogUserOutput("Press any key to contiune");
            //System.Console.ReadKey(true);
        }


        static void Compile(string from, string output)
        {
            Logger.LogUserOutput("Open twee file: " + from);
            FileStream tweeFileStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read);

            File.WriteAllBytes(output, GenStoryFile(AnalyseFile(tweeFileStream)).ToBytes());
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
            storyFile.ImportObjectTree(tree);

            return storyFile;
        }

        static void WriteStoryFile(CodeGen.ZStoryFile storyFile, string output)
        {
            Logger.LogUserOutput("Save story file in:\n" + System.IO.Path.GetFullPath(zStroyFile));
            File.WriteAllBytes(output, storyFile.ToBytes());
        }

        public static void PrintHelp()
        {

            Logger.LogUserOutput("");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("                            **** Help ****");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-tw2z     Der Code in der Quellsprache Twee wird zu Z Code übersetzt,");
            Logger.LogUserOutput("          und in der entsprechende Path gespeichert.");
            Logger.LogUserOutput("          Bsp Eingabe :  -tw2z bsp.tw zfile.z1");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-runZ     Als Eingabe wird ein ZCode eingegeben und über Z-Maschine ausgeführt.");
            Logger.LogUserOutput("          Bsp Eingabe :  -runZ zfile.z1");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-runTw    Ein Twee Code wird compiliert und über Z Machine ausgeführt");
            Logger.LogUserOutput("          Bsp Eingabe :  -runTw txtadv.tw");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-logAll   Um alle Logs zu aktivieren");
            Logger.LogUserOutput("-log      Um spezielle Logs zu aktivieren.");
            Logger.LogUserOutput("          Hierzu beliebige weiteren Argumente anhängen:");
            Logger.LogUserOutput("          all, analyzer, codegen, debug, objecttree, validation");
            Logger.LogUserOutput("");
            Logger.LogUserOutput("-help     Beschreibung der Funktionen ");
        }

        public static bool checkPath(string path)
        {

            if (Directory.Exists(path))
            {
                return true;
            }
            Logger.LogUserOutput("Der Path " + path + "existiert nicht, Prüfen Sie es nochmal");
            return false;
        }
    }
}
