using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
//using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm{

    
    public class LocalEvaluation {
        private Floor floor;
        
        public LocalEvaluation() { this.floor = null; }
        public LocalEvaluation(Floor floor) {
            this.floor = floor;
        }

        
        /// <summary>
        ///  
        /// </summary>
        public Dictionary<PairII, double> Run() {
            Dictionary<PairII, double> wLocal = new Dictionary<PairII, double>();

            foreach (Indicator u in floor.Indicators) 
            if (u.IsStairNode) {
                Dictionary<PairII, double> tempWeights = runDijkstra(u);
                foreach (Indicator v in floor.Indicators) {
                    double weightToS = tempWeights[new PairII(u,v)];
                    Corridor next = v.Next;
                    v.NextOptions.add(next, weightToS, u);

                    if ((v.IsStairNode == true || v.IsExitNode == true) && v.Equals(u) == false) {
                        wLocal[new PairII(u,v)] = weightToS;
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
        ///     Trọng số giữa Stair Node xuất phát đến các Node các trong tầng. 
        /// </returns>
        public Dictionary<PairII, double> runDijkstra(Indicator start) {
            MinHeap<Data> heap = new MinHeap<Data>();
            Dictionary<PairII, double> weights = new Dictionary<PairII, double>();

            foreach (Indicator v in floor.Indicators) {
                weights[new PairII(start, v)] = Double.PositiveInfinity;
                v.Next = null;
            }
            
            heap.Push(new Data(start, 0));

            while(heap.Count > 0) {
                Indicator u = heap.Top().indicator;
                double wu = heap.Top().weightToExit;
                heap.Pop();

                if (weights[new PairII(start, u)] != wu) continue;

                foreach (Corridor cor in u.Neighbors) {
                    Indicator v = cor.To;
                    double wv = weights[new PairII(start,v)];
                    if (wv > wu + cor.calcWeight()) {
                        wv = wu + cor.calcWeight();
                        heap.Push(new Data(v, wv));
                        v.Next = v.Neighbors.Find(corridor => corridor.To == u);

                        weights[new PairII(start, v)] = wv;
                    }
                }
            }

            return weights;
        }
    }
}