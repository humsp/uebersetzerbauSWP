lexer grammar ExpressionLexer;

// functions
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


BRACKET_OPEN	: '(';
BRACKET_CLOSE	: ')';
COMMA			: ',';

OP_LOG_AND	: 'and' | '&' '&'?;
OP_LOG_OR	: 'or' |  '|' '|'?;
OP_LOG_XOR	: 'xor' | '^';

OP_LOG_NOT	: 'not' | '!';


OP_SUB	: '-';
OP_ADD	: '+';
OP_MUL	: '*';
OP_DIV	: '/';
OP_MOD	: '%';

NEQ		: '!=' | 'neq';
EQ		: '==' | 'is' | 'eq';
GT		: '>' | 'gt';
GE		: '>=' | 'gte';
LT		: '<' | 'lt';
LE		: '<=' | 'lte';

ASSIGN_EQ	: '=' | 'to';
ASSIGN_SUB	: '-=';
ASSIGN_ADD	: '+=';
ASSIGN_MUL	: '*=';
ASSIGN_DIV	: '/=';
ASSIGN_MOD	: '%=';


WS				: (' ' | '\t' | '\r' | '\n')+;
VAR_NAME		: DOLLAR (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*;
NAME			: (LETTER|LOW_LINE) (LETTER|DIGIT|LOW_LINE)*; 

DIGITS		: DIGIT+;
DOT			: '.';

// normal symbols
fragment DOLLAR		: '$';
fragment LETTER		: [a-zA-Z];
fragment DIGIT		: [0-9];
fragment LOW_LINE	: '_';
fragment QUOTE		: '"';

STRING			: STRING_START STRING_BODY STRING_END;
STRING_START	: QUOTE -> pushMode(SMode);

// STRING-MODE
mode SMode;
STRING_BODY			: .*?;
STRING_END			: QUOTE -> popMode;