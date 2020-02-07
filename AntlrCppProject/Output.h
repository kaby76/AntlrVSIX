// Template generated code from Antlr4BuildTasks.Template v 2.1

#pragma once

#include <CommonTokenStream.h>
#include <Token.h>
#include <support/CPPUtils.h>

namespace $safeprojectname$
{
    class Output
    {
        static int changed;
        static bool first_time;
    public:
        static std::string OutputTokens(antlr4::CommonTokenStream& stream);
        static std::string OutputTree(antlr4::tree::ParseTree& tree, antlr4::CommonTokenStream& stream);
        static void ParenthesizedAST(antlr4::tree::ParseTree& tree, std::string& sb, antlr4::CommonTokenStream& stream, int level = 0);
        static void StartLine(std::string& sb, antlr4::tree::ParseTree& tree, antlr4::CommonTokenStream& stream, int level = 0);
        static bool replace(std::string& str, const std::string& from, const std::string& to);
        static std::string ToLiteral(std::string& input);
        static std::string PerformEscapes(std::string s);
    };
}
