using FluentAssertions;
using NUnit.Framework;
using Pathompong.Lib.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Pathompong.Lib.DijkstraTest
{
    [TestFixture]
    public class DijkstraTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        private class GetShortestPathTestFactory
        {
            private static List<INode> CreateTestGraph(IEnumerable<string> nodes, IEnumerable<dynamic> edges,
                string source, string destination, out INode sourceNode, out INode destNode)
            {
                var dict = new Dictionary<string, INode>();

                foreach (var node in nodes)
                    dict.Add(node, new Node(node));

                foreach (var edge in edges)
                {
                    var nodeA = dict[edge.Src];
                    var nodeB = dict[edge.Dest];
                    nodeA.AddNeighbor(nodeB, edge.Dist);
                    nodeB.AddNeighbor(nodeA, edge.Dist);
                }

                sourceNode = dict[source];
                destNode = dict[destination];
                return dict.Values.ToList();
            }

            public static IEnumerable GetShortestPathTestCases 
            {
                get
                {
                    INode source;
                    INode destination;
                    var graph = CreateTestGraph(new string[] { "A" },
                        new dynamic[] { }, "A", "A", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        0, new string[] { "A" }).SetName("Shortest path to self");

                    graph = CreateTestGraph(new string[] { "A", "B" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 2 },
                        }, "A", "B", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        2, new string[] { "A", "B" }).SetName("Path to immediate node");

                    graph = CreateTestGraph(new string[] { "A", "B", "C", "D" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 2 },
                            new { Src = "A", Dest = "C", Dist = 1 },
                            new { Src = "B", Dest = "D", Dist = 1 },
                            new { Src = "C", Dest = "D", Dist = 3 },
                        }, "A", "D", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        3, new string[] { "A", "B", "D" }).SetName("Complex path #1");

                    graph = CreateTestGraph(new string[] { "A", "B", "C", "D", "E" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 3 },
                            new { Src = "A", Dest = "C", Dist = 2 },
                            new { Src = "B", Dest = "E", Dist = 2 },
                            new { Src = "C", Dest = "D", Dist = 4 },
                            new { Src = "E", Dest = "D", Dist = 1 },
                        }, "A", "D", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        6, new string[] { "A", "C", "D" }).SetName("Multiple path with same distance");

                    graph = CreateTestGraph(new string[] { "A", "B", "C", "D", "E" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 3 },
                            new { Src = "A", Dest = "C", Dist = 2 },
                            new { Src = "B", Dest = "E", Dist = 2 },
                            new { Src = "C", Dest = "D", Dist = 5 },
                            new { Src = "E", Dest = "D", Dist = 1 },
                        }, "A", "D", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        6, new string[] { "A", "B", "E", "D" }).SetName("More hop with less distance");

                    graph = CreateTestGraph(new string[] { "A", "B", "C" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 1 },
                        }, "A", "C", out source, out destination);
                    yield return new TestCaseData(graph, source, destination,
                        -1, new string[] { }).SetName("Disconnected graph with no path");

                    graph = CreateTestGraph(new string[] { "A", "B", "C", "D" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 2 },
                            new { Src = "A", Dest = "C", Dist = 1 },
                            new { Src = "B", Dest = "D", Dist = 1 },
                            new { Src = "C", Dest = "D", Dist = 3 },
                        }, "A", "D", out source, out destination);
                    graph.RemoveAt(0);
                    yield return new TestCaseData(graph, source, destination,
                        -1, new string[] { }).SetName("No source node in graph");

                    graph = CreateTestGraph(new string[] { "A", "B", "C", "D" },
                        new dynamic[] {
                            new { Src = "A", Dest = "B", Dist = 2 },
                            new { Src = "A", Dest = "C", Dist = 1 },
                            new { Src = "B", Dest = "D", Dist = 1 },
                            new { Src = "C", Dest = "D", Dist = 3 },
                        }, "A", "D", out source, out destination);
                    graph.RemoveAt(3);
                    yield return new TestCaseData(graph, source, destination,
                        -1, new string[] { }).SetName("No destination node in graph");
                }
            }
        }

        [Test, TestCaseSource(typeof(GetShortestPathTestFactory), "GetShortestPathTestCases")]
        public void GetShortestPath(IEnumerable<INode> nodes, INode source, INode destination,
            int expectedDistance, IEnumerable<string> expectedPath)
        {
            // Setup

            // Act
            IEnumerable<INode> path;
            var distance = Dijkstra.GetShortestPath(nodes, source, destination, out path);

            // Assert
            distance.Should().Be(expectedDistance);
            (from node in path select node.Name).ToArray().ShouldBeEquivalentTo(expectedPath);
        }
    }
}
