parser grammar Twee;
options {   tokenVocab = LEX; }

/* Every twee file must begin with a passage Name and everything before discarded*/
start
	: ignoreFirst passage*
	; 

/* Everything is ignored if no '::' appears */
ignoreFirst
	: passage
	| ~PASS ignoreFirst
	;

/* A passage consists of : 
	::* Name [tags]*´WORDS*
	passageContent?
*/
passage
	: PASS PMODEWORD TAG? ~PMODE_END* PMODE_END passageContent?
	;

/* The passage Content consists of plain text (can be formatted), macros, functions, variables and links */
passageContent
	:  (macro|function|text|variable|link) passageContent?
	;

link
	: LINK_START (WORDS PIPE)? (FUNC_LINK|WORDS) ((SQ_BRACKET_CLOSE SQ_BRACKET_OPEN EXPRESSION EXP_END_L)|LINK_END)
	;

macro
	: MACRO_START (DISPLAY|SET|PRINT) EXPRESSION EXP_END_M
	| MACRO_START  (ACTIONS text | CHOICE link?) MACRO_END 
	| MACRO_START IF EXPRESSION EXP_END_M passageContent (MACRO_START ELSE_IF EXPRESSION EXP_END_M passageContent)? (MACRO_START ELSE MACRO_END passageContent)? MACRO_START ENDIF MACRO_END
	| MACRO_START NOBR MACRO_END passageContent MACRO_START ENDNOBR MACRO_END
	| MACRO_START SILENTLY MACRO_END passageContent MACRO_START ENDSILENTLY MACRO_END 
	;
	
function
	: (FUNC_START FUNC_BRACKET_OPEN (EXPRESSION|EXPRESSION FUNC_PARAM+)? EXP_END)
	;

variable
	: VAR_NAME
	;

zeichenkette
	:WORD+
	;

text
	: (zeichenkette|SPACE+|NEW_LINE|INT|FORMAT|EXCLUDE|STRING) text?
	; 