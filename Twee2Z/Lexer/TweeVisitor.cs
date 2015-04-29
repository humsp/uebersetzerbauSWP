using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.Lexer
{
    class TweeVisitor :TweeBaseVisitor<object>
    {

        int i = 0;

     

        public override object VisitLink(TweeParser.LinkContext context)
        {
            Console.WriteLine(i + "_" + context.GetText());
            i++;
            return base.VisitLink(context);
        }

        public override object VisitPassage(TweeParser.PassageContext context)
        {
            Console.WriteLine(i + "_" + context.GetText());
            i++;
            return base.VisitPassage(context);
        }

        public override object VisitPassageContent(TweeParser.PassageContentContext context)
        {
            Console.WriteLine(i + "_" + context.GetText());
            i++;
            return base.VisitPassageContent(context);
        }

        public override object VisitStart(TweeParser.StartContext context)
        {

            Console.WriteLine(context.GetType());
            return base.VisitStart(context);
        }


        
    }
}
