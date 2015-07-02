lexer grammar LEX;
options
{
  //turn on backtracking
  backtrack=true;
}

INT					: DIGIT+;
PASS				: NEW_LINE ':'(':')+ -> pushMode(PMode);
MACRO_START			: MACRO_BRACKET_OPEN -> pushMode(MMode); 
LINK_START			: '[[' -> pushMode(LMode);
FUNC_START			: FUNC_NAME -> pushMode(FMode);
VAR_NAME			: DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
FORMAT				: ('\u0027\u0027'	
					|'//'				
					|'__'				
					|'=='				
					|'^^'				
					|'~~'				
					|'{{{'				
					|'/%'				
					|'}}}'				
					|'%/'	);			
EXCLUDE				: (':'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%'|']'|'['|'\u0027');
NEW_LINE			: ('\r' | '\n' | '\r\n');
STRING_START		: QUOTE -> pushMode(SMode);
STRING_START2		: QUOTE2 -> pushMode(SMode2);
SPACE				: ' ';
WORD				: ~(':'|'0'..'9'|'\r'|'\n'|'['|']'|' '|'"'|'/'|'_'|'='|'^'|'~'|'{'|'/'|'}'|'%'|'|'|'\u0027');
STRING				: STRING_START STRING_BODY STRING_END
					| STRING_START2 STRING_BODY2 STRING_END2;

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
fragment QUOTE2				: '\'';
fragment DOLLAR				: '$';

// PASSAGE-MODE
mode PMode;
PMODEWORD				: SPACE* ((WORD|INT|'/%'|'%/')+ SPACE*)+;
TAG						: TAG_BEGIN ('.'|'_'|(SPACE* ((WORD|INT)+ SPACE*)))* TAG_END;
TAG_BEGIN				: '[';
TAG_END					: ']';
PMODE_END				: SPACE* NEW_LINE -> popMode;

// STRING-MODE
mode SMode;
STRING_BODY			: .*?;
STRING_END			: QUOTE -> popMode;

mode SMode2;
STRING_BODY2		: .*?;
STRING_END2			: QUOTE2 -> popMode;

// FUNCTION-MODE
mode FMode;
FUNC_NAME			: 'random' | 'either' | 'visited' | 'visitedTag' | 'turns' | 'confirm' | 'prompt';
FUNC_BRACKET_OPEN	: '('  -> pushMode(EMode);
FUNC_BRACKET_CLOSE	: ')'  -> popMode; 

// MAKRO-MODE 
mode MMode;
IF					: 'if'   -> pushMode(EMode);
ELSE_IF				: 'else if'   -> pushMode(EMode);
ELSE				: 'else' (SPACE|NEW_LINE)*;
ENDIF				: 'endif' (SPACE|NEW_LINE)*;
NOBR				: 'nobr' (SPACE|NEW_LINE)*;
ENDNOBR				: 'endnobr' (SPACE|NEW_LINE)*;
SILENTLY			: 'silently' (SPACE|NEW_LINE)*;
ENDSILENTLY			: 'endsilently' (SPACE|NEW_LINE)*;
ACTIONS				: 'actions' (SPACE|NEW_LINE)*;
CHOICE				: 'choice' (SPACE|NEW_LINE)*;
DISPLAY				: 'display' -> pushMode(EMode);
SET					: 'set' -> pushMode(EMode);
PRINT				: 'print' -> pushMode(EMode);
MACRO_BRACKET_OPEN	: '<<';
MACRO_END			: '>>' -> popMode;

// EXPRESSION-MODE
mode EMode;
EXPRESSION
	: EXPRESSION_BODY   -> popMode, popMode
	;

EXPRESSION_BODY
	: STRING EXPRESSION_BODY
	| ~'>' EXPRESSION_BODY
	| . TEST
	;

TEST
	: ~'>' EXPRESSION_BODY
	| .
	;

EXP_END_M : '>>'  -> popMode, popMode;


// LINK-MODE

mode LMode;
FUNC_LINK	: ('previous()' | 'start()' | 'passage()');
PIPE				: '|';
SQ_BRACKET_CLOSE	: ']';
SQ_BRACKET_OPEN		: '['  -> pushMode(EMode);
WORDS				: (WORD|SPACE)+; 
LINK_END			: ']]' -> popMode; 