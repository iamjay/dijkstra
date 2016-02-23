using System.Collections.Generic;

namespace Pathompong.Lib.Interface
{
    public interface INode
    {
        string Name { get; }

        IDictionary<INode, int> Neighbors { get; }

        void AddNeighbor(INode node, int distance);

        void AddNeighbors(IEnumerable<KeyValuePair<INode, int>> neighbors);

        void RemoveNeighbor(INode node);

        // Dijkstra's state
        INode Previous { get; set; }

        int TentativeDistance { get; set; }
    }
}
