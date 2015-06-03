using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using Twee2Z.Analyzer;
using Twee2Z.ObjectTree;

namespace Twee2Z.Konsole
{
    public class Program
    {
        const string tweeFile = "passage.tw";
        const string zStroyFile = "storyfile.z8";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintHelp();
                Console.WriteLine("");
                Console.WriteLine("Drücken Sie eine beliebige Taste ...");
                Console.ReadKey();
            }
            else
            {
                switch (args[0].ToLower())
                {
                    case "-tw2z":
                        Compile(args[1], args[2]);
                        break;
                    case "-runz":
                        Console.WriteLine("Case 2");
                        break;
                    case "-runTw":
                        Console.WriteLine("Case 3");
                        break;
                    case "-help":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine("Ihre Eingabe ist fehlerhaft");
                        PrintHelp();
                        break;
                }
            }
        }
        
        static void Compile(string from, string output)
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

        public static void PrintHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("                            **** Help ****");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("-tw2z     Der Code in der Quellsprache Twee wird zu Z Code übersetzt,");
            Console.WriteLine("          und in der entsprechende Path gespeichert.");
            Console.WriteLine("          Bsp Eingabe :  -tw2z bsp.tw zfile.z1");
            Console.WriteLine("");
            Console.WriteLine("-runZ     Als Eingabe wird ein ZCode eingegeben und über Z-Maschine ausgeführt.");
            Console.WriteLine("          Bsp Eingabe :  -runZ zfile.z1");
            Console.WriteLine("");
            Console.WriteLine("-runTw    Ein Twee Code wird compiliert und über Z Machine ausgeführt");
            Console.WriteLine("          Bsp Eingabe :  -runTw txtadv.tw");
            Console.WriteLine("");
            Console.WriteLine("-help     Beschreibung der Funktionen ");
            Console.WriteLine("Bitte beachten Sie Klein- und Großschreibung");
        }
    }
}
