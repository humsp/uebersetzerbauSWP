using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private const string INLINE = "@@";
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

        private const string VARIABLE = @"\$[a-zA-Z_][a-zA-Z0-9_]*$";
        private const string FUNCTION = @"\b[^()]+\((.*)\)$";        

        public static string Unquoted
        {
            get { return UNQUOTED; }
        }
        public static string Link
        {
            get { return LINK; }
        }
        public static string Macro
        {
            get { return MACRO; }
        }

        public static string Image
        {
            get { return IMAGE; }
        }

        public static string HtmlBlock
        {
            get { return HTML_BLOCK; }
        }

        public static string Html
        {
            get { return HTML; }
        }

        public static string InlineStyle
        {
            get { return INLINE; }
        }

        public static string InlineStyleProp
        {
            get { return INLINE_STYLE_PROP; }
        }

        public static string Mono
        {
            get { return MONO; }
        }

        public static string Comment
        {
            get { return COMMENT; }
        }

        public static string Combined
        {
            get
            {
                return "(" + String.Join(")|(.", new string[]{Link, Macro, Image, HtmlBlock, Html, InlineStyle, Mono, Comment, @"''|\/\/|__|\^\^|~~|=="}) + ")";
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

        public static string Variable
        {
            get { return VARIABLE; }
        }

        public static string Function
        {
            get { return FUNCTION; }
        }
    }
}
