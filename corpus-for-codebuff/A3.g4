grammar A2;

a
    : e {System.out.println($e.v);}
    ;

e
    : a1=e '*' b1=e {$v = $a1.v * $b1.v;}
    | a2=e '+' b2=e {$v = $a2.v + $b2.v;}
    | INT         {$v = $INT.int;}
    | '(' x=e ')' {$v = $x.v;}
    ;

INT
    : [0-9]+
    ;

WS
    : [ \t\n]+ -> skip
    ;
