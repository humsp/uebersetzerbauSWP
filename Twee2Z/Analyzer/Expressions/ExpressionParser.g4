parser grammar ExpressionParser;
options {   tokenVocab = ExpressiosLexer; }

expr
	: NAME
	| exprR
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


/* expression R */
exprR
	: (op WS)* op? exprROp
	;

exprROp
	: value WS? (op WS)* op? exprR
	| exprRBracket
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

op
	: OP_LOG_AND
	| OP_LOG_AND2
	| OP_LOG_OR
	| OP_LOG_OR2
	| OP_LOG_XOR
	| OP_LOG_XOR2
	| OP_LOG_NOT
	| OP_LOG_NOT2
	| OP_SUB
	| OP_ADD
	| OP_MUL
	| OP_DIV
	|
	;




/* assign */
assing
	: ASSIGN_EQ
	| ASSIGN_SUB
	| ASSIGN_ADD
	| ASSIGN_MUL
	| ASSIGN_DIV
	| ASSIGN_TO
	| EQ_IS
	;