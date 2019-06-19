using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.PFAlgorithm.VirtualGraph
{
    class SubGraph
    {
        private List<Node> nodes;

        /// <summary>
        /// Danh sách các đỉnh trong tầng, tương ứng với các <c>Indicator</c> và <c>Stair</c>
        /// </summary>
        /// <value>Read Only Value</value>
        public ReadOnlyCollection<Node> Nodes
        {
            get
            {
                return nodes.AsReadOnly();
            }
        }

        private List<Node> stairs;

        /// <summary>
        /// Danh sách các <c>node</c> tương ứng với các <c>stair</c> của <c>floor</c>
        /// </summary>
        /// <value>Chỉ dùng cho mục đích liên kết giữa các tầng khác nhau</value>
        public ReadOnlyCollection<Node> Stairs
        {
            get
            {
                return stairs.AsReadOnly();
            }
        }

        private Floor correspondingFloor;

        /// <summary>
        /// <c>floor</c> trong <c>build</c> được mô phỏng
        /// </summary>
        /// <value>Giá trị read-only</value>
        public Floor CorrespondingFloor
        {
            get
            {
                return correspondingFloor;
            }
        }

        /// <summary>
        /// Khởi tạo các <c>node</c>, <c>edge</c> tương ứng với các <c>indicator</c>, 
        /// <c>corridor</c> trong <c>floor</c> 
        /// </summary>
        /// <param name="floor">Nguồn khởi tạo <c>SubGraph</c></param>
        public SubGraph(Floor floor)
        {
            int numberIndicator = floor.Indicators.Count;
            int numberStair = floor.Stairs.Count;
            correspondingFloor = floor;
            nodes = new List<Node>();
            stairs = new List<Node>();

            /*Khởi tạo các đỉnh trong SubGraph tương ứng với Floor */
            for (int i = 0; i < numberIndicator; i++)
            {
                Node node = new Node(floor.Indicators[i]);
                nodes.Add(node);
                if (floor.Indicators[i].IsStairNode)
                {
                    stairs.Add(node);
                }
            }
            LinkNode();
        }

        /// <summary>
        /// Tạo cạnh tương ứng giữa các <c>node</c> trong <c>subgraph</c>
        /// </summary>
        private void LinkNode()
        {
            foreach (var node in nodes)
            {
                foreach (var corridor in node.CorrespondingIndicator.Neighbors)
                {
                    if (corridor.IsStairway == true)
                        continue;
                    Indicator indicatorTo = corridor.To(node.CorrespondingIndicator);
                    Adjacence adjacence = new Adjacence();
                    Node nod = nodes.Find(x => x.CorrespondingIndicator == indicatorTo);
                    Edge edg = new Edge(corridor, nod);
                    adjacence.node = nod;
                    adjacence.edge = edg;
                    node.adjacences.Add(adjacence);
                }
            }
        }
    }
}