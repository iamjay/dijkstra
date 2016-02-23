using Pathompong.Lib.Interface;
using System.Collections.Generic;

namespace Pathompong.Lib
{
    public class Node : INode
    {
        private string _name;
        private Dictionary<INode, int> _neighbors;
        private INode _previous;
        private int _tentativeDistance;

        public Node(string name)
        {
            _name = name;
            _neighbors = new Dictionary<INode, int>();
        }

        public string Name { get { return _name; } }

        public IDictionary<INode, int> Neighbors { get { return _neighbors; } }

        public INode Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }

        public int TentativeDistance
        {
            get { return _tentativeDistance; }
            set { _tentativeDistance = value; }
        }

        public void AddNeighbor(INode node, int distance)
        {
            _neighbors.Add(node, distance);
        }

        public void AddNeighbors(IEnumerable<KeyValuePair<INode, int>> neighbors)
        {
            foreach (var neighbor in neighbors)
                AddNeighbor(neighbor.Key, neighbor.Value);
        }

        public void RemoveNeighbor(INode node)
        {
            _neighbors.Remove(node);
        }
    }
}
