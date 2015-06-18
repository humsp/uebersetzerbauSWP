using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;

namespace Twee2Z.Analyzer
{
    class TweeVisitor : TweeBaseVisitor<object>
    {
        private Tree _tree;
        private Passage _currentPassage;

        //Text Formatierung

        public override object VisitStart(Twee.StartContext context)
        {
            _tree = new Tree();
            Logger.LogAnalyzer("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(Twee.PassageContext context)
        {
            Logger.LogAnalyzer("[Passage]");
            string[] tags;
            String Name = context.GetChild(1).GetText();
           
            for (int i = Name.Length - 1; i > 0; i--)       //Leerzeichen nach Passname entfernen.
            {
                if (Name[i].Equals(' '))
                {
                    Name = Name.Substring(0, Name.Length - 1);
                }
            }
            Logger.LogAnalyzer("Name: " + Name);
            _currentPassage = new Passage(Name);
            _tree.AddPassage(_currentPassage);
            
            for (int i = 0; i < context.ChildCount; i++)  //Tag Auseinanderbauen und Speichern
            {
                if (context.GetChild(i).GetText().Contains('['))
                {
                    if (!(context.GetChild(2).GetText().Contains('\n')))
                    {
                        tags = new string[context.GetChild(2).GetText().Split(' ').Length];
                        tags = context.GetChild(2).GetText().Split(' ');
                        for (int j = 0; j < tags.Length; j++)
                        {
                            tags[j] = tags[j].Replace("[", "").Replace("]", "");
                            if (!tags[j].Equals(""))
                            {
                                Logger.LogAnalyzer("[PassageTag] = " + tags[j]);
                            }
                        }
                    }
                }
            }
            return base.VisitPassage(context);
        }

        public override object VisitPassageContent(Twee.PassageContentContext context)
        {
            return base.VisitPassageContent(context);
        }


        /**
         * This function is called if a link is read inside a twee file.
         * The function displays the target, displayed text and the expression
         * Possible Link structures: [[target]] ; [[text|target]] ; [[text|target][expression]]
         * 
         * Parameters which are passed to the ObjectTree:
         * @Ziel       : The 'ziel' is a valid passage Name where the link is referencing. [[target]]
         * @Text       : displayed text of the link [[text|target]]
         * @Expression : expression which is executed when Link is clicked
         **/
        public override object VisitLink(Twee.LinkContext context)
        {
            Logger.LogAnalyzer("[Link]");
            string Ziel = "";
            string Text = "";
            string Expression = "";

            // case: [[text|target]]
            if (context.ChildCount == 5)
            {
                Ziel = context.GetChild(3).GetText();
                Text = context.GetChild(1).GetText();
                Logger.LogAnalyzer("Ziel: " + Ziel);
                Logger.LogAnalyzer("Text: " + Text);
                if (Ziel == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Text, false));
            }

            // case: [[target][expression]]
            else if (context.ChildCount == 6)
            {
                Ziel = context.GetChild(1).GetText();
                Expression = context.GetChild(4).GetText();
                Logger.LogAnalyzer("Ziel: " + Ziel);
                Logger.LogAnalyzer("Expression: " + Expression);
                if (Ziel == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Expression, true));
            }

            // case: [[text|target][expression]]
            else if (context.ChildCount == 8)
            {
                Ziel = context.GetChild(3).GetText();
                Text = context.GetChild(1).GetText();
                Expression = context.GetChild(6).GetText();
                Logger.LogAnalyzer("Ziel: " + Ziel);
                Logger.LogAnalyzer("Text: " + Text);
                Logger.LogAnalyzer("Expression: " + Expression);
                if (context.GetChild(2).GetText() == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Text, Expression));
            }

            // case: [[target]]
            else
            {
                Ziel = context.GetChild(1).GetText();
                Logger.LogAnalyzer("Ziel: " + Ziel);
                _currentPassage.AddPassageContent(new PassageLink(Ziel));
            }
            return base.VisitLink(context);
        }

        /**
         * This function is called if a plain text is read inside a twee file.
         * Plain text is everything except for macros, functions, variables, links,
         * tags, format
         * It displays the          
         * 
         * Parameter which is passed to the ObjectTree:
         * @context.GetText() : Whole plain text
         **/
        public override object VisitText(Twee.TextContext context)
        {
            String Text = context.GetChild(0).GetText();
            if (!Text.Equals(""))  /*Solange nicht leer*/
            {
                if (!(Text.Equals("\r\n")  /*Solange nicht NL oder Space, wirds geprintet*/
                    || Text.Equals("\n")
                    || Text.Equals("\r")
                    || Text.Equals(" ")))
                {
                    switch (Text)
                    {
                        case "{{{": ObjectTree.PassageContent.Monospace = true; break;
                        case "}}}": ObjectTree.PassageContent.Monospace = false; break;
                        case "''": ObjectTree.PassageContent.Bold = !ObjectTree.PassageContent.Bold; break;
                        case "//": ObjectTree.PassageContent.Italic = !ObjectTree.PassageContent.Italic; break;
                        case "__": ObjectTree.PassageContent.Underline = !ObjectTree.PassageContent.Underline; break;
                        case "==": ObjectTree.PassageContent.Strikeout = !ObjectTree.PassageContent.Strikeout; break;
                        case "^^": ObjectTree.PassageContent.Superscript = !ObjectTree.PassageContent.Superscript; break;
                        case "~~": ObjectTree.PassageContent.Subscript = !ObjectTree.PassageContent.Subscript; break;
                        case "/%": ObjectTree.PassageContent.Comment = true; break;
                        case "%/": ObjectTree.PassageContent.Comment = false; break;
                        default: 
                            Logger.LogAnalyzer("Text: " + Text);
                            _currentPassage.AddPassageContent(new PassageText(Text));break;
                    }
                }
                else { _currentPassage.AddPassageContent(new PassageText(Text)); }
            }
            return base.VisitText(context);
        }

        /**
         * This function is called if a variable is read inside a twee file.
         * It displays the read variable and passes the name and value to the objecttree
         * 
         * Parameter which is passed to the ObjectTree:
         * @context.GetText(): Name of the Variable
         * @Value            : value of variable. Currently only Integer, String or Boolean
         * 
         **/
        public override object VisitVariable(Twee.VariableContext context)
        {
            String VarName = context.GetText();
            Logger.LogAnalyzer("\nVariable: " + VarName);
            _currentPassage.AddPassageContent(new PassageVariable(VarName, 0));
            return base.VisitVariable(context);
        }

        /**
         * This function is called if a function is read inside a twee file
         * It displays the read function in the console and passes name and parameter 
         * to the objecttree
         * 
         * Parameter which are passed to the Objecttree
         * @function name
         * @parameterlist
         **/
        public override object VisitFunction(Twee.FunctionContext context)
        {
            String Funktion = context.GetText();
            Logger.LogAnalyzer("Function: " + Funktion);
            // substring = str.Split(',')[0]; das könnt ihr dafür nutzen, die einzelne Argumente zu extrahieren 
            // PassgeFunction.addArg funktion steht euch zur Verfügung.
            return base.VisitFunction(context);
        }

        /**
         * This function is called if a macro is read inside a twee file
         * It displays the read macro in the console.
         **/
        public override object VisitMacro(Twee.MacroContext context)
        {
            String Macro = context.GetText();
            Logger.LogAnalyzer("Macro: " + Macro);
            return base.VisitMacro(context);
        }

        public Tree Tree
        {
            get
            {
                return _tree;
            }
        }
    }
}
