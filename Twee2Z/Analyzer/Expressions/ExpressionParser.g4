parser grammar ExpressionParser;
options {   tokenVocab = ExpressionLexer; }

expression
	: NAME
	| expr
	;

expr
	: exprR
	| VAR_NAME assign exprR
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

/* expression  */
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
	| value WS? exprROp
	| exprRBracket WS? exprROp
	;

exprRBracket
	: BRACKET_OPEN WS? exprR+ WS? BRACKET_CLOSE
	;

value
	: function
	| DIGITS
	| VAR_NAME
	| STRING
	;

opUnary
	: OP_SUB
	| OP_ADD
	| OP_LOG_NOT+
	| OP_LOG_NOT2+
	;

op
	: OP_LOG_AND
	| OP_LOG_AND2
	| OP_LOG_OR
	| OP_LOG_OR2
	| OP_LOG_XOR
	| OP_LOG_XOR2
	| OP_MUL
	| OP_DIV
	| EQ_IS
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
	| ASSIGN_TO
	| EQ_IS
	;