using System;
using System.Collections.Generic;

using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    class LCDTAlgorithm : IAlgorithm {
        private Building target;
        private Graph graph;
        /// <summary>
        /// Khởi tạo thuật toán.
        /// </summary>
        /// <param name="target">
        /// Thông số của tòa nhà.
        /// </param>
        void IAlgorithm.Initialize(Building target) {
            this.target = target;
            this.graph = new Graph();
        }
        /// <summary>
        /// Chạy thuật toán LCDT.
        /// </summary>

        void IAlgorithm.Run() {
            Dictionary<PairNN, double> wLocals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            CrossGraph crossGraph = new CrossGraph(target);

            for (int i = 0; i < target.Floors.Count; ++i) {
                SubGraph subGraph;
                if (i == 0) subGraph = new SubGraph(target.Floors[0], target);
                else subGraph = new SubGraph(target.Floors[i]);
                LocalEvaluation local = new LocalEvaluation(subGraph);
                Dictionary<PairNN, double> weightInFloor = local.Run();

                crossGraph.updateGraph(weightInFloor);
                foreach(KeyValuePair<PairNN, double> item in weightInFloor) {
                    wLocals[item.Key] = item.Value;
                }

                graph.addSubGraph(subGraph);

                //Console.WriteLine("Floor {0} done!", i);
            }
            
            /* for (int i = 0; i < target.Floors.Count; ++i) {
                Console.WriteLine("Number of node in Floor {0}: {1}",i ,target.Floors[i].Indicators.Count);
                double maxCorLen = 0;
                int n50 = 0, n100 = 0;

                foreach (Indicator ind in target.Floors[i].Indicators) {
                    foreach (Corridor cor in ind.Neighbors) {
                        if ((cor.From.IsStairNode || cor.From.IsExitNode) && 
                            (cor.To.IsStairNode || cor.To.IsExitNode)) continue;
                        if (cor.Length > maxCorLen) {
                            maxCorLen = cor.Length;
                        }
                        if (cor.Length > 50) {
                            n50 += 1;
                        }
                        if (cor.Length > 100) {
                            n100 += 1;
                        }
                    }
                }

                Console.WriteLine("Max lenght of cor in Floor {0}: {1}", i, maxCorLen);
                Console.WriteLine("Number of cor with len > 50 in Floor {0}: {1}", i, n50);
                Console.WriteLine("Number of cor with len > 100 in Floor {0}: {1}", i, n100);
            }*/
            

            crossGraph.buildGraph();
            graph.addCrossGraph(crossGraph);
            
            GlobalEvaluation global = new GlobalEvaluation(crossGraph);
            Dictionary<PairNN, double> tempwGlobals = global.Run();
            Dictionary<PairNN, double> wGlobals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            foreach (KeyValuePair<PairNN, double> item in tempwGlobals) {
                wGlobals[item.Key] = item.Value;
            }
            //Console.WriteLine("Global Done!");

            EvacuationRouteSelector selector = new EvacuationRouteSelector();
            selector.initialize(graph, wGlobals);
            //selector.selectionPath();

            UpdateEvacuationWithCache updateRoute = new UpdateEvacuationWithCache(target, 3, 1000);
            List<int> srcInds = new List<int>() {16, 16, 16, 33, 33, 50, 50, 75, 87, 138, 115, 138};
            List<int> dstInds = new List<int>() {33, 50, 75, 75, 101, 64, 115, 169, 101, 101, 169, 169};
            selector.selectionPath();
            updateRoute.Run(50, srcInds, dstInds);
            selector.selectionPath();
            crossGraph.buildGraph();
            Console.WriteLine("Algorithm Done!");


        }


    }
}