using System;
using System.Linq;
using System.Collections.Generic;

using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithms.LCDTAlgorithm.Utilities
{
    /// <summary>
    /// Dữ liệu tương ứng với đỉnh ảo dùng cho thuật toán Dijkstra.
    /// </summary>
    class DataN : IComparable, ICloneable
    {
        /// <summary>
        /// Đỉnh ảo tương ứng.
        /// </summary>
        public Node node;
        /// <summary>
        /// Trọng số  đến đỉnh nguồn trong thuật toán Dijkstra. 
        /// </summary>
        public double weightToExit;

        /// <summary>
        /// Khởi tạo dữ liệu.
        /// </summary>
        /// <param name="node"> Đỉnh ảo tương ứng. </param>
        /// <param name="weightToExit"> Trọng số đến đỉnh nguồn trong thuật toán Dijkstra. </param>
        public DataN(Node node, double weightToExit)
        {
            this.node = node;
            this.weightToExit = weightToExit;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj.GetType() != typeof(DataN))
                throw new ArgumentException("obj is not the same type as this instance.");
            DataN data = obj as DataN;
            return weightToExit.CompareTo(data.weightToExit);
        }

        object ICloneable.Clone() { return new DataN(node, weightToExit); }
    }

    /// <summary>
    /// Cặp đỉnh ảo vs đỉnh ảo.
    /// </summary>
    class PairNN
    {
        private Node first, second;
        /// <summary>
        /// Khởi tạo cặp đỉnh ảo.
        /// </summary>
        /// <param name="first"> Đỉnh ảo thứ nhất.</param>
        /// <param name="second"> Đỉnh ảo thứ hai.</param>
        public PairNN(Node first, Node second)
        {
            this.first = first;
            this.second = second;
        }

        /// <value> Trả về đỉnh ảo thứ nhất.</value>
        public Node First
        {
            get { return first; }
            set
            {
                first = value;
            }
        }
        /// <value> Trả về đỉnh ảo thứ hai.</value>
        public Node Second
        {
            get { return second; }
            set
            {
                second = value;
            }
        }
    }

    /// <summary>
    /// So sánh 2 cặp đỉnh ảo.
    /// </summary>
    class NodeEqualityComparer : IEqualityComparer<PairNN>
    {
        /// <summary>
        /// So sánh bằng.
        /// </summary>
        /// <param name="p1"> Cặp đỉnh ảo thứ nhất.</param>
        /// <param name="p2"> Cặp đỉnh ảo thứ hai.</param>
        /// <returns></returns>
        public bool Equals(PairNN p1, PairNN p2)
        {
            if (p1 == null && p2 == null) return true;
            if (p1 == null || p2 == null) return false;
            if (p1.First.CorresspodingIndicator.Equals(p2.First.CorresspodingIndicator)
                && p1.Second.CorresspodingIndicator.Equals(p2.Second.CorresspodingIndicator)) return true;
            return false;
        }

        /// <summary>
        /// Hashcode của cặp đỉnh ảo
        /// </summary>
        /// <param name="p"> Đỉnh ảo cần lấy HashCode.</param>
        /// <returns></returns>
        public int GetHashCode(PairNN p)
        {
            if (p.First == null || p.Second == null) return 0;
            int hCode = p.First.CorresspodingIndicator.GetHashCode() ^ p.Second.CorresspodingIndicator.GetHashCode();
            return hCode.GetHashCode();
        }
    }

    /// <summary>
    /// Những hàm mở rộng để thuận tiện cho việc tính toán.
    /// </summary>
    static class ExtensionMethod
    {
        /// <summary>
        /// Tính trọng số của Corridor.
        /// </summary>
        /// <param name="cor"> Corridor cần tính trọng số.</param>
        /// <returns> Trọng số tính trong thuật toán LCDT.</returns>
        public static double LCDTWeight(this Corridor cor)
        {
            IWeigthFunction function = new LcdtFunction();
            return function.CalculateWeight(cor);;
        }

        /// <summary>
        /// Trả về tầng của Indicator.
        /// </summary>
        /// <param name="indicator"> Indicator cần lấy số tầng.</param>
        /// <returns> Chỉ số tầng của Indicator. </returns>
        public static int getFloorNumber(this Indicator indicator)
        {
            string[] arr = indicator.Id.Split('@');
            return System.Convert.ToInt32(arr[1]);
        }

        /// <summary>
        /// Lấy chỉ số Id của một indicator.
        /// </summary>
        /// <param name="indicator"> Indicator muốn lấy Id. </param>
        /// <returns> Id của indicator.</returns>
        public static int getIdNumber(this Indicator indicator)
        {
            string[] arr = indicator.Id.Split('@');
            return System.Convert.ToInt32(arr[0]);
        }
    }
}