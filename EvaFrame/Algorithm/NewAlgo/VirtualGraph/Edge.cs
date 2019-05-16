using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

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
        public int numberPeople;

        /// <summary>
        /// 
        /// </summary>
        public double weight;

        /// <summary>
        /// 
        /// </summary>
        public Edge()
        {

        }

    }
}