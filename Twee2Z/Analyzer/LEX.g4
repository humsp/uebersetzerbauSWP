lexer grammar LEX;

/*
 * Lexer Rules
 */
 // constructs for parser rules


MACRO_FORM			: MACRO_BRACKET_OPEN EXPRESSION_FORM MACRO_BRACKET_CLOSE;
fragment IF			: 'if';
fragment ELSE_IF	: 'else if';
fragment ELSE		: 'else';
fragment NOBR		: 'nobr';
fragment SILENTLY	: 'silently';
fragment ACTIONS	: 'actions';
fragment CHOICE		: 'choice';
fragment DISPLAY	: 'display';
fragment SET		: 'set';
fragment PRINT		: 'print';
fragment MACRO_BRACKET_OPEN	: '<<';
fragment MACRO_BRACKET_CLOSE: '>>';

fragment EXPRESSION_FORM	: ((((NOT|ADD|SUB) EXPRESSION_FORM) | ((VAR_NAME | STRING | LETTER+ | INT) SPACE* (LOG_OP|MOD|ADD|MUL|DIV|SUB) SPACE* EXPRESSION_FORM) | (FUNC_BRACKET_OPEN EXPRESSION_FORM FUNC_BRACKET_CLOSE)) | (VAR_NAME | STRING| LETTER+ | INT));
fragment MUL	: '*';
fragment DIV	: '/';
fragment ADD	: '+';
fragment SUB	: '-';
fragment LOG_OP : 'is' | 'eq'|'neq'| 'and'| 'or'| '<'| 'lt'|  '<='| 'lte'| '>'| 'gt'| '>='| 'gte';
fragment MOD	: '%';
fragment EQ_SIGN: '=';
fragment NOT	: 'not';

FUNC_FORM					: FUNC_NAME FUNC_BRACKET_OPEN (VAR_NAME (COMMA SPACE* VAR_NAME)*)* FUNC_BRACKET_CLOSE;
fragment FUNC_NAME			: 'random' | 'either' | 'visited' | 'visitedTag' | 'turns' | 'confirm' | 'prompt';
fragment FUNC_BRACKET_OPEN	: '(';
fragment FUNC_BRACKET_CLOSE : ')';

VAR_NAME : DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
fragment DOLLAR				: '$';

// special symbols
STRING	: QUOTE .* QUOTE; 
INT		: DIGIT+;
WORD	: ~('0'..'9'|'\n'|'\r'|' '|'['|']'|'|'); //<-PIPE hinzugefuegt wegen Link, ist noch zu beheben

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

SPACE : ' ';
NEW_LINE : ('\r' | '\n' | '\r\n');
PASS	 : ':'(':'+);

TAGS				: SQ_BRACKET_OPEN SPACE* TAG_WORDS (SPACE+ TAG_WORDS)* SPACE* SQ_BRACKET_CLOSE;
fragment TAG_WORDS				: (INT|WORD)+;
fragment TAG_BRACKET_CLOSE		: ']';
fragment TAG_BRACKET_OPEN		: '[';

LINK_START			: '[[' -> mode(LMODE);
mode LMODE;
FUNC_LINK	: ('previous()' | 'start()' | 'passage()');
PIPE				: '|';
SQ_BRACKET_CLOSE	: ']';
SQ_BRACKET_OPEN		: '[';
WORDS				: EXPRESSION_FORM|WORD+; //<- nach unten weil sonst keine funk erkannt werden
LINK_END			: ']]' -> mode(DEFAULT_MODE); //KEIN POPMODE VERWENDEN!!


