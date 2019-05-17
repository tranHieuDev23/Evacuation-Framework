using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo.VirtualGraph
{
    class Graph
    {
        private List<SubGraph> floorGraphs;

        /// <summary>
        /// Danh sách các <c>Subgraph</c> tương ứng với các <c>Floor</c>
        /// trong <c>Building</c>
        /// </summary>
        /// <value>Read Only Value</value>
        public ReadOnlyCollection<SubGraph> FloorGraphs
        {
            get
            {
                return floorGraphs.AsReadOnly();
            }
        }

        private List<Node> allStairs;

        /// <summary>
        /// Danh sách chứa tất cả các đỉnh stair trong tòa nhà.
        /// </summary>
        /// <value></value>
        public ReadOnlyCollection<Node> AllStairs 
        {
            get
            {
                return allStairs.AsReadOnly();
            }
        }
        private Node root;

        /// <summary>
        /// Đỉnh nguồn, đại diện cho phía bên ngoài tòa nhà.
        /// </summary>
        /// <value>Giá trị Read Only</value>
        public Node Root 
        {
            get
            {
                return root;
            }
        }

        /// <summary>
        /// Khởi tạo đồ thị tương ứng với <c>building</c>
        /// </summary>
        /// <param name="building">Tòa nhà nguồn khởi tạo cho đồ thị</param>
        public Graph(Building building)
        {
            floorGraphs = new List<SubGraph>();
            allStairs = new List<Node>();
            root = new Node(new Indicator());
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
                    if(corridor.To.IsStairNode)
                    {
                        Node nextStair = allStairs.Find(nod => nod.CorrespondingIndicator 
                                                            == corridor.To);
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
                        Corridor cor1 = new Corridor(node.CorrespondingIndicator, null,
                                                    0, 1, 0, 1);
                        adjacence1.edge = new Edge(cor1, root);
                        adjacence1.node = root;
                        node.adjacences.Add(adjacence1);

                        Adjacence adjacence2 = new Adjacence();
                        Corridor cor2 = new Corridor(node.CorrespondingIndicator, null,
                                                    0, 1, 0, 1);
                        adjacence2.edge = new Edge(cor2, node);
                        adjacence2.node = node;
                        root.adjacences.Add(adjacence2);
                    }       
                }
            }
        }

        /// <summary>
        /// Phương thức <c> FetchInforFromBuilding() </c> nhận thông tin cập 
        /// nhật các thông số của tòa nhà tại thời điểm gọi.
        /// </summary>
        public void FetchInforFromBuilding(){
            foreach (var subGraph in floorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    foreach (var adj in node.adjacences)
                    {
                        adj.edge.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Phương thức <c> TackleInformation() </c> xử lí thông tin nhận được
        /// từ building thành các trọng số phù hợp với yêu cầu của thuật toán.
        /// </summary>
        public void TackleInformation(){

        }

        /// <summary>
        /// Trả lại thông tin về hướng chỉ của các <c> Indicator </c> cho
        /// <c> building </c>
        /// </summary>
        public void UpdateResultToBuilding(){
            int count = 0;
            foreach (var subGraph in floorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    if(node.nextEdge == null)
                    {
                        ++count;
                        continue;
                    }
                    node.CorrespondingIndicator.Next = node.nextEdge.CorrespondingCorridor;
                }
            }
            Console.WriteLine("{0} {1}", "numberOfExitNode = ", count);
        }
    }
}