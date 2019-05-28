using System.Collections.Generic;

using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;


namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    /// <summary>
    /// Đồ thị giữa các exit Node và Stair Node.
    /// </summary>
    class CrossGraph
    {
        private Building target;
        /// <value> Thông số tòa nhà. </value>
        public Building Target { get { return target; } }

        private List<Node> nodes;
        /// <value> Danh sách các Node của đồ thị.</value>
        public List<Node> Nodes { get { return nodes; } }

        /// <summary>
        /// Khởi tạo CrossGraph dựa trên một đối tượng tòa nhà.
        /// </summary>
        /// <param name="target">
        /// Thông số tòa nhà.
        /// </param>
        public CrossGraph(Building target)
        {
            this.target = target;
            this.nodes = new List<Node>();
        }

        /// <summary>
        /// Xây dựng các cạnh nối giữa các Stair Node giữa các tầng.
        /// Hàm này cần được gọi sau khi đã cung cấp đủ các tầng thông qua hàm AddFloorFromLocal()
        /// </summary>
        public void ConnectFloors()
        {
            for (int i = 0; i < target.Floors.Count; i ++)
            {
                Floor floor = target.Floors[i];
                foreach (Corridor cor in floor.Stairways)
                {
                    Node n1 = nodes.Find(node => node.CorresspodingIndicator == cor.I1);
                    Node n2 = nodes.Find(node => node.CorresspodingIndicator == cor.I2);
                    if (n1.CorresspodingIndicator.FloorId > i && n2.CorresspodingIndicator.FloorId > i)
                        continue;
                    Edge edge1 = new Edge(n1, n2, cor.LCDTWeight(), cor);
                    Edge edge2 = new Edge(n2, n1, cor.LCDTWeight(), cor);
                    n1.Adjacences.Add(edge1);
                    n2.Adjacences.Add(edge2);
                }
            }
        }

        /// <summary>
        /// Thêm thông tin về một tầng mới vào CrossGraph, dựa trên tính toán của <c>LocalEvaluation</c>.
        /// </summary>
        /// <param name="localWeight">
        /// Trọng số của tầng mới thêm vào.
        /// </param>
        public void AddFloorFromLocal(Dictionary<PairNN, double> localWeight)
        {
            foreach (KeyValuePair<PairNN, double> item in localWeight)
            {
                Node from = nodes.Find(node => node.CorresspodingIndicator == item.Key.First.CorresspodingIndicator);
                if (from == null) from = new Node(item.Key.First.CorresspodingIndicator);
                Node to = nodes.Find(node => node.CorresspodingIndicator == item.Key.Second.CorresspodingIndicator);
                if (to == null) to = new Node(item.Key.Second.CorresspodingIndicator);
                Edge edge1 = new Edge(from, to, item.Value);
                Edge edge2 = new Edge(to, from, item.Value);

                addNode(from);
                addNode(to);
                from.Adjacences.Add(edge1);
                to.Adjacences.Add(edge2);
            }
        }

        /// <summary>
        /// Thêm một Node vào CrossGraph nếu như Node này chưa xuất hiện trong danh sách Node.
        /// </summary>
        /// <param name="node">
        /// Một Node bất kì.
        /// </param>
        private void addNode(Node node)
        {
            if (nodes.Contains(node) == false) nodes.Add(node);
        }
    }
}