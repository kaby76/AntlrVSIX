// Template generated code from Antlr4BuildTasks.Template v 3.0
#include <iostream>
#include "ANTLRInputStream.h"
#include "arithmeticLexer.h"
#include "arithmeticParser.h"
#include "Output.h"
#include "ErrorListener.h"

void Try(std::string str)
{
	antlr4::ANTLRInputStream input(str);
	arithmeticLexer lexer(&input);
	antlr4::CommonTokenStream tokens(&lexer);
	arithmeticParser parser(&tokens);
	$safeprojectname$::ErrorListener listener;
	parser.addErrorListener(&listener);
	arithmeticParser::FileContext* tree = parser.file();
	std::cout << $safeprojectname$::Output::OutputTokens(tokens);
	std::cout << $safeprojectname$::Output::OutputTree(*tree, tokens);
}

int main()
{
	Try("1 + 2 * 3");
}

