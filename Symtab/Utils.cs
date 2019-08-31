namespace org.antlr.symtab
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Antlr4.Runtime;

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
        public static ParserRuleContext getFirstAncestorOfType(ParserRuleContext t, Type clazz)
        {
            while (t != null)
            {
                if (t.GetType() == clazz)
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
        public static void getAllNestedScopedSymbols(Scope scope, IList<Scope> scopes)
        {
            ((List<Scope>)scopes).AddRange(scope.NestedScopedSymbols);
            foreach (Scope s in scope.NestedScopedSymbols)
            {
                getAllNestedScopedSymbols(s, scopes);
            }
        }

        /// <summary>
        /// Order of scopes not guaranteed but is currently breadth-first according
        ///  to nesting depth. Gets ScopedSymbols and non-ScopedSymbols.
        /// </summary>
        public static void getAllNestedScopes(Scope scope, IList<Scope> scopes)
        {
            ((List<Scope>)scopes).AddRange(scope.NestedScopes);
            foreach (Scope s in scope.NestedScopes)
            {
                getAllNestedScopes(s, scopes);
            }
        }

        /// <summary>
        /// Return a string of scope names with the "stack" growing to the left
        ///  E.g., myblock:mymethod:myclass.
        ///  String includes arg scope in string.
        /// </summary>
        public static string toScopeStackString(Scope scope, string separator)
        {
            IList<Scope> scopes = scope.EnclosingPathToRoot;
            return joinScopeNames(scopes, separator);
        }

        /// <summary>
        /// Return a string of scope names with the "stack" growing to the right.
        ///  E.g., myclass:mymethod:myblock.
        ///  String includes arg scope in string.
        /// </summary>
        public static string toQualifierString(Scope scope, string separator)
        {
            IList<Scope> scopes = scope.EnclosingPathToRoot;
            scopes.Reverse();
            return joinScopeNames(scopes, separator);
        }

        public static string ToString(Scope s, int level)
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
            foreach (Symbol sym in s.Symbols)
            { // print out all symbols but not scopes
                if (!(sym is Scope))
                {
                    buf.Append(tab(level));
                    buf.Append(sym);
                    buf.Append("\n");
                }
            }
            foreach (Scope nested in s.NestedScopes)
            { // includes named scopes and local scopes
                buf.Append(ToString(nested, level));
            }
            return buf.ToString();
        }

        public static string ToString(Scope s)
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

        public static string joinScopeNames(IList<Scope> scopes, string separator)
        {
            if (scopes == null || scopes.Count == 0)
            {
                return "";
            }
            StringBuilder buf = new StringBuilder();
            buf.Append(scopes[0].Name);
            for (int i = 1; i < scopes.Count; i++)
            {
                Scope s = scopes[i];
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