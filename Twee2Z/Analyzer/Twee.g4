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
	: (macro|function|text|variable|link) innerPassageContent
	| (macro|function|text|variable|link)
	;

link
	: LINK_START (WORDS PIPE)? (FUNC_LINK|WORDS) (SQ_BRACKET_CLOSE SQ_BRACKET_OPEN WORDS)? LINK_END
	;

passageTags
	: TAGS
	;

/* TODO: weder ACTIONS noch CHOICE akzeptieren etwas. Mode-Problem?*/
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

text
	: innerText
	;

innerText
	: (WORD|NEW_LINE|SPACE|INT|STRING|format) innerText
	| (WORD|NEW_LINE|SPACE|INT|STRING|format)
	;

format
	: (ITALIC_BEGIN	(ITALIC_TEXT_SWITCH | ITALIC_TEXT) ITALIC_END)
	| (UNDERLINE_BEGIN (UNDERLINE_TEXT_SWITCH | UNDERLINE_TEXT) UNDERLINE_END)
	| (STRIKEOUT_BEGIN (STRIKEOUT_TEXT_SWITCH | STRIKEOUT_TEXT)	STRIKEOUT_END)	
	| (SUPERSCRIPT_BEGIN (SUPERSCRIPT_TEXT_SWITCH | SUPERSCRIPT_TEXT) SUPERSCRIPT_END)
	| (SUBSCRIPT_BEGIN (SUBSCRIPT_TEXT_SWITCH | SUBSCRIPT_TEXT) SUBSCRIPT_END)
	| (MONOSPACE_BEGIN (MONOSPACE_TEXT_SWITCH | MONOSPACE_TEXT) MONOSPACE_END)
	| (COMMENT_BEGIN (COMMENT_TEXT_SWITCH | COMMENT_TEXT) COMMENT_END)
	;