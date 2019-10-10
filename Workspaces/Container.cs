namespace Workspaces
{
    using System.Linq;
    using System;
    using System.Collections.Generic;

    public class Container
    {
        public virtual Document FindDocument(string ffn) { return null; }
        public virtual Project FindProject(string ffn) { return null; }
        public virtual Container Parent
        {
            get;
            set;
        }
        public virtual Container AddChild(Container c) { return null; }
    }

    public class DFSContainer
    {
        public static IEnumerable<Document> DFS(Container root)
        {
            var toVisit = new Stack<Container>();
            var visitedAncestors = new Stack<Container>();
            toVisit.Push(root);
            while (toVisit.Count > 0)
            {
                Container node = toVisit.Peek();
                if (!(node is Document))
                {
                    if (visitedAncestors.PeekOrDefault() != node)
                    {
                        visitedAncestors.Push(node);
                        if (node is Document)
                        {
                        }
                        else if (node is Project)
                        {
                            var internal_node = node as Project;
                            var children = internal_node.Children;
                            foreach (var o in children.Reverse())
                            {
                                toVisit.Push(o);
                            }
                            continue;
                        }
                        else if (node is Workspace)
                        {
                            var internal_node = node as Workspace;
                            var children = internal_node.Children;
                            foreach (var o in children.Reverse())
                            {
                                toVisit.Push(o);
                            }
                            continue;
                        }
                    }
                    visitedAncestors.Pop();
                }
                else
                {
                    yield return node as Document;
                }
                toVisit.Pop();
            }
        }
    }

    static class StackHelper
    {
        public static Container PeekOrDefault(this Stack<Container> s)
        {
            return s.Count == 0 ? null : s.Peek();
        }
    }

}
