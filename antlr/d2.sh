#!/bin/bash

if [ $# -eq 0 ]
then
    echo This script executes TestRig for a generated parser.
    echo Use d1.sh to generate the parser first.
    echo Example: d2.sh JSON json -gui example.json
    exit 1
fi

alias grun='java org.antlr.v4.runtime.misc.TestRig'

export i=antlr-4.6-complete.jar
echo i is $i
java -cp ".;h:\\Downloads\\$i"  org.antlr.v4.gui.TestRig  $@

