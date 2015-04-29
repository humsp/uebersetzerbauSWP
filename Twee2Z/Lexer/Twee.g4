grammar Twee;

/*
 * Parser Rules
 */
compileUnit
	:	EOF
	;



start
	: passage+
	;

passage
	: PASSAGE_SYM WS WORD WS passageContent
	;

passageContent
	: text passageContent
	| link passageContent
	|
	;

link
	: LINK_OPEN WORD LINK_CLOSE
	;


text
	: txt text
	| txt
	;

/*
 * Lexer Rules
 */

 // special symbols

PASSAGE_SYM	: '::';
LINK_OPEN	: '[[';
LINK_CLOSE	: ']]';
 
// normal symbols

txt
	: WS
	| WORD
	| EXCLAMATION_MARK
	| POINT
	| COMMA
	| SEMICOLON
	| LOW_LINE
	| INT
	| MUL
	| DIV
	| ADD
	| SUB
	;

WORD: [a-zA-Z]+;
EXCLAMATION_MARK:'!';
POINT: '.';
COMMA: ',';
SEMICOLON: ';';
LOW_LINE: '_';
INT : [0-9]+;
MUL : '*';
DIV : '/';
ADD : '+';
SUB : '-';

WS
	:	(' ' | '\r' | '\n') 
	;

