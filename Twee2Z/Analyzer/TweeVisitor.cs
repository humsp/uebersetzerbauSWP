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

        public override object VisitStart(TweeParser.StartContext context)
        {
            _tree = new Tree();
            Logger.LogAnalyzer("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(TweeParser.PassageContext context)
        {
            Logger.LogAnalyzer("[Passage]");
            return base.VisitPassage(context);
        }

        public override object VisitPassageName(TweeParser.PassageNameContext context)
        {
            Logger.LogAnalyzer("Name: " + context.GetText());
            _currentPassage = new Passage(context.GetText());
            _tree.AddPassage(_currentPassage);
            return base.VisitPassageName(context);
        }

        public override object VisitPassageTags(TweeParser.PassageTagsContext context)
        {
            Logger.LogAnalyzer("[Tags]");
            return base.VisitPassageTags(context);
        }

        public override object VisitInnerPassageTag(TweeParser.InnerPassageTagContext context)
        {
            if (context.ChildCount != 0)
            {
                string tag = context.GetChild(0).GetText();

                Logger.LogAnalyzer("Tag: " + tag);
                _currentPassage.AddTag(tag);

            }
            return base.VisitInnerPassageTag(context);
        }


        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            Logger.LogAnalyzer("[PassageContent]");
            return base.VisitPassageContent(context);
        }

        public override object VisitLink(TweeParser.LinkContext context)
        {
            Logger.LogAnalyzer("[Link]");
            if (context.ChildCount == 6)
            {
                string passageName = context.GetChild(3).GetText();
                Logger.LogAnalyzer("Ziel: " + passageName);
                Logger.LogAnalyzer("Text: " + context.GetChild(1).GetText());
                if (context.GetChild(1).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                if (context.GetChild(3).GetText() == "")
                {
                    throw new Exception("passage name empty:" + context.GetChild(3));
                }

                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText()));
            }
            else if (context.ChildCount == 9)
            {
                string passageName = context.GetChild(3).GetText();
                Logger.LogAnalyzer("Ziel: " + passageName);
                Logger.LogAnalyzer("Text: " + context.GetChild(1).GetText());
                Logger.LogAnalyzer("Expression: " + context.GetChild(6).GetText());
                if (context.GetChild(1).GetText() == "")
                {
                    throw new Exception("passage text empty:" + passageName);
                }
                _currentPassage.AddPassageContent(new PassageLink(passageName, context.GetChild(1).GetText(), context.GetChild(6).GetText()));
            }
            else
            {
                Logger.LogAnalyzer("Ziel: " + context.GetChild(1).GetText());
                _currentPassage.AddPassageContent(new PassageLink(context.GetChild(1).GetText()));
            }
            return base.VisitLink(context);
        }

        public override object VisitText(TweeParser.TextContext context)
        {
            Logger.LogAnalyzer("Text: " + context.GetText());
            _currentPassage.AddPassageContent(new PassageText(context.GetText()));

            return base.VisitText(context);
        }

        public override object VisitVariable(TweeParser.VariableContext context)
        {
            Logger.LogAnalyzer("Variable: " + context.GetText());
            _currentPassage.AddPassageContent(new PassageVariable(context.GetText(), 0));
            //_tree.SetVariable(new Variable(context.GetText()));
            return base.VisitVariable(context);
        }
        public override object VisitFunction(TweeParser.FunctionContext context)
        {
            Logger.LogAnalyzer("Function: " + context.GetText());
   // substring = str.Split(',')[0]; das könnt ihr dafür nutzen, die einzelne Argumente zu extrahieren 
   // PassgeFunction.addArg funktion steht euch zur Verfügung.
            return base.VisitFunction(context);
        }
        public override object VisitMacro(TweeParser.MacroContext context)
        {
            Logger.LogAnalyzer("Macro: " + context.GetText());
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
