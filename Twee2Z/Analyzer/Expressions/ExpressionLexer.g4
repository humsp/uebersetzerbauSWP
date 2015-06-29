lexer grammar ExpressionLexer;

// functions
/*
FCN_RANDOM		: 'random';
FCN_PREVIOUS	: 'previous';
FCN_VISITED		: 'visited';
FCN_VISIT_TAG	: 'visitedTag';
FCN_TURNS		: 'turns';
FCN_PASSAGE		: 'passage';
FCN_CONFIRM		: 'confirm';
FCN_PROMT		: 'prompt';
FCN_ALERT		: 'alert';
FCN_PARAMETER	: 'parameter';
FCN_EITHER		: 'either';
*/

BRACKET_OPEN	: '(';
BRACKET_CLOSE	: ')';
SPACE			: ' ';
COMMA			: ',';

OP_LOG_AND	: 'and';
OP_LOG_AND2	: '&' '&'?;
OP_LOG_OR	: 'or';
OP_LOG_OR2	: '|' '|'?;
OP_LOG_XOR	: 'xor';
OP_LOG_XOR2	: '^';

OP_LOG_NOT	: 'not';
OP_LOG_NOT2	: '!';


OP_SUB	: '-';
OP_ADD	: '+';
OP_MUL	: '*';
OP_DIV	: '/';


EQ_IS	: 'is';
GT		: '>';
GE		: '>=';
LT		: '<';
LE		: '<=';

ASSIGN_EQ	: '=';
ASSIGN_SUB	: '-=';
ASSIGN_ADD	: '+=';
ASSIGN_MUL	: '*=';
ASSIGN_DIV	: '/=';
ASSIGN_TO	: 'to';	


DIGITS		: DIGIT+;

fragment DOLLAR		: '$';
fragment LETTER		: [a-zA-Z];
fragment DIGIT		: [0-9];
fragment LOW_LINE	: '_';
fragment QUOTE		: '"';

WS				: (' ' | '\t' | '\r' | '\n')+;
VAR_NAME		: DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
NAME			: (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*; 

STRING			: STRING_START STRING_BODY STRING_END;
STRING_START	: QUOTE -> pushMode(SMode);

// STRING-MODE
mode SMode;
STRING_BODY			: .*?;
STRING_END			: QUOTE -> popMode;