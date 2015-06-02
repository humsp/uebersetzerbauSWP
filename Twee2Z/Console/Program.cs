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
            int fall = 0;
            if (args.Length == 0)
            {
                help();
                Console.WriteLine("");
                Console.WriteLine("Drücken Sie eine Taste um den Program zu terminieren");
                Console.ReadKey();
            }
            else
            {

                if (args[0] == "-tw2Z") fall = 1;
                if (args[0] == "-runZ") fall = 2;
                if (args[0] == "-runTw") fall = 3;
                if (args.Contains("-help")) fall = 4;

                switch (fall)
                {
                    case 1:
                        while (checkPath(args[1]))
                        {
                            Compile(args[1], args[2]);
                        }
                        break;
                    case 2:
                        Console.WriteLine("Case 2");
                        break;
                    case 3:
                        Console.WriteLine("Case 3");
                        break;
                    case 4:
                        help();
                        break;
                    default:
                        Console.WriteLine("Ihre Eingabe ist fehlerhaft");
                        help();
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
        public static void help()
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

        public static bool checkPath(string path)
        {

            if (Directory.Exists(path)) return true;
            else Console.WriteLine("Der Path {0} existiert nicht, Prüfen Sie es nochmal", path); return false;
        }
    }
}
