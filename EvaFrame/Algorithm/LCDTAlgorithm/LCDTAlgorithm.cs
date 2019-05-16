using System;
using System.Collections.Generic;

using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    class LCDTAlgorithm : IAlgorithm {
        private Building target;
        
        void IAlgorithm.Initialize(Building target) {
            this.target = target;
        }

        void IAlgorithm.Run() {
            Dictionary<PairNN, double> wLocals = new Dictionary<PairNN, double>();
            /* foreach (Edge edge in crossGraph.Edges) {
                wGlobal[new PairII(edge.From, edge.To)] = edge.Weight;
            }*/
            CrossGraph crossGraph = new CrossGraph(target);
            crossGraph.buildGraph();

            foreach(Floor floor in target.Floors) {
                SubGraph subGraph = new SubGraph(floor);
                LocalEvaluation local = new LocalEvaluation(subGraph);
                Dictionary<PairNN, double> weightInFloor = local.Run();

                crossGraph.updateGraph(weightInFloor);
                foreach(KeyValuePair<PairNN, double> item in weightInFloor) {
                    wLocals[item.Key] = item.Value;
                }
            }

            
            GlobalEvaluation global = new GlobalEvaluation(crossGraph);
            Dictionary<PairNN, double> wGlobal = global.Run();

            EvacuationRouteSelector selector = new EvacuationRouteSelector(crossGraph, wLocals, wGlobal);


        }


    }
}