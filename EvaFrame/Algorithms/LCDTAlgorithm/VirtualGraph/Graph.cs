using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithms.LCDTAlgorithm
{
    /// <summary>
    /// Đồ thị ảo tương ứng với tòa nhà trong thuật toán LCDT-GV.
    /// </summary>
    class Graph
    {
        private List<SubGraph> subGraphs;
        /// <value>Danh sách các đồ thị con tương ứng với từng tầng.</value>
        public ReadOnlyCollection<SubGraph> SubGraphs { get { return subGraphs.AsReadOnly(); } }

        private CrossGraph crossGraph;
        /// <value> Đồ thị giữa các Exit Node và Stair Node.</value>
        public CrossGraph CrossGraph
        {
            get { return this.crossGraph; }
            set { this.crossGraph = value; }
        }

        private List<Node> exitNodes;
        /// <returns> Danh sách các đỉnh tương ứng với Exit Node trong tòa nhà.</returns>
        public ReadOnlyCollection<Node> ExitNodes { get { return exitNodes.AsReadOnly(); } }

        /// <summary>
        /// Khởi tạo đồ thị ảo dựa trên một đối tượng <c>Building</c>.
        /// </summary>
        /// <param name="target">Đối tượng <c>Building</c> mà độ thị dựa trên.</param>
        public Graph(Building target)
        {
            this.subGraphs = new List<SubGraph>();
            this.crossGraph = null;
            this.exitNodes = new List<Node>();
            for (int i = 0; i < target.Floors.Count; ++i)
            {
                SubGraph sg = new SubGraph(target.Floors[i]);
                this.subGraphs.Add(sg);
                foreach (Node n in sg.Nodes)
                    if (n.IsExitNode)
                        this.exitNodes.Add(n);
            }
        }
    }
}