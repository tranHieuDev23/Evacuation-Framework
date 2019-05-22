using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    /// <summary>
    /// Đồ thị ảo tổng quan tương ứng với tòa nhà.
    /// </summary>
    public class Graph {
        private List<SubGraph> subGraphs;
        /// <returns> Danh sách các đồ thị con tương ứng với từng tầng. </returns>
        public ReadOnlyCollection<SubGraph> SubGraphs { get { return subGraphs.AsReadOnly(); } }

        private CrossGraph crossGraph;
        /// <value> Đồ thị giữa các Exit Node và Stair Node. </value>
        public CrossGraph CrossGraph { get { return crossGraph;} }

        private List<Node> exitNodes;
        /// <returns> Danh sách các Exit Node tương ứng trong Building. </returns>
        public ReadOnlyCollection<Node> ExitNodes { get { return exitNodes.AsReadOnly(); } }

        /// <summary>
        /// Khởi tạo đồ thị ảo tổng.
        /// </summary>
        public Graph() {
            this.subGraphs = new List<SubGraph>();
            this.crossGraph = null;
            this.exitNodes = new List<Node>();
        }

        /// <summary>
        /// Thêm đồ thị Cross Graph vào đồ thị ảo tổng.
        /// </summary>
        /// <param name="crossGraph"> Đồ thị giữa các Exit node và Stair Node. </param>
        public void addCrossGraph(CrossGraph crossGraph) {
            this.crossGraph = crossGraph;
        }

        /// <summary>
        /// Thêm một đồ thị con vào đồ thị tổng.
        /// </summary>
        /// <param name="subGraph"> Đồ thị ảo con. </param>
        public void addSubGraph(SubGraph subGraph) {
            subGraphs.Add(subGraph);
            foreach (Node node in subGraph.Nodes)
            if (node.IsExitNode == true) {
                exitNodes.Add(node);
            }
        }
    }
}