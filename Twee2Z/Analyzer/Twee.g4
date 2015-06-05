parser grammar Twee;
/*
 * Parser Rules
 */

options {   tokenVocab = LEX; }

/*
 * IMPORTANT: Don't use fragments in parser rules!
 */

start
	: innerText? passage+
	; 

passage
	: passageStart SPACE* passageName SPACE* passageTags? (WORD|SPACE|INT|STRING)* (NEW_LINE|NEW_LINE passageContent)?
	;
	
passageStart
	: PASS
	;

passageName
	:  (WORD|INT)+
	;

passageContent
	:  innerPassageContent	
	;

innerPassageContent
	: (function|text|variable|macro|link) innerPassageContent
	| (function|text|variable|macro|link)
	;

link
	: LINK_START (WORDS PIPE)? (FUNC_LINK|WORDS) (SQ_BRACKET_CLOSE SQ_BRACKET_OPEN WORDS)? LINK_END
	;

passageTags
	: TAGS
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
