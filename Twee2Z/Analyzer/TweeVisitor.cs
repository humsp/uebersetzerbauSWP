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

        public override object VisitPassageTags(TweeParser.PassageTagsContext context)
        {
            Console.WriteLine("[Tags]");
            return base.VisitPassageTags(context);
        }

        public override object VisitInnerPassageTag(TweeParser.InnerPassageTagContext context)
        {
            if (context.ChildCount != 0)
            {
                string tag = context.GetChild(0).GetText();

                Console.WriteLine("Tag: " + tag);
                _currentPassage.AddTag(tag);

            }
            return base.VisitInnerPassageTag(context);
        }


        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            Console.WriteLine("[PassageContent]");
            return base.VisitPassageContent(context);
        }

        public override object VisitLink(TweeParser.LinkContext context)
        {
            Console.WriteLine("[Link]");
            if (context.ChildCount == 6)
            {
                string passageName = context.GetChild(3).GetText();
                Console.WriteLine("Ziel: " + passageName);
                Console.WriteLine("Text: " + context.GetChild(1).GetText());
                if (context.GetChild(1).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText()));
            }
            else if (context.ChildCount == 9)
            {
                string passageName = context.GetChild(3).GetText();
                Console.WriteLine("Ziel: " + passageName);
                Console.WriteLine("Text: " + context.GetChild(1).GetText());
                Console.WriteLine("Expression: " + context.GetChild(6).GetText());
                if (context.GetChild(1).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText(), context.GetChild(6).GetText()));
            }
            else
            {
                Console.WriteLine("Ziel: " + context.GetChild(1).GetText());
                _currentPassage.AddPassageContent(new PassageLink(context.GetChild(1).GetText()));
            }
            return base.VisitLink(context);
        }

        public override object VisitText(TweeParser.TextContext context)
        {
            Console.WriteLine("Text: " + context.GetText());
            _currentPassage.AddPassageContent(new PassageText(context.GetText()));
            return base.VisitText(context);
        }

        public override object VisitVariable(TweeParser.VariableContext context)
        {
            Console.WriteLine("Variable: " + context.GetText());
            _tree.AddVariable(new Variable(context.GetText()));
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
