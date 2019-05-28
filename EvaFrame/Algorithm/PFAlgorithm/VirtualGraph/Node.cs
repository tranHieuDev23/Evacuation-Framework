using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.PFAlgorithm.VirtualGraph
{
    /// <summary>
    /// cấu trúc lưu thông tin các đỉnh và cạnh gần kề 
    /// với đỉnh đang xét
    /// </summary>
    class Adjacence
    {
        /// <summary>
        /// Đỉnh gần kề
        /// </summary>
        public Node node;
        /// <summary>
        /// Đỉnh đi tới được sau t(s) bằng 
        /// cách đi qua node gần kề
        /// </summary>
        public Node reaching;
        /// <summary>
        /// Cạnh gần kề
        /// </summary>
        public Edge edge;
        /// <summary>
        /// Trọng số con đường qua đỉnh gần 
        /// kề đi đến root 
        /// </summary>
        public double passingWeight;
    }

    /// <summary>
    /// Class thu gọn của Indicator với mục đích chỉ dành cho
    /// thực hiện thuật toán
    /// </summary>
    class Node
    {   
        /// <summary>
        /// Danh sách các thông tin gần kề
        /// </summary>
        public List<Adjacence> adjacences;

        /// <summary>
        /// Danh sách đỉnh tới được đây trong tương lai
        /// </summary>

        public List<Node> comingNodes;

        /// <summary>
        /// Đỉnh sẽ tới được trong tương lai
        /// </summary>
        public Node reachedNode;

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

        private Indicator correspondingIndicator;
        
        /// <summary>
        /// <c>Indicator</c> trong <c>building</c> tương ứng với 
        /// <c>Node</c>, dùng trong quá trình trao đổi thông tin 
        /// giữa <c>Graph</c> và <c>Building</c>.
        /// </summary>
        public Indicator CorrespondingIndicator
        {
            get
            {
                return correspondingIndicator;
            }
        }
        
        /// <summary>
        /// Khởi tạo một đỉnh tương ứng với <c>indicator</c>
        /// </summary>
        /// <param name="indicator"><c>Indicator</c> nguồn khởi tạo</param>
        public Node(Indicator indicator)
        {
            this.correspondingIndicator = indicator;
            this.adjacences = new List<Adjacence>();
            this.comingNodes = new List<Node>();
        }
    }
}