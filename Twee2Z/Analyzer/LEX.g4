lexer grammar LEX;

// special symbols

INT					: DIGIT+;
MACRO_START			: MACRO_BRACKET_OPEN -> pushMode(MMode); 
LINK_START			: '[[' -> pushMode(LMode);
FUNC_START			: FUNC_NAME -> pushMode(FMode);
VAR_NAME			: DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
FORMAT				: (	'//'{ObjectTree.PassageContent.Italic = true;}
					|'__'{ObjectTree.PassageContent.Underline = true;}
					|'=='{ObjectTree.PassageContent.Strikeout = true;}
					|'^^'{ObjectTree.PassageContent.Superscript = true;}
					|'~~'{ObjectTree.PassageContent.Subscript = true;}
					|'{{{'{ObjectTree.PassageContent.Monospace = true;}
					|'/%'{ObjectTree.PassageContent.Comment = true;}
					|'}}}'{ObjectTree.PassageContent.Monospace = false;}
					|'%/'{ObjectTree.PassageContent.Comment = false;});
EXCLUDE				: (':'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%');
NEW_LINE			: ('\r' | '\n' | '\r\n');
PASS				: ':'(':'+);
// NEW_LINE_PASS		: NEW_LINE PASS;
// STRING-MODE
STRING_START		: QUOTE -> pushMode(SMode);
SPACE				: ' ';
TAB					: '\t';
WORD				: ~(':'|'0'..'9'|'\r'|'\n'|'['|']'|' '|'"'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%');

/*
ITALIC_BEGIN		: '//'   -> pushMode(ItalicMode);
UNDERLINE_BEGIN		: '__'   -> pushMode(UnderlineMode);
STRIKEOUT_BEGIN		: '=='   -> pushMode(StrikeoutMode);
SUPERSCRIPT_BEGIN	: '^^'   -> pushMode(SuperscriptMode);
SUBSCRIPT_BEGIN		: '~~'   -> pushMode(SubscriptMode);
MONOSPACE_BEGIN		: '{{{'  -> pushMode(MonospaceMode);
COMMENT_BEGIN		: '/%'   -> pushMode(CommentMode);
*/

STRING				: STRING_START STRING_BODY STRING_END;
// ESPRESSION-MODE
EXPRESSION_FORM		: (EXPRESSION_START EXPRESSION_CONTENT);
EXPRESSION_START	: (VAR_NAME | STRING | (NOT|ADD|SUB)? INT) -> pushMode(EMode);


//~('0'..'9'|'\r'|'\n'|'['|']'|' '|'"'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%');
// ('''' | '//' | '__' | '==' | '^^' | '~~' | '{{{' | '/%' | '@@')

// normal symbols
fragment LETTER: [a-zA-Z];
fragment DIGIT : [0-9];
fragment DOUBLE_DOT			: ':';
fragment EXCLAMATION_MARK	:'!';
fragment POINT				: '.';
fragment COMMA				: ',';
fragment SEMICOLON			: ';';
fragment LOW_LINE			: '_';
fragment QUOTE				: '"';
fragment DOLLAR				: '$';

// TAG-MODE
TAGS				: SQ_BRACKET_OPEN SPACE* TAG_WORDS (SPACE+ TAG_WORDS)* SPACE* SQ_BRACKET_CLOSE;
fragment TAG_WORDS				: (INT|WORD)+;
fragment TAG_BRACKET_CLOSE		: ']';
fragment TAG_BRACKET_OPEN		: '[';

mode SMode;
STRING_BODY			: .*?;
STRING_END			: QUOTE -> popMode;

// FUNCTION-MODE
mode FMode;
FUNC_NAME			: 'random' | 'either' | 'visited' | 'visitedTag' | 'turns' | 'confirm' | 'prompt';
FUNC_PARAM			: ((STRING) (COMMA SPACE* (STRING))*)?;
FUNC_BRACKET_OPEN	: '(';
FUNC_BRACKET_CLOSE	: ')' -> popMode; 

// MAKRO-MODE 
mode MMode;
FOR                 : 'for' 'in' SPACE*;
ENDFOR              : 'endfor' SPACE*;
IF					: 'if' SPACE*;
ELSE_IF				: 'else if' SPACE*;
ELSE				: 'else' SPACE*;
ENDIF				: 'endif' SPACE*;
NOBR				: 'nobr' SPACE*;
ENDNOBR				: 'endnobr' SPACE*;
SILENTLY			: 'silently' SPACE*;
ENDSILENTLY			: 'endsilently' SPACE*;
ACTIONS				: 'actions' SPACE*;
CHOICE				: 'choice' SPACE* ;
RADIO               : 'radio' SPACE* ;
DISPLAY				: 'display' (NEW_LINE|SPACE)* VAR_NAME? (NEW_LINE|SPACE)*;
SET					: 'set' (NEW_LINE|SPACE)*;
PRINT				: 'print' (NEW_LINE|SPACE)* VAR_NAME? (NEW_LINE|SPACE)*;
MACRO_BRACKET_OPEN	: '<<';
MACRO_END			: '>>' -> popMode;

mode EMode;
EXPRESSION_CONTENT : ((((NOT|ADD|SUB) EXPRESSION_CONTENT) | (SPACE* (LOG_OP|MOD|ADD|MUL|DIV|SUB) SPACE* EXPRESSION_END) | (FUNC_BRACKET_OPEN EXPRESSION_CONTENT FUNC_BRACKET_CLOSE)));
MUL	: '*';
DIV	: '/';
ADD	: '+';
SUB	: '-';
LOG_OP : 'is' | 'eq'|'neq'| 'and'| 'or'| '<'| 'lt'|  '<='| 'lte'| '>'| 'gt'| '>='| 'gte';
MOD	: '%';
EQ_SIGN: '=';
NOT	: 'not';
LT		: '<';
GT		: '<';
EXPRESSION_END	: (COMMA | FUNC_BRACKET_CLOSE) -> popMode;// (SQ_BRACKET_CLOSE|FUNC_BRACKET_CLOSE|LT) -> popMode;

// LINK-MODE
mode LMode;
FUNC_LINK	: ('previous()' | 'start()' | 'passage()');
PIPE				: '|';
SQ_BRACKET_CLOSE	: ']';
SQ_BRACKET_OPEN		: '[';
WORDS				: WORD+; //<- nach unten weil sonst keine funk erkannt werden
LINK_END			: ']]' -> popMode; 