using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo.VirtualGraph
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

        public Floor CorrespondingFloor
        {
            get
            {
                return correspondingFloor;
            }
        }

        public SubGraph(Floor floor)
        {
            int numberIndicator = floor.Indicators.Count;
            int numberStair = floor.Stairs.Count;
            correspondingFloor = floor;

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
                    Adjacence adjacence = new Adjacence();
                    Node nod = nodes.Find(x => x.CorrespondingIndicator == corridor.To);
                    Edge edg = new Edge(corridor, nod);
                    adjacence.node = nod;
                    adjacence.edge = edg;
                    node.adjacences.Add(adjacence);
                }
            }
        }
    }
}