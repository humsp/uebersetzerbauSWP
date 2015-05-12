using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Twee2Z.Lexer
{
    public static class Pattern
    {
        private const string UNQUOTED = @"(?=(?:[^""'\\]*(?:\\.|'(?:[^'\\]*\\.)*[^'\\]*'|""(?:[^""\\]*\\.)*[^""\\]*""))*[^'""]*$)";
        private const string LINK = @"\[\[([^\|]*?)(?:\|(.*?))?\](\[.*?\])?\]";
        private const string MACRO = @"<<([^>\s]+)(?:\s*)((?:\\.|'(?:[^'\\]*\\.)*[^'\\]*'|""(?:[^""\\]*\\.)*[^""\\]*""|[^'""\\>]|>(?!>))*)>>";
        private const string IMAGE = @"\[([<]?)(>?)img\[(?:([^\|\]]+)\|)?([^\[\]\|]+)\](?:\[([^\]]*)\]?)?(\])";
        private const string HTML_BLOCK = @"<html>((?:.|\n)*?)</html>";
        private const string HTML = @"<(?:\/?([\w\-]+)(?:(\s+[\w\-]+(?:\s*=\s*(?:\"".*?\""|'.*?'|[^'\"">\s]+))?)+\s*|\s*)\/?)>";
        private const string INLINE_STYLE = "@@";
        private const string INLINE_STYLE_PROP = @"((?:([^\(@]+)\(([^\)]+)(?:\):))|(?:([^:@]+):([^;@]+);)|(?:(\.[^\.;@]+);))+";
        private const string MONO = @"^\{\{\{\n(?:(?:^[^\n]*\n)+?)(?:^\}\}\}$\n?)|\{\{\{((?:.|\n)*?)\}\}\}";
        private const string COMMENT = @"/%((?:.|\n)*?)%/";
        private const string MACRO_PARAMS_VAR = @"(\$[\w_\.]*[a-zA-Z_\.]+[\w_\.]*)";
        private const string MACRO_PARAMS_FUNC = @"([\w\d_\.]+\((.*?)\))";
        private const string MACRO_PARAMS = @"(?:(""(?:[^\\""]|\\.)*""|\'(?:[^\\\']|\\.)*\'|(?:\[\[(?:[^\]]*)\]\]))" +
                    @"|\b(\-?\d+\.?(?:[eE][+\-]?\d+)?|NaN)\b" +
                    @"|(true|false|null|undefined)" +
                    "|" + MACRO_PARAMS_VAR + ")";
        private const string IMAGE_FILENAME = @"[^\""']+\.(?:jpe?g|a?png|gif|bmp|webp|svg)";
        private const string EXTERNAL_IMAGE_URL = @"\s*['\""]?(" + IMAGE_FILENAME + @")['\""]?\s*";
        private const string HTML_IMAGE = @"src\s*=" + EXTERNAL_IMAGE_URL;
        private const string CSS_IMAGE = @"url\s*\(" + EXTERNAL_IMAGE_URL + @"\)";

        private static Regex _linkRegex = new Regex(LINK);
        private static Regex _macroRegex = new Regex(MACRO);
        private static Regex _imageRegex = new Regex(IMAGE);
        private static Regex _htmlBlockRegex = new Regex(HTML_BLOCK);
        private static Regex _htmlRegex = new Regex(HTML);
        private static Regex _inlineStyleRegex = new Regex(INLINE_STYLE);
        private static Regex _monoRegex = new Regex(MONO);
        private static Regex _commentRegex = new Regex(COMMENT);

        public static string Unquoted
        {
            get { return UNQUOTED; }
        }
        public static Regex Link
        {
            get { return _linkRegex; }
        }
        public static Regex Macro
        {
            get { return _macroRegex; }
        }

        public static Regex Image
        {
            get { return _imageRegex; }
        }

        public static Regex HtmlBlock
        {
            get { return _htmlBlockRegex; }
        }

        public static Regex Html
        {
            get { return _htmlRegex; }
        }

        public static Regex InlineStyle
        {
            get { return _inlineStyleRegex; }
        }

        public static string InlineStyleProp
        {
            get { return INLINE_STYLE_PROP; }
        }

        public static Regex Mono
        {
            get { return _monoRegex; }
        }

        public static Regex Comment
        {
            get { return _commentRegex; }
        }

        public static string Combined
        {
            get
            {
                return "(" + String.Join(")|(.", new string[] { LINK, MACRO, IMAGE, HTML_BLOCK, HTML, INLINE_STYLE, MONO, COMMENT, @"''|\/\/|__|\^\^|~~|==" }) + ")";
            }
        }

        public static string MacroParamsVar
        {
            get { return MACRO_PARAMS_VAR; }
        }

        public static string MacroParamsFunc
        {
            get { return MACRO_PARAMS_FUNC; }
        }

        public static string MacroParams
        {
            get { return MACRO_PARAMS; }
        }

        public static string ImageFilename
        {
            get { return IMAGE_FILENAME; }
        }

        public static string ExternalImageUrl
        {
            get { return EXTERNAL_IMAGE_URL; }
        }

        public static string ExternalImage
        {
            get
            {
                return Image.Replace(@"([^\[\]\|]+)", ExternalImageUrl);
            }
        }

        public static string HtmlImage
        {
            get { return HTML_IMAGE; }
        }

        public static string CssImage
        {
            get { return CSS_IMAGE; }
        }
    }
}
