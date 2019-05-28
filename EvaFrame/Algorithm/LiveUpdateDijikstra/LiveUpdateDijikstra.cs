using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LiveUpdateDijikstra
{
    /// <summary>
    /// Thuật toán Dijikstra có cập nhật lại, áp dụng lên mô hình tòa nhà thông minh để tìm đường đi vật lý ngắn nhất.
    /// Người dùng có thể tự do lựa chọn hàm trọng số cạnh được sử dụng trong thuật toán này.
    /// </summary>
    public class LiveUpdateDijikstra : IAlgorithm
    {
        private Building target;
        private DijikstraAlgorithm algorithm;

        /// <summary>
        /// Khởi tạo đối tượng thuật toán ban đầu, chưa được gắn với tòa nhà mục tiêu cụ thể nào, với một hàm tính trọng số cạnh cụ thể.
        /// </summary>
        /// <param name="weightFunction">Hàm tính trọng số cạnh được sử dụng.</param>
        public LiveUpdateDijikstra(IWeigthFunction weightFunction)
        {
            this.target = null;
            this.algorithm = new DijikstraAlgorithm(weightFunction);
        }

        void IAlgorithm.Initialize(Building target)
        {
            this.target = target;
        }

        void IAlgorithm.Run()
        {
            if (target == null)
                return;

            Dictionary<Indicator, DijikstraAlgorithm.Data> calculation = algorithm.Run(target);
            foreach (Floor floor in target.Floors)
                foreach (Indicator ind in floor.Indicators)
                    ind.Next = calculation[ind].Next;
        }
    }
}