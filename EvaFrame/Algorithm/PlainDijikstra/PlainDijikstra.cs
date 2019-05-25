using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.PlainDijikstra
{
    /// <summary>
    /// Thuật toán Dijikstra cổ điển, áp dụng lên mô hình tòa nhà thông minh để tìm đường đi vật lý ngắn nhất.
    /// Do thuật toán chỉ thao tác lên thông số vật lý không đổi (độ dài con đường) nên thuật toán chỉ chạy một
    /// lần duy nhất sau khi khởi tạo.
    /// </summary>
    public class PlainDijikstra : IAlgorithm
    {
        private Building target;
        private bool done;

        /// <summary>
        /// Khởi tạo đối tượng thuật toán ban đầu, chưa được gắn với tòa nhà mục tiêu cụ thể nào.
        /// </summary>
        public PlainDijikstra() { target = null; done = false; }

        void IAlgorithm.Initialize(Building target)
        {
            this.target = target;
            this.done = false;
        }

        void IAlgorithm.Run()
        {
            if (target == null || done)
                return;
            done = true;

            DijikstraAlgorithm algorithm = new DijikstraAlgorithm(new LcdtFunction());
            Dictionary<Indicator, DijikstraAlgorithm.Data> calculation = algorithm.Run(target);
            foreach (Floor floor in target.Floors)
                foreach (Indicator ind in floor.Indicators)
                    ind.Next = calculation[ind].Next;
                
        }
    }
}