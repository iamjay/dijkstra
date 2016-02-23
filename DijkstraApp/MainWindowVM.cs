using Newtonsoft.Json;
using Pathompong.Lib;
using Pathompong.Lib.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DijkstraApp
{

    public class MainWindowVM : INotifyPropertyChanged
    {
        private string _inputGraph;
        private string _shortestPath;

        public string InputGraph
        {
            get { return _inputGraph; }
            set
            {
                if (string.Equals(_inputGraph, value))
                    return;

                _inputGraph = value;
                NotifyPropertyChanged("InputGraph");
            }
        }

        public string ShortestPath
        {
            get { return _shortestPath; }
            set
            {
                if (string.Equals(_shortestPath, value))
                    return;

                _shortestPath = value;
                NotifyPropertyChanged("ShortestPath");
            }
        }

        public ICommand GetPathCommand { get; set; }

        public event EventHandler<dynamic> UpdateGraph;

        public MainWindowVM()
        {
            InputGraph = new StringBuilder()
                .AppendLine(@"{")
                .AppendLine(@"  nodes: [""A"", ""B"", ""C"", ""D"", ""E"", ""F""],")
                .AppendLine(@"  edges: [")
                .AppendLine(@"     { src: ""A"", dest: ""B"", dist: 7 },")
                .AppendLine(@"     { src: ""A"", dest: ""C"", dist: 9 },")
                .AppendLine(@"     { src: ""A"", dest: ""F"", dist: 14 },")
                .AppendLine(@"     { src: ""B"", dest: ""C"", dist: 10 },")
                .AppendLine(@"     { src: ""B"", dest: ""D"", dist: 15 },")
                .AppendLine(@"     { src: ""C"", dest: ""D"", dist: 11 },")
                .AppendLine(@"     { src: ""C"", dest: ""F"", dist: 2 },")
                .AppendLine(@"     { src: ""D"", dest: ""E"", dist: 6 },")
                .AppendLine(@"     { src: ""E"", dest: ""F"", dist: 9 }")
                .AppendLine(@"  ],")
                .AppendLine(@"  src: ""A"",")
                .AppendLine(@"  dest: ""E""")
                .AppendLine(@"}")
                .ToString();
            ShortestPath = "";
            GetPathCommand = new RelayCommand(OnGetPath);
        }

        private void OnGetPath(object obj)
        {
            try
            {
                dynamic graph = JsonConvert.DeserializeObject(_inputGraph);

                var nodes = new Dictionary<string, Node>();
                foreach (var n in graph.nodes)
                {
                    var node = new Node(n.Value);
                    nodes[n.Value] = node;
                }

                foreach (var e in graph.edges)
                {
                    var n1 = nodes[e.src.Value];
                    var n2 = nodes[e.dest.Value];
                    var dist = e.dist.Value;
                    n1.AddNeighbor(n2, (int)dist);
                    n2.AddNeighbor(n1, (int)dist);
                }

                var src = nodes[graph.src.Value];
                var dest = nodes[graph.dest.Value];
                IEnumerable<INode> path;
                var distance = Dijkstra.GetShortestPath(nodes.Values.ToList(), src, dest, out path);

                ShortestPath = string.Format("{0}: {1}", distance, string.Join(" ", from n in path select n.Name));

                UpdateGraph(this, new { nodes = nodes, path = path });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error parsing input");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        #endregion
    }
}
