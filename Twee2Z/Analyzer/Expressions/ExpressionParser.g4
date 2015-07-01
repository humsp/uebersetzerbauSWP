parser grammar ExpressionParser;
options {   tokenVocab = ExpressionLexer; }

expression
	//: WS* NAME TODO in link
	: WS* expr
	;

/* base expression  */
expr
	: exprR
	| VAR_NAME assign expr
	;

/* expression R (right from op) */
exprR
	: exprROpUnary
	| exprRContent
	;

/* expression R (right from op) with op*/

exprROpUnary
	: (opUnary WS?)* exprRContent
	;

exprROp
	: op WS? (opUnary WS?)* exprRContent
	;

exprRContent
	: value
	| value WS? exprR
	| exprRBracket WS? exprR
	;

exprRBracket
	: BRACKET_OPEN WS? expr WS? BRACKET_CLOSE
	;

value
	: function
	| DIGITS
	| DIGITS DOT DIGITS
	| VAR_NAME
	| STRING
	;

opUnary
	: OP_SUB
	| OP_ADD
	| OP_LOG_NOT+
	;

op
	: OP_LOG_AND
	| OP_LOG_OR
	| OP_LOG_XOR
	| OP_MUL
	| OP_DIV
	| OP_MOD
	| NEQ
	| EQ
	| GT
	| GE
	| LT
	| LE
	;


/* assign */
assign
	: ASSIGN_EQ
	| ASSIGN_SUB
	| ASSIGN_ADD
	| ASSIGN_MUL
	| ASSIGN_DIV
	| ASSIGN_MOD
	;

/* function */
function
	: NAME WS? BRACKET_OPEN functionArg? BRACKET_CLOSE
	;

functionArgs
	: (WS? functionArg WS? COMMA)* WS? functionArg WS? 
	;

functionArg
	: expr
	;