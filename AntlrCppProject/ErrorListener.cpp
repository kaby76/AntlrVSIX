// Template generated code from Antlr4BuildTasks.Template v 2.0

#include "ErrorListener.h"

void $safeprojectname$::ErrorListener::syntaxError(antlr4::Recognizer* recognizer, antlr4::Token* offendingSymbol, size_t line, size_t col, const std::string& msg, std::exception_ptr e)
{
    had_error = true;
    antlr4::ConsoleErrorListener::syntaxError(recognizer, offendingSymbol, line, col, msg, e);
}
