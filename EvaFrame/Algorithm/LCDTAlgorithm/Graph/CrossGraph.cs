using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;


namespace EvaFrame.Algorithm.LCDTAlgorithm {
    public class CrossGraph {
        
        private Building target;
        /// <value> Thông số tòa nhà. </value>
        public Building Target { get { return target; } }
        
        private List<Edge> edges;
        /// <value> Danh sách các cạnh của đồ thị.</value>
        public List<Edge> Edges{ get { return edges; } }

        private List<Node> nodes;
        /// <value> Danh sách các Node của đồ thị.</value>
        public List<Node> Nodes { get { return nodes; } }
        /// <summary>
        /// Thêm một node chưa xuất hiện vào danh sách Node.
        /// </summary>
        /// <param name="node">
        /// Một Node bất kì.
        /// </param>
        private void addNode(Node node) {
            if (nodes.Contains(node) == false) nodes.Add(node);
        }

        public CrossGraph() {}
        /// <summary>
        /// Khởi tạo Cross Graph.
        /// </summary>
        /// <param name="building">
        /// Thông số tòa nhà.
        /// </param>
        public CrossGraph(Building building) {
            this.target = building;
            this.edges = new List<Edge>();
            this.nodes = new List<Node>();
        }

        /// <summary>
        /// Xây dựng đồ thị.
        /// </summary>
        public void buildGraph() {
            foreach (Floor floor in target.Floors) {
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

        /// <summary>
        /// Cập nhật lại đồ thị khi thêm 1 tầng mới.
        /// </summary>
        /// <param name="weightInLocal">
        /// Trọng số của tầng mới thêm vào.
        /// </param>
        public void updateGraph(Dictionary<PairNN, double> weightInLocal) {
 
            foreach (KeyValuePair<PairNN, double> item in weightInLocal) {
                edges.Add(new Edge(item.Key.First, item.Key.Second, item.Value));

                addNode(item.Key.First);
                addNode(item.Key.Second);
            }

        }
    }
}