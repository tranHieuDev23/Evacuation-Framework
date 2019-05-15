using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    class MainAlgo : IAlgorithm {
        private Building target;
        void IAlgorithm.Initialize(Building target) {
            this.target = target;
            
            CrossGraph crossGraph = new CrossGraph(target);
            crossGraph.buildGraph();
        }

        void IAlgorithm.Run() {
            Dictionary<PairII, double> wGlobal = new Dictionary<PairII, double>();

            foreach(Floor floor in target.Floors) {
                LocalEvaluation local = new LocalEvaluation(floor);
                Dictionary<PairII, double> weightInFloor = local.Run();

                foreach(KeyValuePair<PairII, double> item in weightInFloor) {
                    wGlobal[item.Key] = item.Value;
                }
            }

            GlobalEvaluation global = new GlobalEvaluation(wGlobal, target.Exits);
        }


    }
}