parser grammar Twee;
/*
 * Parser Rules
 */

options {   tokenVocab = LEX; }

/*
 * IMPORTANT: Don't use fragments in parser rules!
 */

// TODO: ignore everything before start
start
	: passage+
	; 

passage
	: passageStart SPACE* passageName SPACE* passageTags? (WORD|SPACE|INT|STRING)* (NEW_LINE|NEW_LINE passageContent)?
	;
	
passageStart
	: PASS
	;

passageName
	:  (WORD|INT|SPACE)+
	;
	catch [RecognitionException] {  }

passageContent
	:  innerPassageContent	
	;

innerPassageContent
	: (macro|function|text|variable|link|format) innerPassageContent
	| (macro|function|text|variable|link|format)
	;

link
	: LINK_START (WORDS PIPE)? (FUNC_LINK|WORDS) (SQ_BRACKET_CLOSE SQ_BRACKET_OPEN WORDS)? LINK_END
	;

passageTags
	: TAGS
	;

/* TODO: neither ACTIONS or CHOICE are accepted */
macro
	: MACRO_START (((DISPLAY|SET|PRINT) (EXPRESSION_FORM)?)| (ACTIONS STRING?) | CHOICE link?) MACRO_END 
	| MACRO_START IF (STRING) MACRO_END passageContent (MACRO_START ELSE_IF (STRING) MACRO_END passageContent)? (MACRO_START ELSE MACRO_END passageContent)? MACRO_START ENDIF MACRO_END
	| MACRO_START NOBR MACRO_END passageContent MACRO_START ENDNOBR MACRO_END
	| MACRO_START SILENTLY MACRO_END passageContent MACRO_START ENDSILENTLY MACRO_END 
	;
	
function
	: (FUNC_START FUNC_BRACKET_OPEN FUNC_PARAM* FUNC_BRACKET_CLOSE)
	;

variable
	: VAR_NAME
	;

format
	: (ITALIC_BEGIN	(ITALIC_TEXT_SWITCH | ITALIC_TEXT | passageContent)* ITALIC_END)
	| (UNDERLINE_BEGIN (UNDERLINE_TEXT_SWITCH | UNDERLINE_TEXT | passageContent)* UNDERLINE_END)
	| (STRIKEOUT_BEGIN (STRIKEOUT_TEXT_SWITCH | STRIKEOUT_TEXT | passageContent)*	STRIKEOUT_END)	
	| (SUPERSCRIPT_BEGIN (SUPERSCRIPT_TEXT_SWITCH | SUPERSCRIPT_TEXT | passageContent)* SUPERSCRIPT_END)
	| (SUBSCRIPT_BEGIN (SUBSCRIPT_TEXT_SWITCH | SUBSCRIPT_TEXT | passageContent)* SUBSCRIPT_END)
	| (MONOSPACE_BEGIN (MONOSPACE_TEXT_SWITCH | MONOSPACE_TEXT | passageContent)* MONOSPACE_END)
	| (COMMENT_BEGIN (COMMENT_TEXT_SWITCH | COMMENT_TEXT | passageContent)* COMMENT_END)
	;

//TODO: last symbol is always put on new line
text
	: innerText
	;

innerText
	: (WORD|NEW_LINE|SPACE|INT|STRING) innerText
	| (WORD|NEW_LINE|SPACE|INT|STRING)
	;