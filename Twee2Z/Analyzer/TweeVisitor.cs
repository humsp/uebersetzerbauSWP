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
		public Passage aktuelle;
		public PassageNode content;
        public override object VisitStart(TweeParser.StartContext context)
        {
            new Tree();
            Console.WriteLine("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(TweeParser.PassageContext context)
        {
            Console.WriteLine("[Passage]");
            aktuelle = new Passage();
            return base.VisitPassage(context);
        }

        public override object VisitPassageName(TweeParser.PassageNameContext context)
        {
            Console.WriteLine("Name: " + context.GetText());
            aktuelle.name = context.GetText();
            return base.VisitPassageName(context);
        }

        public override object VisitPassageTags(TweeParser.PassageTagsContext context)
        {
            Console.WriteLine("Tags: " + context.GetText());
            aktuelle.tags = new Tags(context.GetText());
            return base.VisitPassageTags(context);
        }

        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            if (context.GetChild(0) is TweeParser.TextContext)
            {
                if (context.GetChild(0).GetText() != System.Environment.NewLine)
                    aktuelle.text = context.GetChild(0).GetText();
                Console.WriteLine(context.GetChild(0).GetText());
            }
            return base.VisitPassageContent(context);
        }

        public override object VisitLink(TweeParser.LinkContext context)
        {
            Console.WriteLine("[Link]");
            if (context.ChildCount == 6)
            {
                Console.WriteLine("Ziel: " + context.GetChild(3).GetText());
                Console.WriteLine("Text: " + context.GetChild(1).GetText());
                aktuelle.links.Add(new Link(context.GetChild(3).GetText(), context.GetChild(1).GetText()));
            }
            else if (context.ChildCount == 9)
            {
                Console.WriteLine("Ziel: " + context.GetChild(3).GetText());
                Console.WriteLine("Text: " + context.GetChild(1).GetText());
                Console.WriteLine("Expression: " + context.GetChild(6).GetText());
                aktuelle.links.Add(new Link(context.GetChild(3).GetText(), context.GetChild(1).GetText(), context.GetChild(6).GetText()));
            }
            else
            {
                Console.WriteLine("Ziel: " + context.GetChild(1).GetText());
                aktuelle.links.Add(new Link(context.GetChild(1).GetText()));
            }
            return base.VisitLink(context);
        }
        
        public override object VisitText(TweeParser.TextContext context)
        {
            return base.VisitText(context);
        }

        public override object VisitVariable(TweeParser.VariableContext context)
        {
            Console.WriteLine("Variable: " + context.GetText());
            new Variable(context.GetText());
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

		/*public void createPassageNameNode (){
			object n = VisitPassageName(TweeParser.PassageNameContext context);
			PassageNode passagename = new PassageNode (n);
			aktuelle.kids.Add(passagename);

		}

		public void createPassageTagsNode(){
			object n = VisitPassageTags(TweeParser.PassageTagsContext context);
			PassageNode passageTags = new PassageNode (n);
			aktuelle.kids.Add(passageTags);
		}

		public void createPassageContentNode(){
			object n = VisitPassageContent(TweeParser.PassageContentContext context);
			PassageNode passageContent = new PassageNode (n);
			aktuelle.kids.Add(passageContent);
		}

		public void createLinkNode(){
			object n = VisitLink(TweeParser.LinkContext context);
			PassageNode link = new PassageNode (n);
			content.kids.Add(link);
		
		}

		public void createTextNode(){
			object n = VisitText(TweeParser.TextContext context);
			PassageNode text = new PassageNode (n);
			content.kids.Add(text);
		
		}

		public void createVariable(){
			object n = VisitVariable(TweeParser.VariableContext context);
			PassageNode variablen = new PassageNode (n);
			content.kids.Add(variablen);
		}

		public void createFunctions () {
			object n= VisitFunction(TweeParser.FunctionContext context);
			PassageNode functions = new PassageNode (n);
			content.kids.Add(functions);
			
		}
		public void createMacrosNode (){
			object n = VisitMacro(TweeParser.MacroContext context);
			PassageNode macros = new PassageNode (n);
			content.kids.Add(macros);
		}*/

    }
}
