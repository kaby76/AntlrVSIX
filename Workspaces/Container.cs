namespace Workspaces
{
    using System.Collections.Generic;
    using System.Linq;

    public class Container
    {
        public virtual Container Parent
        {
            get;
            set;
        }
        public virtual Container AddChild(Container c) { return null; }
        public virtual Document FindDocument(string ffn) { return null; }
        public virtual Project FindProject(string ffn) { return null; }
        public virtual Project FindProject(string canonical_name, string name, string ffn) { return null; }
    }

    public class DFSContainer
    {
        public static IEnumerable<Document> DFS(Container root)
        {
            Stack<Container> toVisit = new Stack<Container>();
            Stack<Container> visitedAncestors = new Stack<Container>();
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
                            Project internal_node = node as Project;
                            IEnumerable<Container> children = internal_node.Children;
                            foreach (Container o in children.Reverse())
                            {
                                toVisit.Push(o);
                            }
                            continue;
                        }
                        else if (node is Workspace)
                        {
                            Workspace internal_node = node as Workspace;
                            IEnumerable<Container> children = internal_node.Children;
                            foreach (Container o in children.Reverse())
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

    internal static class StackHelper
    {
        public static Container PeekOrDefault(this Stack<Container> s)
        {
            return s.Count == 0 ? null : s.Peek();
        }
    }

}
