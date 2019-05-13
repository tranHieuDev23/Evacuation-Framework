using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo
{
    /// <summary>
    /// Class thu gọn của Indicator với mục đích chỉ dành cho
    /// thực hiện thuật toán
    /// </summary>
    public class Node
    {
        /// <summary>
        /// cấu trúc lưu thông tin các đỉnh và cạnh gần kề 
        /// với đỉnh đang xét
        /// </summary>
        public struct Adjacences
        {
            /// <summary>
            /// Đỉnh gần kề
            /// </summary>
            Node node;
            /// <summary>
            /// Đỉnh đi tới được sau t(s) bằng 
            /// cách đi qua node gần kề
            /// </summary>
            Node reaching;
            /// <summary>
            /// Cạnh gần kề
            /// </summary>
            Edge edge;
            /// <summary>
            /// Trọng số con đường qua đỉnh gần 
            /// kề đi đến root 
            /// </summary>
            double passingWeight;
        }

        /// <summary>
        /// Danh sách các cạnh kề 
        /// </summary>
        /// <value></value>
        public List<Adjacences> adjacences;

        /// <summary>
        /// Danh sách đỉnh tới được đây trong tương lai
        /// </summary>
        public List<Node> ComingNodes;

        /// <summary>
        /// Đỉnh sẽ tới được trong tương lai
        /// </summary>
        public Node ReachedNode;

        /// <summary>
        /// Cạnh tiếp theo trong đường đi ngắn nhất tới <c>root</c>
        /// </summary>
        public Edge nextEdge;

        /// <summary>
        /// Đỉnh tiếp theo trong đường đi ngắn nhất tới <c>root</c>
        /// </summary>
        public Node next;

        /// <summary>
        /// Số người sẽ tới được đây trong tương lai        
        /// </summary>
        public int nComingPeople;

        /// <summary>
        /// Trọng số của đường đi ngắn nhất tới <c>root</c>
        /// </summary>
        public double weight;

        /// <summary>
        /// Nhãn dán để thực hiện thuật toán dijkstra
        /// </summary>
        public bool label;
        
        /// <summary>
        /// <c>Indicator</c> trong <c>building</c> tương ứng với 
        /// <c>Node</c>, dùng trong quá trình trao đổi thông tin 
        /// giữa <c>Graph</c> và <c>Building</c>.
        /// </summary>
        public Indicator indicator;
        
        public Node()
        {
            
        }
    }
}