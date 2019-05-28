using System.Collections.Generic;
using System.Collections.ObjectModel;

using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    /// <summary>
    /// Đồ thị con tương ứng với một tầng trong Building. 
    /// </summary>
    class SubGraph
    {
        private List<Node> nodes, stairNodes;

        /// <returns>Danh sách các đỉnh ảo trong đồ thị.</returns>
        public List<Node> Nodes { get { return nodes; } }

        /// <returns>Danh sách các Stair Node trong đồ thị.</returns>
        public ReadOnlyCollection<Node> StairNodes { get { return stairNodes.AsReadOnly(); } }

        /// <summary>
        /// Khởi tạo đồ thị con.
        /// </summary>
        /// <param name="floor"> Tầng tương ứng trong Building.</param>
        public SubGraph(Floor floor)
        {
            this.nodes = new List<Node>();
            this.stairNodes = new List<Node>();

            foreach (Indicator indicator in floor.Indicators)
            {
                Node node = new Node(indicator);
                nodes.Add(node);

                if (indicator.IsStairNode || indicator.IsExitNode)
                {
                    stairNodes.Add(node);
                }
            }

            foreach (Node u in nodes)
            {
                foreach (Corridor cor in u.CorresspodingIndicator.Neighbors)
                {
                    Node v = nodes.Find(node => node.CorresspodingIndicator == cor.To(u.CorresspodingIndicator));
                    if (v == null) continue;
                    u.Adjacences.Add(new Edge(u, v, cor.LCDTWeight(), cor));
                    v.Adjacences.Add(new Edge(v, u, cor.LCDTWeight(), cor));
                }
            }

        }

        /// <summary>
        /// Cập nhật lại các trọng số cạnh của SubGraph.
        /// </summary>
        public void Update()
        {
            foreach (Node node in nodes)
            {
                node.NextOptions = new List<NodeOption>();
                foreach (Edge e in node.Adjacences)
                {
                    e.Weight = e.CorrespondingCorridor.LCDTWeight();
                }
            }
        }

    }
}