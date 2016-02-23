using Pathompong.Lib.Interface;
using System.Collections.Generic;

namespace Pathompong.Lib
{
    public class Dijkstra
    {
        private class DistanceNodeComparer : IComparer<INode>
        {
            // Sort node by tentative distance.
            public int Compare(INode x, INode y)
            {
                var result = x.TentativeDistance - y.TentativeDistance;
                if (result == 0)
                    return x.Name.CompareTo(y.Name);
                return result;
            }
        }

        /*
        https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm#Using_a_priority_queue
        1  function Dijkstra(Graph, source):
        2      dist[source] ← 0                                    // Initialization
        3
        4      create vertex set Q
        5
        6      for each vertex v in Graph:           
        7          if v ≠ source
        8              dist[v] ← INFINITY                          // Unknown distance from source to v
        9              prev[v] ← UNDEFINED                         // Predecessor of v
        10
        11         Q.add_with_priority(v, dist[v])
        12
        13
        14     while Q is not empty:                              // The main loop
        15         u ← Q.extract_min()                            // Remove and return best vertex
        16         for each neighbor v of u:                       // only v that is still in Q
        17             alt = dist[u] + length(u, v) 
        18             if alt < dist[v]
        19                 dist[v] ← alt
        20                 prev[v] ← u
        21                 Q.decrease_priority(v, alt)
        22
        23     return dist[], prev[]
        */
        /// <summary>
        /// Find shortest path from source to destination
        /// </summary>
        /// <param name="nodes">List of all nodes</param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="path">Output path found</param>
        /// <returns>Shortest distance from source to destination. -1 on no path found.</returns>
        public static int GetShortestPath(IEnumerable<INode> nodes, INode source, INode destination, out IEnumerable<INode> path)
        {
            // Initializating distance-to-source table with infinity.
            var q = new SortedSet<INode>(new DistanceNodeComparer());
            foreach (var node in nodes)
            {
                node.TentativeDistance = node == source ? 0 : int.MaxValue;
                node.Previous = null;
                q.Add(node);
            }
            // qIndex is used for fast checking of unvisited node.
            var qIndex = new HashSet<INode>(nodes);

            while (q.Count > 0)
            {
                // Remove and return best vertex
                var u = q.Min;
                q.Remove(q.Min);
                qIndex.Remove(u);

                if (u.Equals(destination))
                {
                    // We've found the path.
                    var result = new List<INode>();
                    var node = destination;
                    while (node.Previous != null)
                    {
                        result.Add(node);
                        node = node.Previous;
                    }
                    if (node != source)
                    {
                        // No path from source to destination found.
                        path = new INode[] { };
                        return -1;
                    }
                    result.Add(node);

                    result.Reverse();
                    path = result;
                    return destination.TentativeDistance;
                }

                foreach (var v in u.Neighbors)
                {
                    // only v that is still in Q
                    if (!qIndex.Contains(v.Key))
                        continue;

                    var alt = u.TentativeDistance + v.Value;
                    if (alt < v.Key.TentativeDistance)
                    {
                        q.Remove(v.Key);
                        v.Key.TentativeDistance = alt;
                        v.Key.Previous = u;
                        q.Add(v.Key);
                    }
                }
            }

            path = new INode[] { };
            return -1;
        }
    }
}
