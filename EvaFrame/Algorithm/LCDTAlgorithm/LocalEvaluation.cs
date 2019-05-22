using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
//using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm{

    /// <summary>
    /// Loca Evaluation tìm trọng số giữa các stair node trong một tầng.
    /// </summary>
    public class LocalEvaluation {
        private SubGraph subGraph;
        
        /// <summary>
        /// Khởi tạo thuật toán Local Evaluation.
        /// </summary>
        public LocalEvaluation() { this.subGraph = null; }
        /// <summary>
        /// Khởi tạo thuật toán Local Evaluation.
        /// </summary>
        /// <param name="subGraph">
        /// Một đồ thị con tương ứng với một tầng.
        /// </param>
        public LocalEvaluation(SubGraph subGraph) {
            this.subGraph = subGraph;
        }

        
        /// <summary>
        /// Chạy thuật toán Local Evaluation.
        /// </summary>
        /// <returns>
        /// Trả về trọng số giữa các Stair Node trong cùng tầng.
        /// </returns>
        public Dictionary<PairNN, double> Run() {
            Dictionary<PairNN, double> wLocal = new Dictionary<PairNN, double>();

            foreach (Node u in subGraph.Nodes) 
            if (u.IsStairNode || u.IsExitNode) {
                Dictionary<Node, double> tempWeights = runDijkstra(u);
                
                foreach (Node v in subGraph.Nodes) {
                    double weightToS = tempWeights[v];
                    Edge next = v.Next;
                    v.NextOptions.Add(new NodeOption(next, weightToS, u));

                    if (v.IsStairNode == true && v.Equals(u) == false) {
                        //if (u.IsExitNode) Console.WriteLine("abc");
                        wLocal[new PairNN(v, u)] = weightToS;
                    }
                }

            }
            return wLocal;
        }

        /// <summary>
        /// Chạy thuật toán dijkstra từ 1 Stair Node trong Floor đến các Node khác
        /// </summary>
        /// <param name="start">
        /// Stair Node xuất phát.
        /// </param>
        /// <returns>
        ///     Trọng số giữa Stair Node xuất phát đến các Node các trong đồ thị. 
        /// </returns>
        public Dictionary<Node, double> runDijkstra(Node start) {
            MinHeap<DataN> heap = new MinHeap<DataN>();
            NodeEqualityComparer nodeCompare = new NodeEqualityComparer();
            Dictionary<Node, double>  weights = new Dictionary<Node, double>();

            foreach (Node v in subGraph.Nodes) {
                weights[v] = Double.PositiveInfinity;
                v.Next = null;
            }
            
            weights[start] = 0;
            heap.Push(new DataN(start, 0));

            while(heap.Count > 0) {
                Node u = heap.Top().node;
                double wu = heap.Top().weightToExit;
                heap.Pop();
                
                if (weights[u] != wu) continue;

                foreach (Edge e in u.Adjencents) {
                    Node v = e.To;
                    if (weights.ContainsKey(v) == false) continue;
                    double wv = weights[v];
                    if (wv > wu + e.Weight) {
                        wv = wu + e.Weight;
                        heap.Push(new DataN(v, wv));
                        v.Next = v.Adjencents.Find(edge => edge.To == u);

                        weights[v] = wv;
                    }
                }
            }

            return weights;
        }
    }
}