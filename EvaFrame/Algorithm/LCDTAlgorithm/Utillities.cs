using System;
using System.Linq;
using System.Collections.Generic;

using EvaFrame.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Utilities {

    /// <summary>
    /// Dữ liệu tương ứng với đỉnh ảo dùng cho thuật toán Dijkstra.
    /// </summary>
    public class DataN: IComparable, ICloneable {
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
    /// Dữ liệu tương ứng với đỉnh ảo dùng cho thuật toán Dijkstra.
    /// </summary>
    public class DataI: IComparable, ICloneable {
        /// <summary>
        /// Indicator tương ứng.
        /// </summary>
        public Indicator node;
        /// <summary>
        /// Trọng số đến Indicator nguồn trong thuật toán Dijkstra;
        /// </summary>
        public double weightToExit;

        /// <summary>
        /// Khởi tạo dữ liệu.
        /// </summary>
        /// <param name="node"> Indicator tương ứng. </param>
        /// <param name="weightToExit"> Trọng số đến Indicator nguồn trong thuật toán Dijkstra. </param>
        public DataI(Indicator node, double weightToExit)
        {
            this.node = node;
            this.weightToExit = weightToExit;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj.GetType() != typeof(DataI))
                throw new ArgumentException("obj is not the same type as this instance.");
            DataI data = obj as DataI;
            return weightToExit.CompareTo(data.weightToExit);
        }

        object ICloneable.Clone() { return new DataI(node, weightToExit); }
    }

    /// <summary>
    /// Cặp đỉnh ảo vs đỉnh ảo.
    /// </summary>
    public class PairNN {
        private Node first, second;
        /// <summary>
        /// Khởi tạo cặp đỉnh ảo.
        /// </summary>
        /// <param name="first"> Đỉnh ảo thứ nhất.</param>
        /// <param name="second"> Đỉnh ảo thứ hai.</param>
        public PairNN(Node first, Node second) {
            this.first = first;
            this.second = second;
        }
        
        /// <value> Trả về đỉnh ảo thứ nhất.</value>
        public Node First{
            get { return first;}
            set {
                first = value;
            }
        }
        /// <value> Trả về đỉnh ảo thứ hai.</value>
        public Node Second{
            get{ return second; }
            set{
                second = value;
            }}
    }

    /// <summary>
    /// So sánh 2 cặp đỉnh ảo.
    /// </summary>
    public class NodeEqualityComparer: IEqualityComparer<PairNN> {
        /// <summary>
        /// So sánh bằng.
        /// </summary>
        /// <param name="p1"> Cặp đỉnh ảo thứ nhất.</param>
        /// <param name="p2"> Cặp đỉnh ảo thứ hai.</param>
        /// <returns></returns>
        public bool Equals(PairNN p1, PairNN p2) {
            if (p1 == null && p2 == null) return true;
            if (p1 == null || p2 == null) return false;
            //if (p1.First == p2.First && p1.Second == p2.Second) return true;
            if (p1.First.CorresspodingIndicator.Equals(p2.First.CorresspodingIndicator) 
                && p1.Second.CorresspodingIndicator.Equals(p2.Second.CorresspodingIndicator)) return true;
            return false;
        }

        /// <summary>
        /// Hashcode của cặp đỉnh aor
        /// </summary>
        /// <param name="p"> Đỉnh ảo cần lấy HashCode.</param>
        /// <returns></returns>
        public int GetHashCode(PairNN p) {
            if (p.First == null || p.Second == null) return 0;
            int hCode = p.First.CorresspodingIndicator.GetHashCode() ^ p.Second.CorresspodingIndicator.GetHashCode();
            return hCode.GetHashCode();    
        }
    }

    /// <summary>
    /// Những hàm mở rộng để thuận tiện cho việc tính toán.
    /// </summary>
    public static class ExtensionMethod {

        /// <summary>
        /// Tính trọng số của Corridor.
        /// </summary>
        /// <param name="cor"> Corridor cần tính trọng số.</param>
        /// <returns></returns>
        public static double LCDTWeight(this Corridor cor) {
            if (cor.Density >= Init.Beta * cor.Capacity) return 1e7;
            double w = cor.Length / (cor.Trustiness * (Math.Max(cor.Capacity - cor.Density,0) + 1));
            return w;
        }

        /// <summary>
        /// Tính weight của Corridor trong thuật toán sử dụng cache.
        /// </summary>
        /// <param name="cor"> Corridor cần tính weight.</param>
        /// <returns></returns>
        public static double CacheWeight(this Corridor cor) {
            return cor.Length / cor.Width;
        }

        /// <summary>
        /// Trả về tầng của Indicator.
        /// </summary>
        /// <param name="indicator"> Indicator cần lấy số tầng.</param>
        /// <returns></returns>
        public static int getFloorNumber(this Indicator indicator) {
            string[] arr = indicator.Id.Split('@');

            return System.Convert.ToInt32(arr[1]);
        }

        public static int getIdNumber(this Indicator indicator) {
            string[] arr = indicator.Id.Split('@');

            return System.Convert.ToInt32(arr[0]);
        }

        /// <summary>
        /// Clone Corridor.
        /// </summary>
        /// <param name="item"> Corridor cần Clone.</param>
        /// <returns></returns>
        public static Corridor CorClone(this Corridor item) {
            Corridor newItem = new Corridor(item.I1, item.I2, item.IsStairway, item.Length, item.Width, item.Density, item.Trustiness);
            return newItem;
        }

    }
    
}