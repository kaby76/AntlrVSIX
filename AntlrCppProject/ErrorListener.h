#pragma once

// Template generated code from Antlr4BuildTasks.Template v 2.0

#include <string>
#include <iostream>
#include "ANTLRInputStream.h"
#include "CommonTokenStream.h"
#include "tree/ParseTree.h"
#include "tree/TerminalNode.h"
#include "tree/TerminalNodeImpl.h"
#include "misc/Interval.h"
#include "ConsoleErrorListener.h"

namespace $safeprojectname$
{

    class ErrorListener : public antlr4::ConsoleErrorListener
    {
    private:
        bool had_error;
    public:
        void syntaxError(antlr4::Recognizer* recognizer, antlr4::Token* offendingSymbol, size_t line, size_t col, const std::string& msg, std::exception_ptr e) override;
    };

}

