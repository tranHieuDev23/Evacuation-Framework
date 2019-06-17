using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PFAlgorithm;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;

namespace EvaFrame.Algorithm.PFAlgorithm.VirtualGraph
{
    /// <summary>
    /// Class mô tả cạnh trong <c>VirtualGraph</c> tương ứng với một <c>corridor</c> trong <c>Building</c>.
    /// </summary>
    class Edge
    {
        private Corridor correspondingCorridor;
        /// <value>
        /// <c>Corridor</c> trong <c>building</c> tương ứng với <c>Edge</c>.
        /// Dùng trong quá trình trao đổi thông tin giữa <c>Graph</c> và <c>Building</c>.
        /// </value>
        public Corridor CorrespondingCorridor { get { return correspondingCorridor; } }

        private Node to;
        /// <value>Đỉnh tới của cạnh.</value>
        public Node To { get { return to; } }

        /// <value>Số người hiện tại đang ở <c>Corridor</c> tương ứng. Giá trị read-only.</value>
        public int numberPeople { get => (int)correspondingCorridor.Density; }

        /// <value>
        /// Trọng số hiện tại của <c>corridor</c> tương ứng, sử dụng trong thuật toán PFAlgorithm. Giá trị read-only.
        /// </value>
        public double weight
        {
            get
            {
                return ContextFunction * correspondingCorridor.Length;
            }
        }

        /// <summary>
        /// Giá trị tính toán sự ảnh hưởng của ngoại cảnh tới vận tốc trên đoạn đường.
        /// </summary>
        public double ContextFunction 
        {
            get 
            {
                double trustness = correspondingCorridor.Trustiness;
                double density = correspondingCorridor.Density / correspondingCorridor.Capacity;
                double value = 1.0 / (trustness * (1.01 - density));
                return value > 8 ? 8 : value;
            }
        }

        /// <summary>
        /// Khởi tạo một cạnh tương ứng với <c>corridor</c>.
        /// </summary>
        /// <param name="corridor"><c>Corridor</c> nguồn khởi tạo.</param>
        /// <param name="node">Đỉnh mà cạnh này chỉ đến.</param>
        public Edge(Corridor corridor, Node node)
        {
            this.correspondingCorridor = corridor;
            this.to = node;
        }
    }
}