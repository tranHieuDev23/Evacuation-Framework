using System;
using System.Collections.Generic;

using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    class LCDTAlgorithm : IAlgorithm {
        private Building target;
        /// <summary>
        /// Khởi tạo thuật toán.
        /// </summary>
        /// <param name="target">
        /// Thông số của tòa nhà.
        /// </param>
        void IAlgorithm.Initialize(Building target) {
            this.target = target;
        }
        /// <summary>
        /// Chạy thuật toán LCDT.
        /// </summary>

        void IAlgorithm.Run() {
            Dictionary<PairNN, double> wLocals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            CrossGraph crossGraph = new CrossGraph(target);
            crossGraph.buildGraph();

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

                Console.WriteLine("Floor {0} done!", i);
            }

            
            GlobalEvaluation global = new GlobalEvaluation(crossGraph);
            Dictionary<PairNN, double> wGlobals = global.Run();
            Console.WriteLine("Global Done!");

            EvacuationRouteSelector selector = new EvacuationRouteSelector();
            selector.initialize(crossGraph, wGlobals);
            selector.selectionPath();

        }


    }
}