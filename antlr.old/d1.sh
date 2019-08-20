#!/bin/bash

if [ $# -eq 0 ]
then
    echo This script generates a parser.
    echo Use d2.sh to test the parser after running this script.
    echo Example: d1.sh AntlrLexer.g4 AntlrParser.g4
    exit 1
fi

echo building ANTLR parser.

rm -rf *.tokens
rm -rf *.class

export i=antlr-4.6-complete.jar
echo i is $i
for g in $@; do
 echo compiling $g
 java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -Dlanguage=CSharp -visitor $g
 java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -visitor $g
done
/cygdrive/c/Program\ Files/Java/jdk1.8.0_102/bin/javac.exe -cp ".;h:\\Downloads\\$i" *.java
exit
