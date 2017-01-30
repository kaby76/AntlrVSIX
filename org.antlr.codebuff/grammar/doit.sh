#!/bin/bash
echo building grammar files.
export i=antlr-4.6-complete.jar
echo i is $i
for g in *.g4; do
 echo $g
 java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -Dlanguage=CSharp -visitor $g
 java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -visitor $g
done
/cygdrive/c/Program\ Files/Java/jdk1.8.0_102/bin/javac.exe -cp ".;h:\\Downloads\\$i" *.java
echo ""
