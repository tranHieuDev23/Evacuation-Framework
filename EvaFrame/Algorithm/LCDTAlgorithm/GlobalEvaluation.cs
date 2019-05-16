using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;


public class GlobalEvaluation {
    private CrossGraph crossGraph;

    public GlobalEvaluation() {}
    public GlobalEvaluation(CrossGraph crossGraph) {
        this.crossGraph = crossGraph;
    }

    public Dictionary<PairNN, double> Run() {
        Dictionary<PairNN, double> wGlobal = new Dictionary<PairNN, double>();

        foreach (Node exitNode in crossGraph.Nodes) 
        if (exitNode.IsExitNode == true) {
            Dictionary<PairNN, double> tempWeights = dijkstra(exitNode);

            foreach (KeyValuePair<PairNN, double> item in tempWeights) {
                wGlobal[item.Key] = item.Value;
            }
        }

        return wGlobal;
    }

    public Dictionary<PairNN, double> dijkstra(Node exitNode) {
        Dictionary<PairNN, double> weights = new Dictionary<PairNN, double>(new NodeEqualityComparer());
        MinHeap<Data> heap = new MinHeap<Data>();

        foreach (Node u in crossGraph.Nodes) {
            weights[new PairNN(exitNode, u)] = double.PositiveInfinity;
            u.Next = null;
        }

        weights[new PairNN(exitNode, exitNode)] = 0;
        heap.Push(new Data(exitNode, 0));

        while(heap.Count > 0) {
            Node u = heap.Top().node;
            double wu = heap.Top().weightToExit;

            if (weights[new PairNN(exitNode, u)] != wu) continue;

            foreach (Edge e in u.Adjencents) {
                Node v = e.To;
                double wv = weights[new PairNN(exitNode, v)];

                if (wv > wu + e.Weight) {
                    wv = wu + e.Weight;
                    weights[new PairNN(exitNode, v)] = wv;
                    v.Next = v.Adjencents.Find(edge => edge.To == u);
                    heap.Push(new Data(v, wv));
                }
            }
        }


        return weights;
    }

}