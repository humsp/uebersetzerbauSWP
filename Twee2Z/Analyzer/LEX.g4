lexer grammar LEX;

INT					: DIGIT+;
PASS				: ':'(':')+ -> pushMode(PMode);
MACRO_START			: MACRO_BRACKET_OPEN -> pushMode(MMode); 
LINK_START			: '[[' -> pushMode(LMode);
FUNC_START			: FUNC_NAME -> pushMode(FMode);
VAR_NAME			: DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
FORMAT				: ('\u0027\u0027'	{ObjectTree.PassageContent.Bold = true;}
					|'//'				{ObjectTree.PassageContent.Italic = true;}
					|'__'				{ObjectTree.PassageContent.Underline = true;}
					|'=='				{ObjectTree.PassageContent.Strikeout = true;}
					|'^^'				{ObjectTree.PassageContent.Superscript = true;}
					|'~~'				{ObjectTree.PassageContent.Subscript = true;}
					|'{{{'				{ObjectTree.PassageContent.Monospace = true;}
					|'/%'				{ObjectTree.PassageContent.Comment = true;}
					|'}}}'				{ObjectTree.PassageContent.Monospace = false;}
					|'%/'				{ObjectTree.PassageContent.Comment = false;});
EXCLUDE				: (':'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%'|']'|'['|'\u0027');
NEW_LINE			: ('\r' | '\n' | '\r\n');
STRING_START		: QUOTE -> pushMode(SMode);
SPACE				: ' ';
WORD				: ~(':'|'0'..'9'|'\r'|'\n'|'['|']'|' '|'"'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%'|'|'|'\u0027');
STRING				: STRING_START STRING_BODY STRING_END;

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

// PASSAGE-MODE
mode PMode;
PMODEWORD				: SPACE* ((WORD|INT|'/%'|'%/')+ SPACE*);
TAG						: TAG_BEGIN ('.'|'_'|(SPACE* ((WORD|INT)+ SPACE*)))* TAG_END;
TAG_BEGIN				: '[';
TAG_END					: ']';
PMODE_END				: SPACE* NEW_LINE -> popMode;

// STRING-MODE
mode SMode;
STRING_BODY			: .*?;
STRING_END			: QUOTE -> popMode;

// FUNCTION-MODE
mode FMode;
FUNC_NAME			: 'random' | 'either' | 'visited' | 'visitedTag' | 'turns' | 'confirm' | 'prompt';
FUNC_BRACKET_OPEN	: '('  -> pushMode(EMode);
FUNC_BRACKET_CLOSE	: ')'  -> popMode; 

// MAKRO-MODE 
mode MMode;
IF					: 'if'   -> pushMode(EMode);
ELSE_IF				: 'else if'   -> pushMode(EMode);
ELSE				: 'else' SPACE*;
ENDIF				: 'endif' SPACE*;
NOBR				: 'nobr' SPACE*;
ENDNOBR				: 'endnobr' SPACE*;
SILENTLY			: 'silently' SPACE*;
ENDSILENTLY			: 'endsilently' SPACE*;
ACTIONS				: 'actions' SPACE*;
CHOICE				: 'choice' SPACE* ;
DISPLAY				: 'display' NEW_LINE*  -> pushMode(EMode);
SET					: 'set' NEW_LINE*  -> pushMode(EMode);
PRINT				: 'print' NEW_LINE*   -> pushMode(EMode);
MACRO_BRACKET_OPEN	: '<<' SPACE*;
MACRO_END			: '>>' -> popMode;

// EXPRESSION-MODE
mode EMode;
EXPRESSION :	 ((NOT|ADD|SUB) EXPRESSION SPACE*)
				|((VAR_NAME|STRING|INT+) SPACE* (LOG_OP|MOD|ADD|MUL|DIV|SUB|EQ_SIGN) EXPRESSION SPACE*)
				|('('EXPRESSION ')')
				|(SPACE EXPRESSION)
				|(VAR_NAME|STRING|INT+)
				;
FUNC_PARAM		: COMMA EXPRESSION;
MUL	: '*';
DIV	: '/';
ADD	: '+';
SUB	: '-';
LOG_OP : 'is' | 'eq'|'neq'| 'and'| 'or'| '<'| 'lt'|  '<='| 'lte'| '>'| 'gt'| '>='| 'gte';
MOD	: '%';
EQ_SIGN: '=';
NOT	: 'not';
EXP_END: ')' ->mode(DEFAULT_MODE);
EXP_END_L: ']]' ->mode(DEFAULT_MODE);
EXP_END_M: ('\n'|'\r')* '>>' ->mode(DEFAULT_MODE);

// LINK-MODE

mode LMode;
FUNC_LINK	: ('previous()' | 'start()' | 'passage()');
PIPE				: '|';
SQ_BRACKET_CLOSE	: ']';
SQ_BRACKET_OPEN		: '['  -> pushMode(EMode);
WORDS				: WORD+; 
LINK_END			: ']]' -> popMode; 