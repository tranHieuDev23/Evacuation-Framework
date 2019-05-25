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
            for (int i = 0; i < target.Floors.Count; ++i) {
                SubGraph subGraph;
                if (i == 0) subGraph = new SubGraph(target.Floors[0], target);
                else subGraph = new SubGraph(target.Floors[i]);
                this.graph.addSubGraph(subGraph);
            }
        }
        /// <summary>
        /// Chạy thuật toán LCDT.
        /// </summary>

        void IAlgorithm.Run() {
            Dictionary<PairNN, double> wLocals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            CrossGraph crossGraph = new CrossGraph(target);
            this.graph.addCrossGraph(crossGraph);

            for (int i = 0; i < target.Floors.Count; ++i) {
                graph.SubGraphs[i].Update();
                LocalEvaluation local = new LocalEvaluation(graph.SubGraphs[i]);
                Dictionary<PairNN, double> weightInFloor = local.Run();

                graph.CrossGraph.updateGraph(weightInFloor);
                foreach(KeyValuePair<PairNN, double> item in weightInFloor) {
                    wLocals[item.Key] = item.Value;
                }
            }
            

            graph.CrossGraph.buildGraph();
            //graph.addCrossGraph(crossGraph);
            
            GlobalEvaluation global = new GlobalEvaluation(graph.CrossGraph);
            Dictionary<PairNN, double> tempwGlobals = global.Run();
            Dictionary<PairNN, double> wGlobals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
            foreach (KeyValuePair<PairNN, double> item in tempwGlobals) {
                wGlobals[item.Key] = item.Value;
            }

            EvacuationRouteSelector selector = new EvacuationRouteSelector();
            selector.initialize(graph, wGlobals);

            UpdateEvacuationWithCache updateRoute = new UpdateEvacuationWithCache(target, 3, 1000);
            List<int> srcInds = new List<int>() {16, 16, 16, 33, 33, 50, 50, 75, 87, 138, 115, 138};
            List<int> dstInds = new List<int>() {33, 50, 75, 75, 101, 64, 115, 169, 101, 101, 169, 169};
            selector.selectionPath();
            //updateRoute.Run(50, srcInds, dstInds);
            //graph.CrossGraph.buildGraph();
            //selector.selectionPath();
            
            Console.WriteLine("Algorithm Done!");


        }


    }
}