grammar Twee;

/*
 * Parser Rules
 */

start
	: passage+
	;

passage
	: PASSAGE SPACE? passageName SPACE? passageTags SPACE? NEW_LINE passageContent
	;

passageName
	: SYMBOL
	;

passageTags
	: PASSAGE_TAGS_OPEN SYMBOL PASSAGE_TAGS_CLOSE
	;

passageContent
	: (text|link) passageContent
	;

link
	: LINK_OPEN SYMBOL LINK_CLOSE
	;


text
	: TXT text
	| TXT
	;

/*
 * Lexer Rules
 */

// special symbols
PASSAGE	: '::';
PASSAGE_TAG_OPEN : '[';
PASSAGE_TAG_CLOSE : ']';
LINK_OPEN	: '[[';
LINK_CLOSE	: ']]';

 
// normal symbols
SYMBOL	: WORD
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

TXT	: WHITE_SPACE
	| SYMBOL
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
SPACE : ' ';
NEW_LINE : ('\n' | '\r' | '\n\r');
WHITE_SPACE : (SPACE | NEW_LINE);
