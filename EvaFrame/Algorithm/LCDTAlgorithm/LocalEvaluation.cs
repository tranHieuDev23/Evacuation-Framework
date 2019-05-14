using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm{

    
    public class LocalEvaluation {
        private Floor floor;
        
        LocalEvaluation() { this.floor = null; }
        LocalEvaluation(Floor floor) {
            this.floor = floor;
        }

        private double calcWeight(Corridor cor) {

            double w = cor.Length / (cor.Trustiness * (cor.Capacity - cor.Density + 1));

            return w;
        }
        /// <summary>
        ///  
        /// </summary>
        public void Run() {
            List<List<double>> wLocal = new List<List<double>>();

            foreach (Indicator indicator in floor.Indicators) 
            if (indicator.IsStairNode) {
                Dictionary<Pair<Indicator, Indicator>, double> tempWeights = runDijkstra(indicator);
            }
        }

        /// <summary>
        /// Chạy thuật toán dijkstra trên 1 
        /// </summary>
        /// <param name="start">
        /// 
        /// </param>
        /// <returns></returns>
        public Dictionary<Pair<Indicator, Indicator>, double> runDijkstra(Indicator start) {
            MinHeap<Data> heap = new MinHeap<Data>();
            Dictionary<Pair<Indicator, Indicator>, double> weights = new Dictionary<Pair<Indicator, Indicator>, double>();

            foreach (Indicator v in floor.Indicators) {
                weights[new Pair<Indicator, Indicator>(start, v)] = Double.PositiveInfinity;
            }
            
            heap.Push(new Data(start, 0));

            while(heap.Count > 0) {
                Indicator u = heap.Top().indicator;
                double wu = heap.Top().weightToExit;
                heap.Pop();

                if (weights[new Pair<Indicator, Indicator>(start, u)] != wu) continue;

                foreach (Corridor cor in u.Neighbors) {
                    Indicator v = cor.To;
                    double wv = weights[new Pair<Indicator, Indicator>(start,v)];
                    if (wv > wu + calcWeight(cor)) {
                        wv = wu + calcWeight(cor);
                        heap.Push(new Data(v, wv));

                        weights[new Pair<Indicator, Indicator>(start, v)] = wv;
                    }
                }
            }

            return weights;
        }
    }
}