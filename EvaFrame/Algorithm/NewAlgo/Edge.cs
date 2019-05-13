using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo
{
    public class Edge
    {
        /// <summary>
        /// <c>Corridor</c> trong <c>building</c> tương ứng với 
        /// <c>Edge</c>, dùng trong quá trình trao đổi thông tin 
        /// giữa <c>Graph</c> và <c>Building</c>.
        /// </summary>
        public Corridor corridor;

        public Node to;

        public double lenght;

        public double width;

        public double trustNess;

        public double density;

        /// <summary>
        /// Số người hiện tại đang ở cạnh
        /// </summary>
        public int numberPeople;

        public double weight;

        public Edge(){

        }

    }
}