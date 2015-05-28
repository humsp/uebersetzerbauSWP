grammar Twee;

/*
 * Parser Rules
 */

start
	: WORD* passage+
	;

passage
	: passageStart SPACE* passageName SPACE* (passageTags SPACE*)? (NEW_LINE passageContent)?
	;
	
passageStart
	: PASSAGE_START COLON* 
	| COLON COLON+
	;

passageName
	: symbol (symbol|SPACE)*
	;

passageTags
	//: SQ_BRACKET_OPEN SPACE* innerPassageTag SQ_BRACKET_CLOSE
	: TAG_N (SPACE|WORD)*
	;

innerPassageTag
	: symbol SPACE+ innerPassageTag*
	;

passageContent
	: (text|link|variable|function|macro) passageContent
	| (text|link|variable|function|macro)
	;

link
	: LINK_OPEN (symbol PIPE)? (symbol|FUNC_LINK) SQ_BRACKET_CLOSE SQ_BRACKET_CLOSE
	| LINK_OPEN (symbol PIPE)? (symbol|FUNC_LINK) SQ_BRACKET_CLOSE_OPEN expression SQ_BRACKET_CLOSE SQ_BRACKET_CLOSE
	;

symbol
	: WORD
	;
	
text
	: (WORD|SPACE|SQ_BRACKET_OPEN|SQ_BRACKET_CLOSE|COLON|PIPE|NEW_LINE|STRING) text
	| (WORD|SPACE|SQ_BRACKET_OPEN|SQ_BRACKET_CLOSE|COLON|PIPE|NEW_LINE|STRING)	
	;

variable
	: VAR_N
	;

expression
	: SPACE* (FUNC_BRACKET_OPEN SPACE*)? (NOT|ADD|SUB)? SPACE* (variable|function|STRING|digit) SPACE* ((DIV|MUL|LOG_OP)? expression)* (SPACE* FUNC_BRACKET_CLOSE)?
	;

function
	: (WORD|FUNC_N) FUNC_BRACKET_OPEN ((expression(COMMA expression)*)?) FUNC_BRACKET_CLOSE
	;
digit
	: ('1'|'2'|'3'|'4'|'5'|'6'|'7'|'8'|'9'|'0')+
	;

macro
	: MACRO_BRACKET_OPEN ((MACRO_EXPR? (NEW_LINE|SPACE)* expression)|('actions' text+)|('choice' SPACE link)) MACRO_BRACKET_CLOSE
	| MACRO_BRACKET_OPEN 'if' SPACE+ expression MACRO_BRACKET_CLOSE passageContent (MACRO_BRACKET_OPEN 'else if' SPACE+ expression MACRO_BRACKET_CLOSE passageContent)? (MACRO_BRACKET_OPEN 'else' MACRO_BRACKET_CLOSE passageContent)? '<<endif>>'
	| MACRO_BRACKET_OPEN 'nobr' MACRO_BRACKET_CLOSE passageContent SPACE+ '<<endnobr>>'
	| MACRO_BRACKET_OPEN 'silently' MACRO_BRACKET_CLOSE passageContent SPACE+ '<<endsilently>>'
	;

/*
 * Lexer Rules
 */

// special symbols
PASSAGE_START : NEW_LINE '::';
LINK_OPEN : '[[';
COLON: ':';
SQ_BRACKET_OPEN: '[';
SQ_BRACKET_CLOSE: ']';
SQ_BRACKET_CLOSE_OPEN: '][';
PIPE: '|';
DOLLAR: '$';
FUNC_BRACKET_OPEN: '(';
FUNC_BRACKET_CLOSE: ')';
MACRO_BRACKET_OPEN: '<<';
MACRO_BRACKET_CLOSE: '>>';
FUNC_N : 'random' | 'either' | 'visited' | 'visitedTag' | 'turns' | 'confirm' | 'prompt';
NOT: 'not';
LOG_OP : 'is' | 'eq'|'neq'| 'and'| 'or'| '<'| 'lt'|  '<='| 'lte'| '>'| 'gt'| '>='| 'gte' | '%'| '=';
FUNC_LINK : 'previous()' | 'start()' | 'passage()';
MACRO_EXPR : 'set' | 'display' | 'print';
STRING : SPACE* '"' (WORD|SPACE|SQ_BRACKET_OPEN|SQ_BRACKET_CLOSE|COLON|PIPE|NEW_LINE)* '"';
 
// normal symbols
TAG_N : SQ_BRACKET_OPEN SPACE* (LETTER|POINT|LOW_LINE|INT) ((LETTER|POINT|LOW_LINE|INT)|SPACE)* SQ_BRACKET_CLOSE;
VAR_N : DOLLAR (LETTER|LOW_LINE) (LETTER|INT|LOW_LINE)*; //(NEW_LINE|SPACE)*;
WORD: (LETTER|EXCLAMATION_MARK|POINT|COMMA|SEMICOLON|LOW_LINE|INT|MUL|DIV|ADD|SUB)+;
fragment LETTER: [a-zA-Z];
fragment EXCLAMATION_MARK:'!';
fragment POINT: '.';
fragment COMMA: ',';
fragment SEMICOLON: ';';
fragment LOW_LINE: '_';
fragment INT : [0-9];
fragment MUL : '*';
fragment DIV : '/';
fragment ADD : '+';
fragment SUB : '-';
SPACE : ' ';
NEW_LINE : ('\r' | '\n' | '\r\n');