grammar Twee;

/*
 * Parser Rules
 */

start
	: passage+
	;

passage
	: passageStart SPACE* passageName SPACE* (passageTags SPACE*)? NEW_LINE passageContent
	;
	
passageStart
	: PASSAGE_START
	| COLON COLON
	;

passageName
	: symbol
	;

passageTags
	: SQ_BRACKET_OPEN symbol (SPACE+ symbol)* SQ_BRACKET_CLOSE
	;

passageContent
	: (text|link) passageContent
	| (text|link)
	;

link
	: LINK_OPEN symbol SQ_BRACKET_CLOSE SQ_BRACKET_CLOSE
	| LINK_OPEN text PIPE symbol SQ_BRACKET_CLOSE (SQ_BRACKET_OPEN text)? SQ_BRACKET_CLOSE
	;

symbol
	: WORD
	;

text
	: (WORD|SPACE|SQ_BRACKET_OPEN|SQ_BRACKET_CLOSE|COLON|PIPE|NEW_LINE) text
	| (WORD|SPACE|SQ_BRACKET_OPEN|SQ_BRACKET_CLOSE|COLON|PIPE|NEW_LINE)
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
PIPE: '|';
 
// normal symbols	
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
