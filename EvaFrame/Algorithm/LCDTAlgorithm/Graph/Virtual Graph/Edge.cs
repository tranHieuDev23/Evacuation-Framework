using System;

using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    /// <summary>
    /// Cạnh ảo của đồ thị ảo tưởng ứng với một Corridor trong Building.
    /// </summary>
    public class Edge
    {
        private Corridor correspondingCorridor;
        /// <value>Corridor tương ứng với cạnh ảo.</value>
        public Corridor CorrespondingCorridor { get { return correspondingCorridor; } }

        private Node from;
        /// <value>Đỉnh ảo xuất phát.</value>
        public Node From { get { return from; } }

        private Node to;
        /// <value>Đỉnh ảo đích.</value>
        public Node To { get { return to; } }

        private double weight;
        /// <value> Trọng số của cạnh ảo. </value>
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        /// <summary>
        /// Khởi tạo cạnh ảo.
        /// </summary>
        /// <param name="from"> Đỉnh ảo nguồn. </param>
        /// <param name="to"> Đỉnh ảo đích. </param>
        /// <param name="weight"> Trọng số của cạnh ảo.</param>
        /// <param name="cor"> Corridor tương ứng trong Building. </param>
        public Edge(Node from, Node to, double weight, Corridor cor = null)
        {
            if (to == null)
                throw new ArgumentNullException("To is Null");
            if (from == null)
                throw new ArgumentNullException("From is Null");
            this.from = from;
            this.to = to;
            this.weight = weight;
            this.correspondingCorridor = cor;
        }
        
        /// <summary>
        /// Khởi tạo cạnh ảo.
        /// </summary>
        /// <param name="cor"> Corridor tương ứng trong Building. </param>
        public Edge(Corridor cor)
        {
            this.correspondingCorridor = cor;
        }
    }
}