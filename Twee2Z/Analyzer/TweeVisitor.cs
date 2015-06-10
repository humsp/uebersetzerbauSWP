using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.ObjectTree;

namespace Twee2Z.Analyzer
{
     class TweeVisitor : TweeBaseVisitor<object>
    {
        static public short link_flag = 0;
        private Tree _tree = new Tree();
        private Passage _currentPassage;

        public override object VisitStart(Twee.StartContext context)
        {
            new Tree();
            Console.WriteLine("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(Twee.PassageContext context)
        {
            Console.WriteLine("[Passage]");

            return base.VisitPassage(context);
        }

        public override object VisitPassageName(Twee.PassageNameContext context)
        {
            Console.WriteLine("Name: " + context.GetText());
            _currentPassage = new Passage(context.GetText());
            _tree.AddPassage(_currentPassage);
            return base.VisitPassageName(context);
        }       

        public override object VisitPassageContent(Twee.PassageContentContext context)
        {                    
            return base.VisitPassageContent(context);
        }
        public override object VisitPassageTags(Twee.PassageTagsContext context)
        {
            string[] tags = new string[context.GetText().Split(' ').Length];
            tags = context.GetText().Split(' ');
            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].Replace("[", "").Replace("]", "");
                if (!tags[i].Equals("")) { 
                    //Speichern hier in ObjectTree!
                    Console.WriteLine("[PassageTag] = " + tags[i]); }
            }
                
            return base.VisitPassageTags(context);
        }

        public override object VisitLink(Twee.LinkContext context)
        {
            Console.WriteLine("[Link]");
            /*Hier OBJECTTREE:*/
            string Ziel = "";
            string Text = "";
            string Expression = "";

            if (context.ChildCount == 5)
            {

                Ziel = context.GetChild(3).GetText();
                Text = context.GetChild(1).GetText();
                Console.WriteLine("Ziel: " + Ziel);
                Console.WriteLine("Text: " + Text);
                if (Ziel == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Text, false));
            }
            else if (context.ChildCount == 6)
            {
                Ziel = context.GetChild(1).GetText();
                Expression = context.GetChild(4).GetText();
                Console.WriteLine("Ziel: " + Ziel);
                Console.WriteLine("Expression: " + Expression);
                if (Ziel == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Expression, true));
            }
            else if (context.ChildCount == 8)
            {
                Ziel = context.GetChild(3).GetText();
                Text = context.GetChild(1).GetText();
                Expression = context.GetChild(6).GetText();
                Console.WriteLine("Ziel: " + Ziel);
                Console.WriteLine("Text: " + Text);
                Console.WriteLine("Expression: " + Expression);
                if (context.GetChild(2).GetText() == "")
                {
                    throw new Exception("passage text empty:" + Ziel);
                }
                _currentPassage.AddPassageContent(new PassageLink(Ziel, Text, Expression));
            }
            else
            {
                Ziel = context.GetChild(1).GetText();
                Console.WriteLine("Ziel: " + Ziel);
                _currentPassage.AddPassageContent(new PassageLink(Ziel));
            }
            return base.VisitLink(context);
        }
        public override object VisitText(Twee.TextContext context)
        {
            if (!context.GetText().Equals("\r\n"))
            {
                if (!context.GetText().Equals(""))
                {
                    Console.Write(context.GetChild(0).GetText());
                }
                else
                {
                    Console.WriteLine("Text: " + context.GetChild(0).GetText());
                }
                    
            }
            
            _currentPassage.AddPassageContent(new PassageText(context.GetText()));
            return base.VisitText(context);
        }

        public override object VisitVariable(Twee.VariableContext context)
        {
            Console.WriteLine("\nVariable: " + context.GetText());
            _tree.SetVariable(new Variable(context.GetText()));
            return base.VisitVariable(context);
        }

        public override object VisitFunction(Twee.FunctionContext context)
        {
            Console.WriteLine("Function: " + context.GetText());
            return base.VisitFunction(context);
        }

       /* public override object VisitFormat(Twee.FormatContext context)
        {
            switch(context.GetChild(0).GetText()) {
                 
                case "{{{": PassageText.Monospace    = true;
                case "/*": PassageText.Comment = true;
                case "}}}": PassageText.Monospace = false;
                case "*/ //": PassageText.Comment = false;
         /*
                case "~~":  PassageText.Subscript = !PassageText.Subscript;
                case "//": PassageText.Italic = !PassageText.Italic;
                case "__": PassageText.Underline = !PassageText.Underline;
                case "==": PassageText.Strikeout = !PassageText.Strikeout;
                case "^^": PassageText.Superscript = !PassageText.Superscript;
                case "''": PassageText.Bold = !PassageText.Bold;
            }

            new PassageText(MAGIC);
            Console.WriteLine("Format: " + context.GetText());
            return base.VisitFormat(context);
        }*/

        public override object VisitMacro(Twee.MacroContext context)
        {
            Console.WriteLine("Macro: " + context.GetText());
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
