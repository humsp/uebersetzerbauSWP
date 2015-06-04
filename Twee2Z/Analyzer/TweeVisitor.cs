using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.ObjectTree;

namespace Twee2Z.Analyzer
{
     class TweeVisitor : TweeParserBaseVisitor<object>
    {
        static public short link_flag = 0;
        private Tree _tree = new Tree();
        private Passage _currentPassage;

        public override object VisitStart(TweeParser.StartContext context)
        {
            new Tree();
            Console.WriteLine("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(TweeParser.PassageContext context)
        {
            Console.WriteLine("[Passage]");

            return base.VisitPassage(context);
        }

        public override object VisitPassageName(TweeParser.PassageNameContext context)
        {
            Console.WriteLine("Name: " + context.GetText());
            _currentPassage = new Passage(context.GetText());
            _tree.AddPassage(_currentPassage);
            return base.VisitPassageName(context);
        }       

        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            Console.WriteLine("[PassageContent]");
            return base.VisitPassageContent(context);
        }
        public override object VisitLink(TweeParser.LinkContext context)
        {
            Console.WriteLine("[Link]");
            if (context.ChildCount == 7)
            {

                string passageName = context.GetChild(4).GetText();
                Console.WriteLine("Ziel: " + passageName);
                Console.WriteLine("Text: " + context.GetChild(2).GetText());
                if (context.GetChild(1).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText(), false));
            }
            else if (context.ChildCount == 8)
            {
                string passageName = context.GetChild(2).GetText();
                Console.WriteLine("Ziel: " + passageName);
                Console.WriteLine("Expression: " + context.GetChild(5).GetText());
                if (context.GetChild(2).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(3).GetText(), true));
            }
            else if (context.ChildCount == 10)
            {
                string passageName = context.GetChild(4).GetText();
                Console.WriteLine("Ziel: " + passageName);
                Console.WriteLine("Text: " + context.GetChild(2).GetText());
                Console.WriteLine("Expression: " + context.GetChild(7).GetText());
                if (context.GetChild(2).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText(), context.GetChild(6).GetText()));
            }
            else
            {
                Console.WriteLine("Ziel: " + context.GetChild(0).ChildCount);
               // _currentPassage.AddPassageContent(new PassageLink(context.GetChild(2).GetText()));
            }
            return base.VisitLink(context);
        }
        public override object VisitText(TweeParser.TextContext context)
        {
            if (!context.GetText().Equals("\r\n")) {
                Console.WriteLine("Text: " + context.GetText());
            }
            _currentPassage.AddPassageContent(new PassageText(context.GetText()));
            return base.VisitText(context);
        }



        public override object VisitVariable(TweeParser.VariableContext context)
        {
            Console.WriteLine("Variable: " + context.GetText());
            _tree.SetVariable(new Variable(context.GetText()));
            return base.VisitVariable(context);
        }

        public override object VisitFunction(TweeParser.FunctionContext context)
        {
            Console.WriteLine("Function: " + context.GetText());
            return base.VisitFunction(context);
        }

        public override object VisitMacro(TweeParser.MacroContext context)
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
