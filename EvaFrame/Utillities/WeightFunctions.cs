using System;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Utilities
{
    /// <summary>
    /// Interface của hàm tính toán trọng số trên cạnh. Trọng số này có thể chỉ dựa trên độ dài vật lý của cạnh (thuật toán Dijikstra cơ bản),
    /// hoặc sử dụng một công thức phức tạp hơn dựa vào các thông số khác như khả năng thông qua, số lượng người trên cạnh, vân vân.
    /// </summary>
    public interface IWeigthFunction
    {
        /// <summary>
        /// Tính toán trọng số trên một cạnh cụ thể.
        /// </summary>
        /// <param name="corridor">Cạnh cần tính trọng số</param>
        /// <returns>Trọng số của cạnh.</returns>
        double CalculateWeight(Corridor corridor);
    }

    namespace WeightFunctions
    {
        /// <summary>
        /// Class hàm trọng số của hành lang đơn giản, chỉ trả về độ dài vật lý của hành lang.
        /// </summary>
        public class LengthOnlyFunction : IWeigthFunction
        {
            double IWeigthFunction.CalculateWeight(Corridor corridor)
            {
                return corridor.Length;
            }
        }

        /// <summary>
        /// Class hàm trọng số của hành lang dựa theo mô hình LCDT, dựa trên paper A Scalable Approach for Dynamic Evacuation Routing in Large Smart Buildings.
        /// </summary>
        public class LcdtFunction : IWeigthFunction
        {
            double IWeigthFunction.CalculateWeight(Corridor corridor)
            {
                return corridor.Length / (corridor.Trustiness * (Math.Max(corridor.Capacity - corridor.Density, 0) + 1));
            }
        }
    }
}