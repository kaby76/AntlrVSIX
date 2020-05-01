using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
    // Rough translation of the DFS algorithm in Cormen and
    // pseudo coded in section "Depth-first search"
    // https://en.wikipedia.org/wiki/Topological_sorting#Depth-first_search
    // Here, I premark all back edges first given the work set,
    // then perform the sort. The number of nodes in the order must be all nodes
    // in the graph.

    enum Mark
    {
        None,
        Temporary,
        Permenent
    }

    public class TopologicalSort<T, E> : IEnumerable<T>
         where E : IEdge<T>
    {
        private readonly Dictionary<E, EdgeClassifier.Classification> classify = new Dictionary<E, EdgeClassifier.Classification>();
        private readonly IEnumerable<T> _work;
        private readonly IGraph<T, E> _graph;
        private Dictionary<T, Mark> _marked; // marked[v] = has v been marked in dfs?
        private List<T> l;

        public TopologicalSort(IGraph<T, E> graph, IEnumerable<T> subset_vertices)
        {
            _graph = graph;
            _work = subset_vertices;
            foreach (T v in _work)
            {
                EdgeClassifier.Classify(graph, v, ref classify);
            }
        }

        public List<T> topological_sort()
        {
            l = new List<T>();
            _marked = new Dictionary<T, Mark>();
            foreach (T v in _graph.Vertices)
            {
                _marked[v] = Mark.None;
            }
            foreach (T v in _work)
            {
                if (_marked[v] == Mark.None)
                {
                    dfs(v);
                }
            }
            foreach (T v in _graph.Vertices)
            {
                if (_marked[v] == Mark.None)
                {
                    dfs(v);
                }
            }
            if (l.Count != _graph.Vertices.Count())
                throw new Exception("Topological sort failed, not all vertices in the sort.");
            return l;
        }

        private void dfs(T v)
        {
            if (_marked[v] == Mark.Permenent)
                return;
            if (_marked[v] == Mark.Temporary)
                return;

            _marked[v] = Mark.Temporary;
            foreach (T w in _graph.Successors(v))
            {
                dfs(w);
            }

            _marked[v] = Mark.Permenent;

            l.Add(v);
        }


        public IEnumerable<T> GetEnumerable()
        {
            var x = topological_sort();
            foreach (T v in x.Reverse<T>())
            {
                yield return v;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var x = topological_sort();
            foreach (T v in x.Reverse<T>())
            {
                yield return v;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var x = topological_sort();
            foreach (T v in x.Reverse<T>())
            {
                yield return v;
            }
        }
    }
}
