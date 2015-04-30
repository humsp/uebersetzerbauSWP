using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.Lexer
{
    class TweeVisitor :TweeBaseVisitor<object>
    {

        public override object VisitLink(TweeParser.LinkContext context)
        {
            Console.WriteLine("Link: " + context.GetChild(1).GetText());
            return base.VisitLink(context);
        }

        public override object VisitPassage(TweeParser.PassageContext context)
        {
            return base.VisitPassage(context);
        }

        public override object VisitPassageName(TweeParser.PassageNameContext context)
        {
            Console.WriteLine("Passage name:" + context.GetText());
            return base.VisitPassageName(context);
        }

        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            if (context.GetChild(0) is TweeParser.TextContext)
            {
                Console.WriteLine("Text:" + context.GetChild(0).GetText());
            }
            else
            {
             //   Console.WriteLine(context.GetChild(0).GetText());
            }
            return base.VisitPassageContent(context);
        }

        public override object VisitStart(TweeParser.StartContext context)
        {

            //Console.WriteLine("Start:____");
            //Console.WriteLine(context.GetText());
            return base.VisitStart(context);
        }

        public override object VisitTxt(TweeParser.TxtContext context)
        {
            //Console.WriteLine("Start:____");
            //Console.WriteLine(context.GetText());
            return base.VisitTxt(context);
        }

        public override object VisitText(TweeParser.TextContext context)
        {
           // Console.WriteLine("Start:____");
            //Console.WriteLine(context.GetText());
            return base.VisitText(context);
        }


        
    }
}
