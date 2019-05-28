using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    class LCDTAlgorithm : IAlgorithm
    {
        private Building target;
        private Graph graph;
        private List<LocalEvaluation> localEvaluators;

        void IAlgorithm.Initialize(Building target)
        {
            this.target = target;
            this.graph = new Graph(target);
            this.localEvaluators = new List<LocalEvaluation>();
            foreach (SubGraph subgraph in this.graph.SubGraphs)
                this.localEvaluators.Add(new LocalEvaluation(subgraph));
        }

        void IAlgorithm.Run()
        {
            graph.CrossGraph = new CrossGraph(target);

            for (int i = 0; i < target.Floors.Count; ++i)
            {
                graph.SubGraphs[i].Update();
                Dictionary<PairNN, double> weightInFloor = localEvaluators[i].Run();
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