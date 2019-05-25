using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.NewAlgo;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;

namespace EvaFrame.Algorithm.NewAlgo.VirtualGraph
{
    /// <summary>
    /// Cạnh trong <c>VirtualGraph</c> biểu thị tương ứng cho <c>corridor</c> 
    /// trong <c>building</c>
    /// </summary>
    public class Edge
    {
        private Corridor correspondingCorridor;
        /// <value>
        /// <c>Corridor</c> trong <c>building</c> tương ứng với 
        /// <c>Edge</c>, dùng trong quá trình trao đổi thông tin 
        /// giữa <c>Graph</c> và <c>Building</c>.
        /// </value>
        public Corridor CorrespondingCorridor { get { return correspondingCorridor; } }

        private Node to;
        /// <value>
        /// 
        /// </value>
        public Node To { get { return to; } }

        /// <summary>
        /// Số người hiện tại đang ở <c>corridor</c> tương ứng
        /// </summary>
        /// <value>Giá trị read-only</value>
        public int numberPeople
        {
            get
            {
                return (int) correspondingCorridor.Density;
            }
        }

        /// <summary>
        /// Trọng số hiện tại của <c>corridor</c> tương ứng
        /// </summary>
        /// <value>Giá trị read-only</value>
        public double weight
        {
            get
            {
                return Utility.ContextFunction(correspondingCorridor.Trustiness, 
                                                correspondingCorridor.Density / correspondingCorridor.Capacity)
                     * correspondingCorridor.Length;
            }
        }

        private static IWeigthFunction weightFunction = new LcdtFunction();

        /// <summary>
        /// Khởi tạo một cạnh tương ứng với <c>corridor</c>
        /// </summary>
        /// <param name="corridor"><c>corridor</c> nguồn khởi tạo</param>
        /// <param name="node">Đỉnh mà cạnh này chỉ đến</param>
        public Edge(Corridor corridor, Node node)
        {
            correspondingCorridor = corridor;
            to = node;
        }
    }
}