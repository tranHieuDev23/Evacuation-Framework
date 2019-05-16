using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;


namespace EvaFrame.Algorithm.LCDTAlgorithm {
    public class CrossGraph {
        private Building building;
        private List<Edge> edges;
        public List<Edge> Edges{
            get { return edges; }
        }

        private List<Node> nodes;
        public List<Node> Nodes {
            get { return nodes; }
        }
        private void addNode(Node node) {
            if (nodes.Contains(node) == false) nodes.Add(node);
        }

        public CrossGraph() {}
        public CrossGraph(Building building) {
            this.building = building;
            this.edges = new List<Edge>();
            this.nodes = new List<Node>();
        }

        public void buildGraph() {
            /* foreach (Indicator exit in building.Exits) {
                Node exitNode = new Node(exit);

                foreach (Indicator stairIndicator in building.Floors[0].Stairs) {
                    Node stairNode = new Node(stairIndicator);
                    edges.Add(new Edge(exitNode, stairNode,));
                }
            }*/

            foreach (Floor floor in building.Floors) {
                foreach (Indicator stairIndicator in floor.Stairs) {
                    foreach (Corridor cor in stairIndicator.Neighbors)
                    if (cor.IsStairway) {
                        Node from = new Node(cor.From);
                        Node to = new Node(cor.To);
                        edges.Add(new Edge(from, to, cor.calcWeight(), cor));
                        
                        addNode(from);
                        addNode(to);
                    }
                }   
            }
        }

        public void updateGraph(Dictionary<PairNN, double> weightInLocal) {
 
            foreach (KeyValuePair<PairNN, double> item in weightInLocal) {
                edges.Add(new Edge(item.Key.First, item.Key.Second, item.Value));

                addNode(item.Key.First);
                addNode(item.Key.Second);
            }

        }
    }
}