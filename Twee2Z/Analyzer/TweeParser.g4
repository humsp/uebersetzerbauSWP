parser grammar TweeParser;

/*
 * Parser Rules
 */

 tokens {  }

/*
 * IMPORTANT: Don't use fragments in parser rules!
 */

start
	: .* passage+
	; 

passage
	: passageStart SPACE* passageName SPACE* (NEW_LINE passageContent)?
	;
	
passageStart
	: DOUBLE_DOT +
	;

passageName
	:  (WORD|INT)+
	;

passageContent
	:  innerPassageContent	
	;

innerPassageContent
	: (link|function|text|variable|macro) innerPassageContent
	| (function|text|variable|macro|link)
	;

link
	: LINK_START (WORDS PIPE)? (WORDS|FUNC_LINK) LINK_END
	;
macro
	: MACRO_FORM
	;
	
function
	: FUNC_FORM
	;

variable
	: VAR_NAME
	;

text
	: innerText
	;

innerText
	: (WORD|NEW_LINE|SPACE|INT|STRING) innerText
	| (WORD|NEW_LINE|SPACE|INT|STRING)
	;
