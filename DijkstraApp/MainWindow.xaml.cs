using Pathompong.Lib;
using Pathompong.Lib.Interface;
using Smrf.NodeXL.Core;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DijkstraApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = DataContext as MainWindowVM;
            vm.UpdateGraph += Vm_UpdateGraph;
        }

        private void Vm_UpdateGraph(object sender, dynamic e)
        {
            nodeXL.ClearGraph();

            var vertices = nodeXL.Graph.Vertices;
            var edges = nodeXL.Graph.Edges;

            // Add nodes
            Dictionary<string, Node> nodes = e.nodes;
            foreach (Node node in nodes.Values)
            {
                var vertex = vertices.Add();
                vertex.Name = node.Name;
                vertex.SetValue(ReservedMetadataKeys.PerVertexLabel, node.Name);
                vertex.SetValue(ReservedMetadataKeys.PerColor, Colors.Black);
            }

            // Add path
            IVertex prevNode = null;
            foreach (INode node in e.path)
            {
                IVertex nextNode;
                vertices.Find(node.Name, out nextNode);
                if (prevNode != null)
                {
                    var edge = edges.Add(prevNode, nextNode);
                    edge.SetValue(ReservedMetadataKeys.PerColor, Colors.Red);
                }
                prevNode = nextNode;
            }

            // Add edges
            foreach (Node node in nodes.Values)
            {
                IVertex v1;
                vertices.Find(node.Name, out v1);

                foreach (var neighbor in node.Neighbors)
                {
                    IVertex v2;
                    vertices.Find(neighbor.Key.Name, out v2);

                    var edge = edges.Add(v1, v2);
                    edge.SetValue(ReservedMetadataKeys.PerEdgeLabelFontSize, 14.0F);
                    edge.SetValue(ReservedMetadataKeys.PerEdgeLabel, neighbor.Value.ToString());
                }
            }

            nodeXL.DrawGraph(true);
        }
    }
}
