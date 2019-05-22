using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;


namespace EvaFrame.Algorithm.LCDTAlgorithm {
    /// <summary>
    /// Đồ thị giữa các exit Node và Stair Node.
    /// </summary>
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
        /// <summary>
        /// Khởi tạo đồ thị.
        /// </summary>
        public CrossGraph() {
            this.target = null;
            this.edges = null;
            this.nodes = null;
        }
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
                foreach (Indicator indicator in floor.Indicators) {
                    foreach (Corridor cor in indicator.Neighbors)
                    if (cor.From.IsStairNode == true && cor.To.IsStairNode == true) {
                        Node from = nodes.Find( node => node.CorresspodingIndicator == cor.From);
                        Node to = nodes.Find( node => node.CorresspodingIndicator == cor.To);
                        Edge edge = new Edge(from, to, cor.calcWeight(), cor);
                        
                        int fromFloor = from.CorresspodingIndicator.getFloorNumber();
                        int toFloor = to.CorresspodingIndicator.getFloorNumber();
                        //System.Console.WriteLine("From floor = {0}, To Floor = {1}",fromFloor, toFloor);
                        if (fromFloor > toFloor) {
                            from.Next = edge;
                        }
                        edges.Add(edge);
                        from.Adjencents.Add(edge);
                        to.Adjencents.Add(edge);
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
                Node from = nodes.Find( node => node.CorresspodingIndicator == item.Key.First.CorresspodingIndicator );
                if (from == null) from = new Node( item.Key.First.CorresspodingIndicator );
                Node to = nodes.Find( node => node.CorresspodingIndicator == item.Key.Second.CorresspodingIndicator );
                if (to == null) to = new Node( item.Key.Second.CorresspodingIndicator );
                Edge edge = new Edge(from, to, item.Value);

                edges.Add(edge);
                addNode(from);
                addNode(to);
                from.Adjencents.Add(edge);
                to.Adjencents.Add(edge);
            }

        }
    }
}