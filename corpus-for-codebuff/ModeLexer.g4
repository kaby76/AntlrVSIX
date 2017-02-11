lexer grammar ModeLexer;

INT
    : [0-9]+
    ;

WS
    : [ \t\n]+ -> skip
    ;

mode AA; // E.g., [int x, List<String> a[]]

    NESTED_ARG_ACTION
        :   '['                         -> more, pushMode(AA)
        ;

    ARG_ACTION_ESCAPE
        :   '\\' .                      -> more
        ;

    ARG_ACTION_STRING_LITERAL
        :   ('"' ('\\' . | ~["\\])* '"')-> more
        ;

    ARG_ACTION_CHAR_LITERAL
        :   ('"' '\\' . | ~["\\] '"')   -> more
        ;

    ARG_ACTION
        :   ']'                         -> popMode
        ;

    UNTERMINATED_ARG_ACTION // added this to return non-EOF token type here. EOF did something weird
        :   EOF                         -> popMode
        ;

    ARG_ACTION_CHAR // must be last
        :   .                           -> more
        ;

