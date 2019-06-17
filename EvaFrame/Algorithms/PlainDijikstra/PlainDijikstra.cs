using System.Collections.Generic;
using EvaFrame.Algorithms;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithms.PlainDijikstra
{
    /// <summary>
    /// Thuật toán Dijikstra cổ điển, áp dụng lên mô hình tòa nhà thông minh để tìm đường đi vật lý ngắn nhất.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Do thuật toán chỉ thao tác lên thông số vật lý không đổi (độ dài con đường) nên thuật toán chỉ 
    /// chạy một lần duy nhất sau khi khởi tạo.
    /// </para>
    /// <para>
    /// Thuật toán này được cài đặt trong thư viên để người dùng có thể dễ dàng áp dụng và theo dõi những 
    /// nhược điểm của các thuật toán tìm đường cơ bản trong bài toán tìm đường thoát hiểm.
    /// </para>
    /// </remarks>
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

            DijikstraAlgorithm algorithm = new DijikstraAlgorithm(new LengthOnlyFunction());
            Dictionary<Indicator, DijikstraAlgorithm.Data> calculation = algorithm.Run(target);
            foreach (Floor floor in target.Floors)
                foreach (Indicator ind in floor.Indicators)
                    ind.Next = calculation[ind].Next;
        }
    }
}