using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanguageServer
{
    // Class to offer Antlr tree edits, both in-place and out-of-place,
    // and tree copying.
    public class TreeEdits
    {
        public delegate IParseTree Fun(in IParseTree arg1, out bool arg2);

        public static System.Collections.Generic.IEnumerable<IParseTree> FindTopDown(
            IParseTree tree,
            Fun find)
        {
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                var found = find(n, out bool @continue);
                if (found != null)
                    yield return found;
                if (!@continue) { }
                else if (n as TerminalNodeImpl != null) { }
                else
                {
                    for (int i = n.ChildCount - 1; i >= 0; --i)
                    {
                        var c = n.GetChild(i);
                        stack.Push(c);
                    }
                }
            }
        }

        public static bool Replace(IParseTree tree, Func<IParseTree, IParseTree> replace)
        {
            var replacement = replace(tree);
            if (replacement != null)
            {
                IParseTree parent = tree.Parent;
                var c = parent as ParserRuleContext;
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == tree)
                    {
                        var temp = c.children[i];
                        if (temp is TerminalNodeImpl)
                        {
                            var t = temp as TerminalNodeImpl;
                            t.Parent = null;
                            c.children[i] = replacement;
                            var r = replacement as TerminalNodeImpl;
                            r.Parent = c;
                        }
                        else if (temp is ParserRuleContext)
                        {
                            var t = temp as ParserRuleContext;
                            t.Parent = null;
                            c.children[i] = replacement;
                            var r = replacement as ParserRuleContext;
                            r.Parent = c;
                        }
                        else
                            throw new LanguageServerException("Tree contains something other than TerminalNodeImpl or ParserRuleContext");
                        break;
                    }
                }
                return true; // done.
            }
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return true;
                else
                    return false;
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    if (Replace(c, replace))
                        return true;
                }
            }
            return false;
        }

        public static bool InsertAfter(IParseTree tree, Func<IParseTree, IParseTree> insert_point)
        {
            var insert_this = insert_point(tree);
            if (insert_this != null)
            {
                IParseTree parent = tree.Parent;
                var c = parent as ParserRuleContext;
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == tree)
                    {
                        c.children.Insert(i+1, insert_this);
                        var r = insert_this as ParserRuleContext;
                        r.Parent = c;
                        break;
                    }
                }
                return true; // done.
            }
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return true;
                else
                    return false;
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    if (InsertAfter(c, insert_point))
                        return true;
                }
            }
            return false;
        }

        public static bool Delete(IParseTree tree, Func<IParseTree, bool> delete)
        {
            bool delete_this = delete(tree);
            if (delete_this)
            {
                IParseTree parent = tree.Parent;
                var c = parent as ParserRuleContext;
                for (int i = 0; i < c.ChildCount; ++i)
                {
                    var child = c.children[i];
                    if (child == tree)
                    {
                        var temp = c.children[i];
                        var t = temp as ParserRuleContext;
                        t.Parent = null;
                        c.children.RemoveAt(i);
                        break;
                    }
                }
                return true; // done.
            }
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return true;
                else
                    return false;
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    if (Delete(c, delete))
                        return true;
                }
            }
            return false;
        }

        public static TerminalNodeImpl LeftMostToken(IParseTree tree)
        {
            if (tree is TerminalNodeImpl)
                return tree as TerminalNodeImpl;
            for (int i = 0; i < tree.ChildCount; ++i)
            {
                var c = tree.GetChild(i);
                if (c == null)
                    return null;
                var lmt = LeftMostToken(c);
                if (lmt != null)
                    return lmt;
            }
            return null;
        }

        public static TerminalNodeImpl RightMostToken(IParseTree tree)
        {
            if (tree is TerminalNodeImpl)
                return tree as TerminalNodeImpl;
            for (int i = tree.ChildCount - 1; i >= 0; --i)
            {
                var c = tree.GetChild(i);
                if (c == null)
                    return null;
                var lmt = RightMostToken(c);
                if (lmt != null)
                    return lmt;
            }
            return null;
        }

        public static string GetText(IList<IToken> list)
        {
            if (list == null)
                return "";
            StringBuilder sb = new StringBuilder();
            foreach (var l in list)
            {
                sb.Append(l.Text);
            }
            return sb.ToString();
        }

        public static (Dictionary<TerminalNodeImpl, string>, List<string>) TextToLeftOfLeaves(CommonTokenStream stream, IParseTree tree)
        {
            var result = new Dictionary<TerminalNodeImpl, string>();
            var result2 = new List<string>();
            Stack<IParseTree> stack = new Stack<IParseTree>();
            stack.Push(tree);
            while (stack.Any())
            {
                var n = stack.Pop();
                if (n is TerminalNodeImpl)
                {
                    var nn = n as TerminalNodeImpl;
                    {
                        var p1 = TreeEdits.LeftMostToken(nn).SourceInterval.a;
                        var p2 = stream.GetHiddenTokensToLeft(p1);
                        var p3 = TreeEdits.GetText(p2);
                        result.Add(nn, p3);
                        result2.Add(nn.GetText());
                    }
                }
                else
                {
                    if (!(n is ParserRuleContext p))
                        continue;
                    if (p.children == null)
                        continue;
                    if (p.children.Count == 0)
                        continue;
                    foreach (var c in p.children.Reverse())
                    {
                        stack.Push(c);
                    }
                }
            }
            return (result, result2);
        }

        public static IParseTree CopyTreeRecursive(IParseTree original, IParseTree parent, Dictionary<TerminalNodeImpl, string> text_to_left)
        {
            if (original == null) return null;
            else if (original is TerminalNodeImpl)
            {
                var o = original as TerminalNodeImpl;
                var new_node = new TerminalNodeImpl(o.Symbol);
                if (text_to_left != null)
                {
                    if (text_to_left.TryGetValue(o, out string value))
                        text_to_left.Add(new_node, value);
                }
                if (parent != null)
                {
                    var parent_rule_context = (ParserRuleContext)parent;
                    new_node.Parent = parent_rule_context;
                    parent_rule_context.AddChild(new_node);
                }
                return new_node;
            }
            else if (original is ParserRuleContext)
            {
                var type = original.GetType();
                var new_node = (ParserRuleContext)Activator.CreateInstance(type, null, 0);
                if (parent != null)
                {
                    var parent_rule_context = (ParserRuleContext)parent;
                    new_node.Parent = parent_rule_context;
                    parent_rule_context.AddChild(new_node);
                }
                int child_count = original.ChildCount;
                for (int i = 0; i < child_count; ++i)
                {
                    var child = original.GetChild(i);
                    CopyTreeRecursive(child, new_node, text_to_left);
                }
                return new_node;
            }
            else return null;
        }

        public static void Reconstruct(StringBuilder sb, IParseTree tree, Dictionary<TerminalNodeImpl, string> text_to_left)
        {
            if (tree as TerminalNodeImpl != null)
            {
                TerminalNodeImpl tok = tree as TerminalNodeImpl;
                text_to_left.TryGetValue(tok, out string inter);
                if (inter == null)
                    sb.Append(" ");
                else
                    sb.Append(inter);
                if (tok.Symbol.Type == TokenConstants.EOF)
                    return;
                sb.Append(tok.GetText());
            }
            else
            {
                for (int i = 0; i < tree.ChildCount; ++i)
                {
                    var c = tree.GetChild(i);
                    Reconstruct(sb, c, text_to_left);
                }
            }
        }
    }
}
