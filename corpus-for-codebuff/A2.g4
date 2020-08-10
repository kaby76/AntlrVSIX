grammar A2;
s : e ;
e : e '*' ( e ) | int ;
int : INT ;
INT : [0-9]+ ;
WS : [ \t\n]+ -> skip ;
