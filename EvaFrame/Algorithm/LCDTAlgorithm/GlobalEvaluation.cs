using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class GlobalEvaluation {
        private CrossGraph crossGraph;

        public GlobalEvaluation() {}
        /// <summary>
        /// Khởi tạo thuật toán Global Evaluation.
        /// </summary>
        /// <param name="crossGraph">
        /// Một đồ thị ảo giữa các Stair Node với nhau và với Exit Node.
        /// </param>
        public GlobalEvaluation(CrossGraph crossGraph) {
            this.crossGraph = crossGraph;
        }
        
        /// <summary>
        /// Chạy thuật toán Global Evaluation.
        /// </summary>
        /// <returns>
        /// Trả trọng số giữa các Stair Node với Exit Node.
        /// </returns>
        public Dictionary<PairNN, double> Run() {
            Dictionary<PairNN, double> wGlobal = new Dictionary<PairNN, double>(new NodeEqualityComparer());

            foreach (Node exitNode in crossGraph.Nodes) 
            if (exitNode.IsExitNode == true) {
                //Console.WriteLine("Id : {0}", exitNode.CorresspodingIndicator.Id);
                Dictionary<PairNN, double> tempWeights = dijkstra(exitNode);
                

                foreach (KeyValuePair<PairNN, double> item in tempWeights) {
                    wGlobal[item.Key] = item.Value;
                }

                //Node stairNode = crossGraph.Nodes.Find( node => node.IsStairNode == true );
                //bool checkGlobalEvaluation = tempWeights.ContainsKey(new PairNN(stairNode, exitNode));
            }

            return wGlobal;
        }

        /// <summary>
        /// Thuật toán Dijkstra với đỉnh xuất phát là 1 Exit Node.
        /// </summary>
        /// <param name="exitNode">
        /// Exit Node xuất phát.
        /// </param>
        /// <returns>
        /// Trọng số đường đi ngắn nhất từ Exit Node đến các Stair Node.
        /// </returns>
        public Dictionary<PairNN, double> dijkstra(Node exitNode) {
            Dictionary<PairNN, double> weights = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            MinHeap<Data> heap = new MinHeap<Data>();

            foreach (Node u in crossGraph.Nodes) {
                weights[new PairNN(u, exitNode)] = double.PositiveInfinity;
                u.Next = null;
            }

            weights[new PairNN(exitNode, exitNode)] = 0;
            heap.Push(new Data(exitNode, 0));

            while(heap.Count > 0) {
                Node u = heap.Top().node;
                double wu = heap.Top().weightToExit;
                heap.Pop();

                if (weights[new PairNN(u, exitNode)] != wu) continue;

                foreach (Edge e in u.Adjencents) {
                    Node v = e.To;
                    PairNN ev = new PairNN(exitNode, v);
                    double wv = weights[ev];

                    if (wv > wu + e.Weight) {
                        wv = wu + e.Weight;
                        weights[new PairNN(v, exitNode)] = wv;
                        v.Next = v.Adjencents.Find(edge => edge.To == u);
                        heap.Push(new Data(v, wv));
                    }
                }
            }


            return weights;
        }

    }
}