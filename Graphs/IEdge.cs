using System;

namespace Graphs
{
    public interface IEdge<NODE> : IComparable<IEdge<NODE>>
    {
        NODE From
        {
            get;
        }

        NODE To
        {
            get;
        }
    }
}
