using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    /// <summary>
    /// Class mô tả chức năng LocalEvaluation của module Smart Guildance Agent trong thuật toán LCDT-GV, có nhiệm vụ tìm trọng số giữa các Stair Node trong một tầng.
    /// </summary>
    class LocalEvaluation
    {
        private SubGraph subGraph;

        /// <summary>
        /// Khởi tạo thuật toán đối tượng Local Evaluation.
        /// </summary>
        /// <param name="subGraph">
        /// Đồ thị con tương ứng với một tầng.
        /// </param>
        public LocalEvaluation(SubGraph subGraph)
        {
            this.subGraph = subGraph;
        }

        /// <summary>
        /// Chạy thuật toán Local Evaluation.
        /// </summary>
        /// <returns>
        /// Một dictionary lưu trữ các trọng số giữa các cặp Stair Node trong cùng một tầng tầng.
        /// </returns>
        public Dictionary<PairNN, double> Run()
        {
            Dictionary<PairNN, double> wLocal = new Dictionary<PairNN, double>();

            foreach (Node u in subGraph.StairNodes)
            {
                Dictionary<Node, double> tempWeights = RunDijkstra(u);

                foreach (Node v in subGraph.Nodes)
                {
                    double weightToS = tempWeights[v];
                    Edge next = v.Next;
                    v.NextOptions.Add(new NodeOption(next, weightToS, u));

                    if (v.IsStairNode == true)
                    {
                        wLocal[new PairNN(v, u)] = weightToS;
                    }
                }
            }
            return wLocal;
        }

        private Dictionary<Node, double> RunDijkstra(Node start)
        {
            MinHeap<DataN> heap = new MinHeap<DataN>();
            Dictionary<Node, double> weights = new Dictionary<Node, double>();

            foreach (Node v in subGraph.Nodes)
            {
                weights[v] = Double.PositiveInfinity;
                v.Next = null;
            }

            weights[start] = 0;
            heap.Push(new DataN(start, 0));

            while (heap.Count > 0)
            {
                Node u = heap.Top().node;
                double wu = heap.Top().weightToExit;
                heap.Pop();

                if (weights[u] != wu) continue;

                foreach (Edge e in u.Adjacences)
                {
                    Node v = e.To;
                    if (weights.ContainsKey(v) == false) continue;
                    double wv = weights[v];
                    if (wv > wu + e.Weight)
                    {
                        wv = wu + e.Weight;
                        heap.Push(new DataN(v, wv));
                        v.Next = e;

                        weights[v] = wv;
                    }
                }
            }

            return weights;
        }
    }
}