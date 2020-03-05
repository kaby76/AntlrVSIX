namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Utils
    {
        /// <summary>
        /// Return first ancestor node up the chain towards the root that has ruleName.
        ///  Search includes the current node.
        /// </summary>
        public static ParserRuleContext getAncestor(Parser parser, ParserRuleContext ctx, string ruleName)
        {
            int ruleIndex = parser.GetRuleIndex(ruleName);
            return getAncestor(ctx, ruleIndex);
        }

        /// <summary>
        /// Return first ancestor node up the chain towards the root that has the rule index.
        ///  Search includes the current node.
        /// </summary>
        public static ParserRuleContext getAncestor(ParserRuleContext t, int ruleIndex)
        {
            while (t != null)
            {
                if (t.RuleIndex == ruleIndex)
                {
                    return t;
                }
                t = (ParserRuleContext)t.Parent;
            }
            return null;
        }

        /// <summary>
        /// Return first ancestor node up the chain towards the root that is clazz.
        ///  Search includes the current node.
        /// </summary>
        public static ParserRuleContext getFirstAncestorOfType(ParserRuleContext t, IType clazz)
        {
            while (t != null)
            {
#pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
                if (t.GetType() == clazz)
#pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
                {
                    return t;
                }
                t = (ParserRuleContext)t.Parent;
            }
            return null;
        }

        /// <summary>
        /// Order of scopes not guaranteed but is currently breadth-first according
        ///  to nesting depth. Gets ScopedSymbols only.
        /// </summary>
        public static void getAllNestedScopedSymbols(IScope scope, IList<IScope> scopes)
        {
            ((List<IScope>)scopes).AddRange(scope.NestedScopedSymbols);
            foreach (IScope s in scope.NestedScopedSymbols)
            {
                getAllNestedScopedSymbols(s, scopes);
            }
        }

        /// <summary>
        /// Order of scopes not guaranteed but is currently breadth-first according
        ///  to nesting depth. Gets ScopedSymbols and non-ScopedSymbols.
        /// </summary>
        public static void getAllNestedScopes(IScope scope, IList<IScope> scopes)
        {
            ((List<IScope>)scopes).AddRange(scope.NestedScopes);
            foreach (IScope s in scope.NestedScopes)
            {
                getAllNestedScopes(s, scopes);
            }
        }

        /// <summary>
        /// Return a string of scope names with the "stack" growing to the left
        ///  E.g., myblock:mymethod:myclass.
        ///  String includes arg scope in string.
        /// </summary>
        public static string toScopeStackString(IScope scope, string separator)
        {
            IList<IScope> scopes = scope.EnclosingPathToRoot;
            return joinScopeNames(scopes, separator);
        }

        /// <summary>
        /// Return a string of scope names with the "stack" growing to the right.
        ///  E.g., myclass:mymethod:myblock.
        ///  String includes arg scope in string.
        /// </summary>
        public static string toQualifierString(IScope scope, string separator)
        {
            IList<IScope> scopes = scope.EnclosingPathToRoot;
            scopes.Reverse();
            return joinScopeNames(scopes, separator);
        }

        public static string ToString(IScope s, int level)
        {
            if (s == null)
            {
                return "";
            }
            StringBuilder buf = new StringBuilder();
            buf.Append(tab(level));
            buf.Append(s.Name);
            buf.Append("\n");
            level++;
            foreach (ISymbol sym in s.Symbols)
            { // print out all symbols but not scopes
                if (!(sym is IScope))
                {
                    buf.Append(tab(level));
                    buf.Append(sym);
                    buf.Append("\n");
                }
            }
            foreach (IScope nested in s.NestedScopes)
            { // includes named scopes and local scopes
                buf.Append(ToString(nested, level));
            }
            return buf.ToString();
        }

        public static string ToString(IScope s)
        {
            return ToString(s, 0);
        }

        //  Generic filtering, mapping, joining that should be in the standard library but aren't

        public static T findFirst<T>(IList<T> data, System.Func<T, bool> pred)
        {
            if (data != null)
            {
                foreach (T x in data)
                {
                    if (pred(x))
                    {
                        return x;
                    }
                }
            }
            return default(T);
        }

        public static IList<T> filter<T>(IList<T> data, System.Func<T, bool> pred)
        {
            IList<T> output = new List<T>();
            if (data != null)
            {
                foreach (T x in data)
                {
                    if (pred(x))
                    {
                        output.Add(x);
                    }
                }
            }
            return output;
        }

        public static ISet<T> filter<T>(ICollection<T> data, System.Func<T, bool> pred)
        {
            ISet<T> output = new HashSet<T>();
            foreach (T x in data)
            {
                if (pred(x))
                {
                    output.Add(x);
                }
            }
            return output;
        }

        public static IList<R> map<T, R>(ICollection<T> data, System.Func<T, R> getter)
        {
            IList<R> output = new List<R>();
            if (data != null)
            {
                foreach (T x in data)
                {
                    output.Add(getter(x));
                }
            }
            return output;
        }

        public static IList<R> map<T, R>(T[] data, System.Func<T, R> getter)
        {
            IList<R> output = new List<R>();
            if (data != null)
            {
                foreach (T x in data)
                {
                    output.Add(getter(x));
                }
            }
            return output;
        }

        public static string join<T>(ICollection<T> data, string separator)
        {
            return join(data.GetEnumerator(), separator, "", "");
        }

        public static string join<T>(ICollection<T> data, string separator, string left, string right)
        {
            return join(data.GetEnumerator(), separator, left, right);
        }

        public static string join<T>(IEnumerator<T> iter, string separator, string left, string right)
        {
            StringBuilder buf = new StringBuilder();

            while (iter.MoveNext())
            {
                buf.Append(iter.Current);
                //JAVA TO C# CONVERTER TODO TASK: Java iterators are only converted within the context of 'while' and 'for' loops:
                if (iter.MoveNext())
                {
                    buf.Append(separator);
                }
            }

            return left + buf.ToString() + right;
        }

        public static string join<T>(T[] array, string separator)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < array.Length; ++i)
            {
                builder.Append(array[i]);
                if (i < array.Length - 1)
                {
                    builder.Append(separator);
                }
            }

            return builder.ToString();
        }

        public static string tab(int n)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 1; i <= n; i++)
            {
                buf.Append("    ");
            }
            return buf.ToString();
        }

        public static string joinScopeNames(IList<IScope> scopes, string separator)
        {
            if (scopes == null || scopes.Count == 0)
            {
                return "";
            }
            StringBuilder buf = new StringBuilder();
            buf.Append(scopes[0].Name);
            for (int i = 1; i < scopes.Count; i++)
            {
                IScope s = scopes[i];
                buf.Append(separator);
                buf.Append(s.Name);
            }
            return buf.ToString();
        }
    }

    public static class HelpMe
    {
    }
}