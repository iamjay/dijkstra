using FluentAssertions;
using NUnit.Framework;
using Pathompong.Lib.Interface;
using System.Collections.Generic;

namespace Pathompong.Lib.DijkstraTest
{
    [TestFixture]
    public class NodeTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void AddNeighbor()
        {
            // Setup
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");

            // Act
            nodeA.AddNeighbor(nodeB, 3);

            // Assert
            nodeA.Neighbors.Count.Should().Be(1);
            nodeA.Neighbors.Should().Contain(nodeB, 3);
            nodeB.Neighbors.Count.Should().Be(0);
        }

        [Test]
        public void AddNeighbors()
        {
            // Setup
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");

            // Act
            nodeA.AddNeighbors(new KeyValuePair<INode, int>[] {
                new KeyValuePair<INode, int>(nodeB, 2),
                new KeyValuePair<INode, int>(nodeC, 3),
            });

            // Assert
            nodeA.Neighbors.Count.Should().Be(2);
            nodeA.Neighbors.Should().Contain(nodeB, 2);
            nodeA.Neighbors.Should().Contain(nodeC, 3);
            nodeB.Neighbors.Count.Should().Be(0);
            nodeC.Neighbors.Count.Should().Be(0);
        }

        [Test]
        public void RemoveNeighbor()
        {
            // Setup
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");

            // Act
            nodeA.AddNeighbors(new KeyValuePair<INode, int>[] {
                new KeyValuePair<INode, int>(nodeB, 2),
                new KeyValuePair<INode, int>(nodeC, 3),
            });
            nodeA.RemoveNeighbor(nodeC);

            // Assert
            nodeA.Neighbors.Count.Should().Be(1);
            nodeA.Neighbors.Should().Contain(nodeB, 2);
            nodeB.Neighbors.Count.Should().Be(0);
            nodeC.Neighbors.Count.Should().Be(0);
        }

        [Test]
        public void RemoveNeighbor_NotExists()
        {
            // Setup
            Node nodeA = new Node("A");
            Node nodeB = new Node("B");
            Node nodeC = new Node("C");

            // Act
            nodeA.AddNeighbors(new KeyValuePair<INode, int>[] {
                new KeyValuePair<INode, int>(nodeB, 2),
            });
            nodeA.RemoveNeighbor(nodeC);

            // Assert
            nodeA.Neighbors.Count.Should().Be(1);
            nodeA.Neighbors.Should().Contain(nodeB, 2);
            nodeB.Neighbors.Count.Should().Be(0);
            nodeC.Neighbors.Count.Should().Be(0);
        }
    }
}
