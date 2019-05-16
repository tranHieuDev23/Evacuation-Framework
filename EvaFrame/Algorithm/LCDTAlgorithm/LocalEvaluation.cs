using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
//using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm{

    
    public class LocalEvaluation {
        private SubGraph subGraph;
        
        public LocalEvaluation() { this.subGraph = null; }
        public LocalEvaluation(SubGraph subGraph) {
            this.subGraph = subGraph;
        }

        
        /// <summary>
        ///  
        /// </summary>
        public Dictionary<PairNN, double> Run() {
            Dictionary<PairNN, double> wLocal = new Dictionary<PairNN, double>();

            foreach (Node u in subGraph.Nodes) 
            if (u.IsStairNode) {
                Dictionary<PairNN, double> tempWeights = runDijkstra(u);
                
                foreach (Node v in subGraph.Nodes) {
                    double weightToS = tempWeights[new PairNN(u,v)];
                    Edge next = v.Next;
                    v.NextOptions.Add(new NodeOption(next, weightToS, u));

                    if ((v.IsStairNode == true || v.IsExitNode == true) && v.Equals(u) == false) {
                        wLocal[new PairNN(u,v)] = weightToS;
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
        public Dictionary<PairNN, double> runDijkstra(Node start) {
            MinHeap<Data> heap = new MinHeap<Data>();
            NodeEqualityComparer nodeCompare = new NodeEqualityComparer();
            Dictionary<PairNN, double>  weights = new Dictionary<PairNN, double>(nodeCompare);

            foreach (Node v in subGraph.Nodes) {
                weights[new PairNN(start, v)] = Double.PositiveInfinity;
                v.Next = null;
            }
            
            weights[new PairNN(start, start)] = 0;
            heap.Push(new Data(start, 0));

            while(heap.Count > 0) {
                Node u = heap.Top().node;
                double wu = heap.Top().weightToExit;
                heap.Pop();
                
                if (weights[new PairNN(start, u)] != wu) continue;

                foreach (Edge e in u.Adjencents) {
                    Node v = e.To;
                    PairNN sv = new PairNN(start, v);
                    if (weights.ContainsKey(sv) == false) continue;
                    double wv = weights[sv];
                    if (wv > wu + e.Weight) {
                        wv = wu + e.Weight;
                        heap.Push(new Data(v, wv));
                        v.Next = v.Adjencents.Find(edge => edge.To == u);

                        weights[new PairNN(start, v)] = wv;
                    }
                }
            }

            return weights;
        }
    }
}