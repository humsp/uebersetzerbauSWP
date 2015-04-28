grammar Twee;

/*
 * Parser Rules
 */
compileUnit
	:	EOF
	;



start
	: .* firstPassage (passage)*
	;

firstPassage
	: PASSAGE_SYM 'start' passageContent
	;

passage
	: PASSAGE_SYM word passageContent
	;

passageContent
	: word passageContent
	| link passageContent
	|
	;

link
	: LINK_OPEN word LINK_CLOSE
	;

word
	: LETTER+
	;


/*
 * Lexer Rules
 */
LETTER		: [a-zA-Z];
PASSAGE_SYM	: '::';
LINK_OPEN	: '[[';
LINK_CLOSE	: ']]';
 


INT : [0-9]+;
MUL : '*';
DIV : '/';
ADD : '+';
SUB : '-';

WS
	:	(' ' | '\r' | '\n') -> channel(HIDDEN) // glaube Zeilenumbruch muss registriert werden....
	;
