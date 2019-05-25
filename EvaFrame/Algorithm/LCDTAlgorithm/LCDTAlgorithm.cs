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
                SubGraph subGraph = new SubGraph(target.Floors[i]);
                this.graph.addSubGraph(subGraph);
            }
        }
        /// <summary>
        /// Chạy thuật toán LCDT.
        /// </summary>

        void IAlgorithm.Run() {
            CrossGraph crossGraph = new CrossGraph(target);
            graph.addCrossGraph(crossGraph);

            for (int i = 0; i < target.Floors.Count; ++i) {
                graph.SubGraphs[i].Update();
                LocalEvaluation local = new LocalEvaluation(graph.SubGraphs[i]);
                Dictionary<PairNN, double> weightInFloor = local.Run();

                graph.CrossGraph.updateGraph(weightInFloor);
            }
            

            graph.CrossGraph.buildGraph();
            
            GlobalEvaluation global = new GlobalEvaluation(graph.CrossGraph);
            Dictionary<PairNN, double> wGlobals = global.Run();

            EvacuationRouteSelector selector = new EvacuationRouteSelector();
            selector.initialize(graph, wGlobals);
            selector.updateStairWay();
            selector.selectionPath();
            
            Console.WriteLine("Algorithm Done!");


        }


    }
}