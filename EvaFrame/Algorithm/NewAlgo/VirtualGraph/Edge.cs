using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.NewAlgo;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;

namespace EvaFrame.Algorithm.NewAlgo.VirtualGraph
{
    /// <summary>
    /// 
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
        /// Số người hiện tại đang ở cạnh
        /// </summary>
        public int numberPeople
        {
            get
            {
                return (int) correspondingCorridor.Density;
            }
        }

        /// <summary>
        /// Trọng số của cạnh
        /// </summary>
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
        /// 
        /// </summary>
        public Edge(Corridor cor, Node nod)
        {
            correspondingCorridor = cor;
            to = nod;
        }
    }
}