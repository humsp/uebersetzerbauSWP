using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Twee2Z.Analyzer
{
    class TweeVisitor : TweeBaseVisitor<object>
    {
        public override object VisitStart(TweeParser.StartContext context)
        {
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
            return base.VisitPassageName(context);
        }

        public override object VisitPassageTags(TweeParser.PassageTagsContext context)
        {
            Console.WriteLine("Tags: " + context.GetText());
            return base.VisitPassageTags(context);
        }

        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            if (context.GetChild(0) is TweeParser.TextContext)
            {
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
            }
            else if (context.ChildCount == 9)
            {
                Console.WriteLine("Ziel: " + context.GetChild(3).GetText());
                Console.WriteLine("Text: " + context.GetChild(1).GetText());
                Console.WriteLine("Expression: " + context.GetChild(6).GetText());
            }
            else
                Console.WriteLine("Ziel: " + context.GetChild(1).GetText());
            return base.VisitLink(context);
        }
        
        public override object VisitText(TweeParser.TextContext context)
        {
            return base.VisitText(context);
        }
    }
}
