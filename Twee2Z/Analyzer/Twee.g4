parser grammar Twee;
options {   tokenVocab = LEX; }

start
	: ignoreFirst passage*
	; 

ignoreFirst
	: passage
	| ~PASS ignoreFirst 
	;

passage
	: PASS PMODEWORD TAG? ~PMODE_END* PMODE_END passageContent?
	;

passageContent
	:  innerPassageContent+
	;

innerPassageContent
	: (macro|text|link) 
	;

link
	: LINK_START ((LTARGET|LEXCLUDE)+ PIPE)? (FUNC_LINK|LTARGET) ((SQ_BRACKET_CLOSE SQ_BRACKET_OPEN expression)|LINK_END)
	;

macro
	: MACRO_START (DISPLAY|SET|PRINT) expression
	| MACRO_START  (ACTIONS text | CHOICE link?) MACRO_END 
	| macroBranchIf macroBranchIfElse* macroBranchElse? macroBranchPop
	| MACRO_START NOBR MACRO_END passageContent MACRO_START ENDNOBR MACRO_END
	| MACRO_START SILENTLY MACRO_END passageContent MACRO_START ENDSILENTLY MACRO_END 
	;

macroBranchIf
	: MACRO_START IF expression passageContent
	;

macroBranchIfElse
	: MACRO_START ELSE_IF expression passageContent
	;

macroBranchElse
	: MACRO_START ELSE MACRO_END passageContent
	;

macroBranchPop
	: MACRO_START ENDIF MACRO_END
	;

expression
	: EXPRESSION
	;

/*function
	: (FUNC_START FUNC_BRACKET_OPEN (EXPRESSION|EXPRESSION FUNC_PARAM+)? FUNC_BRACKET_CLOSE)
	;

variable
	: VAR_NAME
	;
*/
zeichenkette
	: WORD+
	;

text
	: (zeichenkette|SPACE+|NEW_LINE|INT|FORMAT|EXCLUDE)
	; 