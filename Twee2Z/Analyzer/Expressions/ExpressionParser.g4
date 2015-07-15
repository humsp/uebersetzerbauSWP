parser grammar ExpressionParser;
options {   tokenVocab = ExpressionLexer; }

expression
	: WS* s0
	;

s0
	: VAR_NAME WS* assign s0
	| s1
	;

s1
	: (s2 WS* opAnd s1)
	| s2
	;

s2
	: s3 WS* opOr s2
	| s3
	;

s3 
	: s4 WS* opXor s3
	| s4
	;

s4
	: s5 WS* opCompare s4
	| s5
	;

s5
	: s6 WS* opMod s5
	| s6
	;

s6
	: s7 WS* opPrio6 s6
	| s7
	;

s7
	: s8 WS* opPrio7 s7
	| s8
	;

s8 
	: WS* opPrio8Not s8
	| WS* s9
	;

s9
	: BRACKET_OPEN WS? s0 WS? BRACKET_CLOSE
	| value
	;


value
	: function
	| TRUE
	| FALSE
	| DIGITS
	| DIGITS DOT DIGITS
	| DOT DIGITS
	| VAR_NAME
	| STRING
	;

/* assign op prio 0*/
assign
	: ASSIGN_EQ
	| ASSIGN_SUB
	| ASSIGN_ADD
	| ASSIGN_MUL
	| ASSIGN_DIV
	| ASSIGN_MOD
	;

// op prio 1
opCompare
	: NEQ
	| EQ
	| GT
	| GE
	| LT
	| LE
	;

// op prio 2
opAnd
	: OP_LOG_AND
	;

// op prio 3
opOr
	: OP_LOG_OR
	;
	
// op prio 4
opXor
	: OP_LOG_XOR
	;

// op prio 5
opMod
	: OP_MOD
	;

// op prio 6
opPrio6
	: OP_SUB
	| OP_ADD
	;

// op prio 7
opPrio7
	: OP_MUL 
	| OP_DIV
	;

// op prio 8
opPrio8Not
	: OP_LOG_NOT
	;
	

/* function */
function
	: functionName WS? BRACKET_OPEN ((WS? functionArg WS? COMMA)* WS? functionArg WS?)? BRACKET_CLOSE
	;

functionName
	: FCN_RANDOM
	| FCN_PREVIOUS
	| FCN_VISITED
	| FCN_VISIT_TAG
	| FCN_TURNS
	| FCN_PASSAGE
	| FCN_CONFIRM
	| FCN_PROMT
	| FCN_ALERT
	| FCN_PARAMETER
	| FCN_EITHER
	;

functionArg
	: s0
	;