#!/bin/bash
echo building grammar files.
#java -cp '.;h:\Downloads\antlr-4.5.3-complete.jar' org.antlr.v4.Tool -Dlanguage=CSharp -visitor CSharp4.g4
#java -cp '.;h:\Downloads\antlr-4.5.3-complete.jar' org.antlr.v4.Tool -Dlanguage=CSharp CSharp4Lexer.g4

for i in antlr-4.6-complete.jar; do
echo i is $i

rm -rf *.tokens
rm -rf *.class
rm -rf ANTLR*.java

java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -Dlanguage=CSharp -visitor ANTLRv4Lexer.g4
java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -visitor ANTLRv4Lexer.g4

java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -Dlanguage=CSharp -visitor ANTLRv4Parser.g4
java -cp ".;h:\\Downloads\\$i" org.antlr.v4.Tool -visitor ANTLRv4Parser.g4

/cygdrive/c/Program\ Files/Java/jdk1.8.0_102/bin/javac.exe -cp ".;h:\\Downloads\\$i" *.java
java -cp ".;h:\\Downloads\\$i" Test
echo ""
exit

done
