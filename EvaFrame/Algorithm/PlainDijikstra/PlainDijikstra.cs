using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Utilities;
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

        // Dữ liệu trong Min Heap của thuật toán Dijikstra
        private class Data : IComparable, ICloneable
        {
            public Indicator indicator;
            public double weightToExit;

            public Data(Indicator indicator, double weightToExit)
            {
                this.indicator = indicator;
                this.weightToExit = weightToExit;
            }

            int IComparable.CompareTo(object obj)
            {
                if (obj.GetType() != typeof(Data))
                    throw new ArgumentException("obj is not the same type as this instance.");
                Data data = obj as Data;
                return weightToExit.CompareTo(data.weightToExit);
            }

            object ICloneable.Clone() { return new Data(indicator, weightToExit); }
        }

        void IAlgorithm.Run()
        {   
            // Nếu như thuật toán chưa được gắn với tòa nhà mục tiêu nào, hoặc đã chạy xong một lần
            if (target == null || done)
                return;
            done = true;
            Console.WriteLine("Algorithm start running!");

            // Các cấu trúc dữ liệu cần thiết cho thuật toán Dijikstra
            MinHeap<Data> heap = new MinHeap<Data>();
            Dictionary<Indicator, double> weightToExit = new Dictionary<Indicator, double>();

            // Gán tất cả các đỉnh trên tòa nhà với khoảng cách dương vô cùng.
            foreach (Floor f in target.Floors)
                foreach (Indicator i in f.Indicators)
                {
                    i.Next = null;
                    weightToExit[i] = Double.PositiveInfinity;
                }
            // Gán tất cả các đỉnh thoát với khoảng cách bằng 0.
            foreach (Indicator exit in target.Exits)
            {
                weightToExit[exit] = 0;
                heap.Push(new Data(exit, 0));
            }

            // Nội dung thuật toán Dijikstra
            while(heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Indicator u = data.indicator;
                double wu = data.weightToExit;
                if (weightToExit[u] != wu)
                    continue;
                
                foreach (Corridor c in u.Neighbors)
                {
                    Indicator v = c.To;
                    double wv = weightToExit[v];
                    if (wv <= wu + c.Length)
                        continue;
                    wv = wu + c.Length;
                    v.Next = v.Neighbors.Find(cor => cor.To == u);
                    weightToExit[v] = wv;
                    heap.Push(new Data(v, wv));
                }
            }

            Console.WriteLine("Algorithm finished running!");
        }
    }
}