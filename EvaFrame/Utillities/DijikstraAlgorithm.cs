using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;

namespace EvaFrame.Utilities
{
    /// <summary>
    /// Thuật toán Dijikstra tổng quát, chạy trên class <c>Building</c>.
    /// Có thể sử dụng công thức tính trọng số trên cạnh bất kì, bằng cách cài đặt và truyền vào interface <c>IWeightFunction</c>.
    /// </summary>
    public class DijikstraAlgorithm
    {
        private IWeigthFunction weightFunction;

        /// <summary>
        /// Khởi tạo đối tượng <c>DijikstraAlgorithm</c>. Throw <c>ArgumentException</c> nếu như <c>weightFunction</c> bằng null.
        /// </summary>
        /// <param name="weightFunction"><c>IWeightFunction</c> dùng để tính trọng số các cạnh trên đồ thị.</param>
        public DijikstraAlgorithm(IWeigthFunction weightFunction)
        {
            if (weightFunction == null)
                throw new ArgumentException("weightFunction cannot be null!", "weightFunction");
            this.weightFunction = weightFunction;
        }

        /// <summary>
        /// Kết quả tính toán của thuật toán Dijikstra.
        /// </summary>
        public class Data : IComparable, ICloneable
        {
            private Indicator indicator;
            /// <value><c>Indicator</c> mà đối tượng <c>Data</c> này đang lưu trữ thông tin. Giá trị read-only.</value>
            public Indicator Indicator { get { return indicator; } }

            private double distanceToExit;
            /// <value>Khoảng cách từ <c>Indicator</c> tới Exit Node gần nhất. Giá trị read-only.</value>
            public double DistanceToExit { get { return distanceToExit; } }

            private Corridor next = null;
            /// <value>Hành lang tiếp theo mà người trên <c>Indicator</c> cần đi theo đường đi ngắn nhất. Giá trị read-only.</value>
            public Corridor Next { get { return next; } }

            /// <summary>
            /// Khởi tạo một đối tượng <c>Data</c> mới.
            /// </summary>
            /// <param name="indicator"><c>Indicator</c> mà đối tượng <c>Data</c> này đang lưu trữ thông tin.</param>
            /// <param name="distanceToExit">Khoảng cách từ <c>Indicator</c> tới Exit Node gần nhất.</param>
            /// <param name="next">Hành lang tiếp theo mà người trên <c>Indicator</c> cần đi theo đường đi ngắn nhất.</param>
            public Data(Indicator indicator, double distanceToExit, Corridor next)
            {
                this.indicator = indicator;
                this.distanceToExit = distanceToExit;
                this.next = next;
            }

            int IComparable.CompareTo(object obj)
            {
                if (obj.GetType() != typeof(Data))
                    throw new ArgumentException("obj is not the same type as this instance.");
                Data data = obj as Data;
                return distanceToExit.CompareTo(data.distanceToExit);
            }

            object ICloneable.Clone() { return new Data(indicator, distanceToExit, next); }
        }

        /// <summary>
        /// Chạy thuật toán Dijikstra trên một đối tượng <c>Building</c> cụ thể.
        /// </summary>
        /// <param name="target">Đối tượng <c>Building</c> cần tính toán.</param>
        /// <returns>Một đối tượng <c>Dictionary</c> ánh xạ các đối tượng <c>Indicator</c> tới kết quả tính toán tương ứng.</returns>
        public Dictionary<Indicator, Data> Run(Building target)
        {
            // Các cấu trúc dữ liệu cần thiết cho thuật toán Dijikstra
            MinHeap<Data> heap = new MinHeap<Data>();
            Dictionary<Indicator, Data> result = new Dictionary<Indicator, Data>();

            // Gán tất cả các đỉnh thoát với khoảng cách bằng 0, và các đỉnh còn lại với khoảng cách dương vô cùng.
            foreach (Floor f in target.Floors)
                foreach (Indicator i in f.Indicators)
                {
                    if (i.IsExitNode)
                    {
                        Data exitData = new Data(i, 0, null);
                        result[i] = exitData;    
                        heap.Push(exitData);
                    }
                    else
                        result[i] = new Data(i, Double.PositiveInfinity, null);
                }

            // Nội dung thuật toán Dijikstra
            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();
                Indicator u = data.Indicator;
                if (data.DistanceToExit != result[u].DistanceToExit)
                    continue;
                
                double wu = data.DistanceToExit;
                foreach (Corridor c in u.Neighbors)
                {
                    Indicator v = c.To(u);
                    double wv = result[v].DistanceToExit;
                    double wc = weightFunction.CalculateWeight(c);
                    if (wv <= wu + wc)
                        continue;

                    wv = wu + wc;
                    Data vData = new Data(v, wv, c);
                    result[v] = vData;
                    heap.Push(vData);
                }
            }

            return result;
        }
    }
}