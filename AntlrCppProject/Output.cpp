// Template generated code from Antlr4BuildTasks.Template v 3.0

#include "Output.h"
#include <string>
#include <CommonTokenStream.h>
#include "tree/ParseTree.h"
#include "tree/TerminalNode.h"
#include "tree/TerminalNodeImpl.h"
#include "misc/Interval.h"

int $safeprojectname$::Output::changed = 0;
bool $safeprojectname$::Output::first_time = true;

std::string $safeprojectname$::Output::OutputTokens(antlr4::CommonTokenStream& stream)
{
    std::string sb;
    std::vector<antlr4::Token*> x = stream.getTokens();
    for (std::vector<antlr4::Token*>::iterator it = x.begin(); it != x.end(); ++it)
    {
        antlr4::Token* t = *it;
        antlr4::Token& token = *t;
        sb += ("Token "
            + std::to_string(token.getTokenIndex()) + " "
            + std::to_string((long)(token.getType())) + " "
            + PerformEscapes(token.getText()));
        sb += "\n";
    }
    return sb;
}

std::string $safeprojectname$::Output::OutputTree(antlr4::tree::ParseTree& tree, antlr4::CommonTokenStream& stream)
{
    std::string sb;
    ParenthesizedAST(tree, sb, stream);
    return sb;
}

void $safeprojectname$::Output::ParenthesizedAST(antlr4::tree::ParseTree& tree, std::string& sb, antlr4::CommonTokenStream& stream, int level)
{
    // Antlr always names a non-terminal with first letter lowercase,
    // but renames it when creating the type in C#. So, remove the prefix,
    // lowercase the first letter, and remove the trailing "Context" part of
    // the name. Saves big time on output!
    if (antlrcpp::is<antlr4::tree::TerminalNodeImpl*>(&tree))
    {
        antlr4::tree::TerminalNodeImpl* tok = dynamic_cast<antlr4::tree::TerminalNodeImpl*>(&tree);
        antlr4::misc::Interval interval = tok->getSourceInterval();
        std::vector<antlr4::Token*> * inter = nullptr;
        if (tok->getSymbol()->getTokenIndex() >= 0)
            inter = &stream.getHiddenTokensToLeft(tok->getSymbol()->getTokenIndex());
        if (inter != nullptr)
        {
            for (auto z = inter->begin(); z != inter->end(); ++z)
            {
                auto t = *z;
                StartLine(sb, tree, stream, level);
                sb.append("( HIDDEN text=" + PerformEscapes(t->getText()));
                sb.append("\n");
            }
        }
        StartLine(sb, tree, stream, level);
        sb.append("( TOKEN i=" + std::to_string(tree.getSourceInterval().a)
            + " txt=" + PerformEscapes(tree.getText())
            + " tt="
            + std::to_string(tok->getSymbol()->getType()));
        sb.append("\n");
    }
    else
    {
        std::string fixed_name = typeid(tree).name();
//        replace(fixed_name, "parsers::MySQLParser::", "");
        StartLine(sb, tree, stream, level);
        sb.append("( " + fixed_name);
        sb.append("\n");
    }
    for (auto ci = tree.children.begin(); ci != tree.children.end(); ++ci)
    {
        std::vector<antlr4::tree::ParseTree*>::value_type c = *ci;
        ParenthesizedAST(*c, sb, stream, level + 1);
    }
    if (level == 0)
    {
        for (int k = 0; k < 1 + changed - level; ++k) sb.append(") ");
        sb.append("\n");
        changed = 0;
    }
}

void $safeprojectname$::Output::StartLine(std::string& sb, antlr4::tree::ParseTree& tree, antlr4::CommonTokenStream& stream, int level)
{
    if (changed - level >= 0)
    {
        if (!first_time)
        {
            for (int j = 0; j < level; ++j) sb.append("  ");
            for (int k = 0; k < 1 + changed - level; ++k) sb.append(") ");
            sb.append("\n");
        }
        changed = 0;
        first_time = false;
    }
    changed = level;
    for (int j = 0; j < level; ++j) sb.append("  ");
}

bool $safeprojectname$::Output::replace(std::string& str, const std::string& from, const std::string& to) {
    size_t start_pos = str.find(from);
    if (start_pos == std::string::npos)
        return false;
    str.replace(start_pos, from.length(), to);
    return true;
}

std::string $safeprojectname$::Output::ToLiteral(std::string& input)
{
    std::string literal = input;
    replace(literal, "\\", "\\\\");
    replace(literal, "\b", "\\b");
    replace(literal, "\n", "\\n");
    replace(literal, "\t", "\\t");
    replace(literal, "\r", "\\r");
    replace(literal, "\f", "\\f");
    replace(literal, "\"", "\\\"");
    // replace(literal, Format("\" +{0}\t\"", Environment.NewLine), "");
    return literal;
}

std::string $safeprojectname$::Output::PerformEscapes(std::string s)
{
    std::string new_s;
    new_s.append(ToLiteral(s));
    return new_s;
}
