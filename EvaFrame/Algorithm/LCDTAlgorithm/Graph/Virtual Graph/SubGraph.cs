using System.Collections.Generic;
using System.Collections.ObjectModel;

using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    /// <summary>
    /// Đồ thị con tương ứng với một tầng trong Building. 
    /// </summary>
    public class SubGraph {
        private List<Node> nodes, stairNodes;
        /// <returns> Danh sách các đỉnh ảo trong đồ thị. </returns>
        public ReadOnlyCollection<Node> Nodes{ get { return nodes.AsReadOnly(); } }
        /// <returns> Danh sách các Stair Node trong đồ thị. </returns>
        public ReadOnlyCollection<Node> StairNodes{ get { return stairNodes.AsReadOnly(); } }
        private bool isFirstFloor;
        /// <value> Kiểm tra xem đây có phải đồ thị con tương ứng với tầng 1 tòa nhà hay không. </value>
        public bool IsFirstFloor { get {return isFirstFloor; } }

        /// <summary>
        /// Khởi tạo đồ thị con.
        /// </summary>
        /// <param name="floor"> Tầng tương ứng trong Building. </param>
        /// <param name="building"> Nếu là tầng 1 thì cần truyền vào để lấy danh sách Exit Node. </param>
        public SubGraph(Floor floor, Building building = null) {
            this.nodes = new List<Node>();
            this.stairNodes = new List<Node>();

            if (building != null) {
                foreach (Indicator exitNode in building.Exits) {
                    Node node = new Node(exitNode);
                    nodes.Add(node);
                    node.IsExitNode = true;
                    stairNodes.Add(node);
                }

                isFirstFloor = true;
            }
            else isFirstFloor = false;

            foreach(Indicator indicator in floor.Indicators) {
                Node node = new Node(indicator);
                nodes.Add(node);
                
                if (indicator.IsStairNode == true) {
                    node.IsStairNode = true;
                    stairNodes.Add(node);
                }
            } 

            foreach(Node u in nodes) {
                foreach(Corridor cor in u.CorresspodingIndicator.Neighbors) {
                    Node v = nodes.Find(node => node.CorresspodingIndicator == cor.To(u.CorresspodingIndicator));
                    if (v == null) continue;
                    u.Adjencents.Add(new Edge(u, v, cor.calcWeight(), cor));
                }
            }

        }

    }
}