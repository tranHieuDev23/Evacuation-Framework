using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    /// <summary>
    /// Các lựa chọn của đỉnh ảo.
    /// </summary>
    public class NodeOption {
        private Edge next;
        /// <value> Cạnh ảo mà người di tản có thể lựa chọn. </value>
        public Edge Next { get { return next; } }
        private double weightToS;
        /// <value> Trọng số của cạnh ảo tướng ứng. </value>
        public double WeightToS { get { return weightToS; } }
        private Node stairNode;
        /// <value> Stair node tương ứng dùng để đi xuống tầng thấp hơn. </value>
        public Node StairNode { get { return stairNode; } }

        /// <summary>
        /// Khởi tạo một lựa chọn cho đỉnh ảo.
        /// </summary>
        /// <param name="next"> Cạnh ảo mà người di tản có thể lựa chọn. </param>
        /// <param name="weightToS"> Trọng số tương ứng với cạnh ảo. </param>
        /// <param name="stairNode"> Stair node để đi xuống tầng dưới. </param>
        public NodeOption(Edge next, double weightToS, Node stairNode) {
            this.next = next;
            this.weightToS = weightToS;
            this.stairNode = stairNode;
        }
    }
    
    /// <summary>
    /// Đỉnh ảo.
    /// </summary>
    public class Node {
        private Indicator corresspondingIndicator;
        /// <value> Indicator tương ứng trong Building. </value>
        public Indicator CorresspodingIndicator{ get{ return corresspondingIndicator; } }
        
        /// <summary>
        /// Khởi tạo đỉnh ảo.
        /// </summary>
        /// <param name="indicator"> Indicator tương ứng trong Building. </param>
        public Node(Indicator indicator) {
            this.corresspondingIndicator = indicator;
            this.adjencents = new List<Edge>();
            this.isStairNode = indicator.IsStairNode;
            this.isExitNode = indicator.IsExitNode;
            this.next = null;
            this.nextOptions = new List<NodeOption>();
        }

        private bool isStairNode;
        /// <value> Trả true nếu đây là Stair. Ngược lại trả về false. </value>
        public bool IsStairNode{ 
            get { return isStairNode; } 
            set { isStairNode = value; }
        }
        private bool isExitNode;
        /// <value> Trả về true nếu đây là cổng thoát khỏi tòa nhà. Ngược lại trả về false. </value>
        public bool IsExitNode{ 
            get { return isExitNode; } 
            set { isExitNode = value; }
        }
        
        private List<Edge> adjencents;
        /// <value> Danh sách các cảnh ảo kề với đỉnh. </value>
        public List<Edge> Adjencents{ get { return adjencents; } }

        private Edge next;
        /// <value> Cạnh tiếp theo mà người dân sẽ di tản. </value>
        public Edge Next {
            get { return next; }
            set {

                if (value != null)
                    corresspondingIndicator.Next = value.CorrespondingCorridor;
                next = value;
            }
        }

        private List<NodeOption> nextOptions;
        /// <value> Danh sách các lựa chọn mà người dân có thể di tản vào. </value>
        public List<NodeOption> NextOptions{
            get { return nextOptions; }
            set { nextOptions = value; }
        }
    }

}