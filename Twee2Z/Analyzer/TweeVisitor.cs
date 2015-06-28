using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;
using System.Collections;
using Twee2Z.ObjectTree.Expr;
using Twee2Z.ObjectTree.PassageContents;
using Twee2Z.ObjectTree.PassageContents.Macro;
using Twee2Z.ObjectTree.PassageContents.Macro.Branch;

namespace Twee2Z.Analyzer
{
    class TweeVisitor : TweeBaseVisitor<object>
    {
        private TweeBuilder _builder;

        public Tree Tree
        {
            get { return _builder.Tree; }
        }

        public override object VisitStart(Twee.StartContext context)
        {
            _builder = new TweeBuilder();
            Logger.LogAnalyzer("[Start]");
            return base.VisitStart(context);
        }

        public override object VisitPassage(Twee.PassageContext context)
        {
            Logger.LogAnalyzer("[Passage]");
            string[] tags;

            // Remove Spaces after PassageName
            String name = context.GetChild(1).GetText().Trim();

            Logger.LogAnalyzer("Name: " + name);
            _builder.AddPassage(new Passage(name));

            if (context.TAG() != null)
            {
                // Get each tag and pass & print it
                string tagString = context.TAG().GetText();
                if (tagString.Length != 0)
                {
                    tagString = tagString.Substring(1, tagString.Length - 2);
                    tags = tagString.Replace("\t", " ").Split(' ');
                    for (int i = 0; i < tags.Length; i++)
                    {
                        tags[i].Trim();
                        if (!tags[i].Equals(""))
                        {
                            Logger.LogAnalyzer("[PassageTag] = " + tags[i]);
                            _builder.AddTag(tags[i]);
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
                _builder.AddPassageContent(new PassageLink(Ziel, Text, false));
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
                _builder.AddPassageContent(new PassageLink(Ziel, Expression, true));
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
                _builder.AddPassageContent(new PassageLink(Ziel, Text, Expression));
            }

            // case: [[target]]
            else
            {
                Ziel = context.GetChild(1).GetText();
                Logger.LogAnalyzer("Ziel: " + Ziel);
                _builder.AddPassageContent(new PassageLink(Ziel));
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
                        case "{{{":
                            _builder.CurrentFormat.Monospace = true;
                            break;
                        case "}}}":
                            _builder.CurrentFormat.Monospace = false;
                            break;
                        case "''":
                            _builder.CurrentFormat.Bold = !_builder.CurrentFormat.Bold;
                            break;
                        case "//":
                            _builder.CurrentFormat.Italic = !_builder.CurrentFormat.Italic;
                            break;
                        case "__":
                            _builder.CurrentFormat.Underline = !_builder.CurrentFormat.Underline;
                            break;
                        case "==":
                            _builder.CurrentFormat.Strikeout = !_builder.CurrentFormat.Strikeout;
                            break;
                        case "^^":
                            _builder.CurrentFormat.Superscript = !_builder.CurrentFormat.Superscript;
                            break;
                        case "~~":
                            _builder.CurrentFormat.Subscript = !_builder.CurrentFormat.Subscript;
                            break;
                        case "/%":
                            _builder.CurrentFormat.Comment = true;
                            break;
                        case "%/":
                            _builder.CurrentFormat.Comment = false;
                            break;
                        default:
                            Logger.LogAnalyzer("Text: " + Text);
                            _builder.AddPassageContent(new PassageText(Text));
                            break;
                    }
                }
                else
                {
                    _builder.AddPassageContent(new PassageText(Text));
                }
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
            _builder.AddPassageContent(new PassageVariable(VarName, 0));
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
            String functionName = context.GetChild(0).GetText();
            String paramList = context.GetChild(2).GetText().Trim();

            PassageFunction objectF = new PassageFunction(functionName);

            Logger.LogAnalyzer("Function: " + functionName);

            /*
            if (!(_paramList.Equals(')')))
            {
                for (int i = 0; i < _paramList.Length; i++)
                    objectF.addArg(_paramList[i]);
            }
            */
            return base.VisitFunction(context);
        }

        /**
         * This function is called if a macro is read inside a twee file
         * It displays the read macro in the console.
         **/
        public override object VisitMacro(Twee.MacroContext context)
        {
            String macro = context.GetText();
            Logger.LogAnalyzer("Macro: " + macro);

            Expression expr;
            switch (context.GetChild(1).GetText().ToLower())
            {
                case "display":
                    expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroDisplay(expr));
                    break;
                case "set":
                    expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroSet(expr));
                    break;
                case "print":
                    expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroPrint(expr));
                    break;
                case "if":
                case "endif":
                    //nothing to do
                    break;
            }
            return base.VisitMacro(context);
        }

        public override object VisitMacroBranchIf(Twee.MacroBranchIfContext context)
        {
            _builder.AddPassageContent(new PassageMacroBranch());

            Expression expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
            _builder.AddPassageContent(new PassageMacroIf(expr));
            return base.VisitMacroBranchIf(context);
        }

        public override object VisitMacroBranchIfElse(Twee.MacroBranchIfElseContext context)
        {
            Expression expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
            _builder.AddPassageContent(new PassageMacroElseIf(expr));
            return base.VisitMacroBranchIfElse(context);
        }

        public override object VisitMacroBranchElse(Twee.MacroBranchElseContext context)
        {
            _builder.AddPassageContent(new PassageMacroElse());
            return base.VisitMacroBranchElse(context);
        }


        public override object VisitMacroBranchPop(Twee.MacroBranchPopContext context)
        {
            _builder.FinishBranch();
            return base.VisitMacroBranchPop(context);
        }

        public Expression ParseExpression(Twee.ExpressionContext context)
        {
            // TODO
            return new Expression(context.GetText());
        }
    }
}
