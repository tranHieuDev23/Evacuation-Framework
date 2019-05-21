using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    public class Graph {
        private List<SubGraph> subGraphs;
        public ReadOnlyCollection<SubGraph> SubGraphs { get { return subGraphs.AsReadOnly(); } }

        private CrossGraph crossGraph;
        public CrossGraph CrossGraph { get { return crossGraph;} }

        private List<Node> exitNodes;
        public ReadOnlyCollection<Node> ExitNodes { get { return exitNodes.AsReadOnly(); } }

        public Graph() {
            this.subGraphs = new List<SubGraph>();
            this.crossGraph = null;
            this.exitNodes = new List<Node>();
        }

        public void addCrossGraph(CrossGraph crossGraph) {
            this.crossGraph = crossGraph;
            /*foreach (Node node in crossGraph.Nodes)
            if (node.IsExitNode) {
                this.exitNodes.Add(node);
            }*/
        }

        public void addSubGraph(SubGraph subGraph) {
            subGraphs.Add(subGraph);
            foreach (Node node in subGraph.Nodes)
            if (node.IsExitNode == true) {
                exitNodes.Add(node);
            }
        }
    }
}