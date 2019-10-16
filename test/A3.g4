grammar A3;

s
    : e
    ;

e
    : e '*' e 		# Mult
    | INT      		# primary
    ;

IdNT
    : [0-9]+
    ;

WS
    : [ \t\n]+ -> skip
    ;
