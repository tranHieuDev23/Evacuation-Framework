using System;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Utilities.WeightFunctions
{
    /// <summary>
    /// Class hàm trọng số của hành lang đơn giản, chỉ trả về độ dài vật lý của hành lang.
    /// </summary>
    public class LengthOnlyFunction : DijikstraAlgorithm.IWeigthFunction
    {
        double DijikstraAlgorithm.IWeigthFunction.calculateWeight(Corridor corridor)
        {
            return corridor.Length;
        }
    }

    /// <summary>
    /// Class hàm trọng số của hành lang dựa theo mô hình LCDT, dựa trên paper A Scalable Approach for Dynamic Evacuation Routing in Large Smart Buildings.
    /// </summary>
    public class LcdtFunction: DijikstraAlgorithm.IWeigthFunction
    {
        double DijikstraAlgorithm.IWeigthFunction.calculateWeight(Corridor corridor)
        {
            return corridor.Length / (corridor.Trustiness * (Math.Max(corridor.Capacity - corridor.Density, 0) + 1));
        }
    }
}