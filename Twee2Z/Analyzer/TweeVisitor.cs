using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Twee2Z.ObjectTree;
using Twee2Z.Utils;
using System.Collections;
using Twee2Z.ObjectTree.PassageContents;
using Twee2Z.ObjectTree.PassageContents.Macro;
using Twee2Z.ObjectTree.PassageContents.Macro.Branch;
using Twee2Z.ObjectTree.Expressions;
using Twee2Z.Analyzer.Expressions;

namespace Twee2Z.Analyzer
{
    /// <summary>
    /// Visitor Class for all twee content
    /// Each visitor method is called if the grammar has reached the appropriated structure in the twee file
    /// </summary>
    class TweeVisitor : TweeBaseVisitor<object>
    {
        private TweeBuilder _builder;

        public Tree Tree
        {
            get { return _builder.Tree; }
        }

        /// <summary>
        /// Initialize Objecttree and displays [Start] to mark the beginning of content of the twee file.
        /// Additionally it displays a console warning, if content before "::Start" is existing
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitStart(Twee.StartContext context)
        {
            _builder = new TweeBuilder();

            // Remove leading new lines, spaces and colons and check if beginning is "Start"
            if (context.GetText().TrimStart('\n', '\r', ':', ' ').Substring(0, 5) != "Start")
                Logger.LogWarning("Content before the start passage is ignored");

            Logger.LogAnalyzer("[Start]");
            return base.VisitStart(context);
        }

        /// <summary>
        /// Each time a passage is visited, it is marked with [Passage] and the passage name as well
        /// possible existing tags are displayed
        /// The tags are each stored in the objecttree
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitPassage(Twee.PassageContext context)
        {
            Logger.LogAnalyzer("[Passage]");
            string[] _tags;
            String _name = context.GetChild(1).GetText().Trim();

            Logger.LogAnalyzer("Name: " + _name);
            _builder.AddPassage(new Passage(_name));

            if (context.TAG() != null)
            {
                // Get each tag and display & store it
                string _tagString = context.TAG().GetText();
                if (_tagString.Length != 0)
                {
                    _tagString = _tagString.Substring(1, _tagString.Length - 2);
                    _tags = _tagString.Replace("\t", " ").Split(' ');
                    for (int i = 0; i < _tags.Length; i++)
                    {
                        _tags[i].Trim();
                        if (!_tags[i].Equals(""))
                        {
                            Logger.LogAnalyzer("[PassageTag] = " + _tags[i]);
                            _builder.AddTag(_tags[i]);
                        }
                    }
                }
            }
            return base.VisitPassage(context);
        }

        /// <summary>
        /// Depending on the content, this function calls the other visiter methods.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitPassageContent(Twee.PassageContentContext context)
        {
            return base.VisitPassageContent(context);
        }

        /// <summary>
        /// A link is marked with [Link] and depending on the Link's content,
        /// target, displayedText or Expression are displayed 
        /// Additionally depending on the case, the link is stored in the
        /// objecttree
        /// If a expression exists, it will be evaluated with ParseExpression(ExpreContext)
        /// </summary>
        /// <remarks>Logger displays the mark "[Link]" and the target, displayedText and Expression if existing</remarks>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitLink(Twee.LinkContext context)
        {
            Logger.LogAnalyzer("[Link]");
            string _target = "";
            string _displayText = "";
            string _expression = "";
            Expression _expr;

            if (context.PIPE() != null)
            {
                // case: [[text|target]]   
                if (context.expression() == null)
                {
                    _target = context.GetChild(context.ChildCount - 2).GetText();
                    for (int i = 1; i < (context.ChildCount - 3); i++)
                    {
                        _displayText = _displayText + context.GetChild(i).GetText();
                    }
                    Logger.LogAnalyzer("_target: " + _target);
                    Logger.LogAnalyzer("DisplayText: " + _displayText);
                    _builder.AddPassageContent(new PassageLink(_target, _displayText));
                }

                // case: [[text|target][expression]]
                else
                {
                    _target = context.GetChild(context.ChildCount - 4).GetText();
                    for (int i = 1; i < (context.ChildCount - 5); i++)
                    {
                        _displayText = _displayText + context.GetChild(i).GetText();
                    }
                    _expression = context.GetChild<Twee.ExpressionContext>(0).GetText().TrimEnd(']');
                    Logger.LogAnalyzer("Target: " + _target);
                    Logger.LogAnalyzer("DisplayText: " + _displayText);
                    Logger.LogAnalyzer("Expression: " + _expression);
                    _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageLink(_target, _displayText, _expr));
                }

            }

            // case: [[target][expression]]
            else if (context.ChildCount == 5 && context.GetChild(3).GetText().Equals("["))
            {
                _target = context.GetChild(1).GetText();
                _expression = context.GetChild(4).GetText();
                Logger.LogAnalyzer("Target: " + _target);
                Logger.LogAnalyzer("Expression: " + _expression);
                _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                _builder.AddPassageContent(new PassageLink(_target, _expr));
            }

            // case: [[target]]
            else
            {
                _target = context.GetChild(1).GetText();
                Logger.LogAnalyzer("Target: " + _target);
                _builder.AddPassageContent(new PassageLink(_target));
            }
            return base.VisitLink(context);
        }

        /// <summary>
        /// Display plain text and sets the flags for possible formats such as bold, italic, ...
        /// Additionally the text is stored in the objecttree alongside the formats to mark the text as bold, italic, ...
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitText(Twee.TextContext context)
        {
            String _text = context.GetChild(0).GetText();
            if (!_text.Equals(""))  
            {
                if (!(_text.Equals("\r\n") 
                    || _text.Equals("\n")
                    || _text.Equals("\r")
                    || _text.Equals(" ")))
                {
                    switch (_text)
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
                            Logger.LogAnalyzer("Text: " + _text);
                            _builder.AddPassageContent(new PassageText(_text));
                            break;
                    }
                }
                else
                {
                    _builder.AddPassageContent(new PassageText(_text));
                }
            }
            return base.VisitText(context);
        }

        /// <summary>
        /// Display the macro alongside the expression in the macro.
        /// The expression is passed to ParseExpression() and
        /// the macro is stored inside the objecttree
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitMacro(Twee.MacroContext context)
        {
            String macro = context.GetText();
            Logger.LogAnalyzer("Macro: " + macro);

            Expression _expr;
            switch (context.GetChild(1).GetText().ToLower())
            {
                case "display":
                    _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroDisplay(_expr));
                    break;
                case "set":
                    _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroSet(_expr));
                    break;
                case "print":
                    _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
                    _builder.AddPassageContent(new PassageMacroPrint(_expr));
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

            Expression _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
            _builder.AddPassageContent(new PassageMacroIf(_expr));
            return base.VisitMacroBranchIf(context);
        }

        public override object VisitMacroBranchIfElse(Twee.MacroBranchIfElseContext context)
        {
            Expression _expr = ParseExpression(context.GetChild<Twee.ExpressionContext>(0));
            _builder.AddPassageContent(new PassageMacroElseIf(_expr));
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

        /// <summary>
        /// Evaluate the given expression
        /// </summary>
        /// <param name="context"></param>
        /// <returns>value of expression</returns>
        public Expression ParseExpression(Twee.ExpressionContext context)
        {
            string text = context.GetChild(0).GetText();
            Expression _expr = ExpressionAnalyzer.Parse(text.Substring(0, text.Count() - 2));
            return _expr;
        }
    }
}
