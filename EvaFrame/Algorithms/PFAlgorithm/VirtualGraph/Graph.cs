using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.PFAlgorithm.VirtualGraph
{
    class Graph
    {
        private List<SubGraph> floorGraphs;
        /// <value>
        /// Danh sách các <c>Subgraph</c> tương ứng với các <c>Floor</c> trong <c>Building</c>. Giá trị read-only.
        /// </value>
        public ReadOnlyCollection<SubGraph> FloorGraphs
        {
            get
            {
                return floorGraphs.AsReadOnly();
            }
        }

        private List<Node> allStairs;
        /// <value>
        /// Danh sách chứa tất cả các Stair Node trong tòa nhà. Giá trị read-only.
        /// </value>
        public ReadOnlyCollection<Node> AllStairs
        {
            get
            {
                return allStairs.AsReadOnly();
            }
        }

        private Node root;
        /// <value>
        /// Đỉnh nguồn, đại diện cho phía bên ngoài tòa nhà. Giá trị read-only.
        /// </value>
        public Node Root
        {
            get
            {
                return root;
            }
        }

        /// <summary>
        /// Khởi tạo đồ thị tương ứng với <c>Building</c>.
        /// </summary>
        /// <param name="building">Tòa nhà nguồn khởi tạo cho đồ thị.</param>
        public Graph(Building building)
        {
            floorGraphs = new List<SubGraph>();
            allStairs = new List<Node>();
            root = new Node(null);
            /*Khởi tạo các subgraph tương ứng với các tầng */
            foreach (var floor in building.Floors)
            {
                SubGraph subGraph = new SubGraph(floor);
                floorGraphs.Add(subGraph);
                foreach (var stair in subGraph.Stairs)
                {
                    allStairs.Add(stair);
                }
            }
            /*Liên kết giữa các subgraph thông qua các stairNode */
            foreach (var stair in allStairs)
            {
                foreach (var corridor in stair.CorrespondingIndicator.Neighbors)
                {
                    if (corridor.IsStairway)
                    {
                        Indicator indicatorTo = corridor.To(stair.CorrespondingIndicator);
                        Node nextStair = allStairs.Find(nod => nod.CorrespondingIndicator == indicatorTo);
                        Adjacence adjacence = new Adjacence();
                        adjacence.edge = new Edge(corridor, nextStair);
                        adjacence.node = nextStair;

                        stair.adjacences.Add(adjacence);
                    }
                }
            }
            /*Khởi tạo các node tương ứng với exit trong building và kết nối vào graph */
            foreach (var subGraph in floorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    if (node.CorrespondingIndicator.IsExitNode)
                    {
                        Adjacence adjacence1 = new Adjacence();
                        Corridor cor1 = null;
                        adjacence1.edge = new Edge(cor1, root);
                        adjacence1.node = root;
                        node.adjacences.Add(adjacence1);

                        Adjacence adjacence2 = new Adjacence();
                        Corridor cor2 = null;
                        adjacence2.edge = new Edge(cor2, node);
                        adjacence2.node = node;
                        root.adjacences.Add(adjacence2);
                    }
                }
            }
        }

        /// <summary>
        /// Trả lại thông tin về hướng chỉ của các <c>Indicator</c> cho <c>Building</c>.
        /// </summary>
        public void UpdateResultToBuilding()
        {
            int count = 0;
            foreach (var subGraph in floorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    if (node.next == root)
                    {
                        ++count;
                        continue;
                    }
                    node.CorrespondingIndicator.Next = node.nextEdge.CorrespondingCorridor;
                }
            }
        }
    }
}