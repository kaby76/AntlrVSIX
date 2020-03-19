using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class BreadthFirstOrder<T, E> : IEnumerable<T>
        where E : IEdge<T>
    {
        private Dictionary<T, int> bfs;
        private Queue<T> bfsorder;
        private int bfsCounter;

        public BreadthFirstOrder(IGraph<T, E> graph, IEnumerable<T> subset_vertices)
        {
            bfs = new Dictionary<T, int>();
            foreach (var v in graph.Vertices) bfs[v] = 0;
            bfsorder = new Queue<T>();
            foreach (T v in subset_vertices)
                order(graph, v);
        }

        private void order(IGraph<T, E> G, T s)
        {
            Queue<T> Q = new Queue<T>();
            Dictionary<T, int> color = new Dictionary<T, int>();
            foreach (var v in G.Vertices) color[v] = 0;
            Dictionary<T, int> d = new Dictionary<T, int>();
            foreach (var v in G.Vertices) d[v] = 0;
            Dictionary<T, T> pi = new Dictionary<T, T>();
            foreach (var v in G.Vertices) pi[v] = default(T);

            bfsorder.Enqueue(s);
            foreach (T u in G.Vertices)
            {
                if (! u.Equals(s))
                {
                    color[u] = 0;
                    d[u] = Int32.MaxValue;
                    pi[u] = default(T);
                }
            }
            color[s] = 1;
            d[s] = 0;
            pi[s] = default(T);
            Q.Enqueue(s);
            while (Q.Count > 0)
            {
                var u = Q.Dequeue();
                foreach (var v in G.Successors(u))
                {
                    if (color[v] == 0)
                    {
                        color[v] = 1;
                        if (!bfsorder.Contains(v))
                        {
                            bfs[v] = ++bfsCounter;
                            bfsorder.Enqueue(v);
                        }
                        d[v] = d[u] + 1;
                        pi[v] = u;
                        Q.Enqueue(v);
                    }
                }
                color[u] = 2;
            }
        }

        public IEnumerable<T> BFS()
        {
            return bfsorder;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T v in this.BFS())
            {
                yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (T v in this.BFS())
            {
                yield return v;
            }
        }
    }
}
