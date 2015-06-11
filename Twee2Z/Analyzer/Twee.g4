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
	: passageStart SPACE* passageName passageTags? (NEW_LINE|NEW_LINE passageContent)?
	;
passageStart
	: PASS
	;
	
passageName
	:  ((WORD|INT)+ SPACE*)+ //alles auﬂer startsonderzeichen
	;
	//catch [RecognitionException] {  }

passageContent
	:  (macro|function|text|variable|link) passageContent?
	;


link
	: LINK_START (WORDS PIPE)? (FUNC_LINK|WORDS) (SQ_BRACKET_CLOSE SQ_BRACKET_OPEN WORDS)? LINK_END
	;

passageTags
	: TAGS
	;

/* TODO: neither ACTIONS or CHOICE are accepted */
macro
	: MACRO_START (((DISPLAY|SET|PRINT))| (ACTIONS STRING?) | CHOICE link?) MACRO_END 
	| MACRO_START IF (VAR_NAME) MACRO_END passageContent (MACRO_START ELSE_IF (STRING) MACRO_END passageContent)? (MACRO_START ELSE MACRO_END passageContent)? MACRO_START ENDIF MACRO_END
	| MACRO_START NOBR MACRO_END passageContent MACRO_START ENDNOBR MACRO_END
	| MACRO_START SILENTLY MACRO_END passageContent MACRO_START ENDSILENTLY MACRO_END 
	;
	
function
	: (FUNC_START FUNC_BRACKET_OPEN FUNC_PARAM* FUNC_BRACKET_CLOSE)
	;

variable
	: VAR_NAME
	;
zeichenkette
	:WORD+
	;
text
	: innerText
	;

innerText
	: (zeichenkette|SPACE+|NEW_LINE|INT|FORMAT|EXCLUDE|STRING) innerText?
	;
/*
	: (ITALIC_BEGIN	(ITALIC_TEXT_SWITCH | ITALIC_TEXT | passageContent)* ITALIC_END)
	| (UNDERLINE_BEGIN (UNDERLINE_TEXT_SWITCH | UNDERLINE_TEXT | passageContent)* UNDERLINE_END)
	| (STRIKEOUT_BEGIN (STRIKEOUT_TEXT_SWITCH | STRIKEOUT_TEXT | passageContent)*	STRIKEOUT_END)	
	| (SUPERSCRIPT_BEGIN (SUPERSCRIPT_TEXT_SWITCH | SUPERSCRIPT_TEXT | passageContent)* SUPERSCRIPT_END)
	| (SUBSCRIPT_BEGIN (SUBSCRIPT_TEXT_SWITCH | SUBSCRIPT_TEXT)* SUBSCRIPT_END)
	| (MONOSPACE_BEGIN (format)* MONOSPACE_END)
	| (COMMENT_BEGIN (COMMENT_TEXT_SWITCH | COMMENT_TEXT | passageContent)* COMMENT_END)
	;
*/
//TODO: last symbol is always put on new line
