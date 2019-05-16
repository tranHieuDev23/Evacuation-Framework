using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class EvacuationRouteSelector {
        private CrossGraph crossGraph;
        private Dictionary<PairNN, double> wLocals, wGlobals;

        public EvacuationRouteSelector() {
            this.crossGraph = null;
        }
        public void initialize( CrossGraph crossGraph,
                                Dictionary<PairNN, double> wLocals,
                                Dictionary<PairNN, double> wGlobals) {
            this.crossGraph = crossGraph;
            this.wLocals = wLocals;
            this.wGlobals = wGlobals;
        }

        public void selectionPath(Building building) {
            foreach (Floor floor in building.Floors) {
                foreach (Indicator indicator in floor.Indicators) {
                    Node u = new Node(indicator);

                    double minPath = Double.PositiveInfinity;
                    if (floor.Equals(building.Floors[0])) {
                        foreach (Indicator exit in building.Exits) {
                            Node exitNode = new Node(exit);
                            PairNN uToExitNode = new PairNN(u, exitNode);
                            if (minPath > wLocals[uToExitNode]) {
                                minPath = wLocals[uToExitNode];
                                u.Next = u.NextOptions.Find(option => option.StairNode == exitNode).Next;
                            }
                        }
                    }
                    else {
                        foreach(Indicator exit in building.Exits) {
                            Node exitNode = new Node(exit);

                            foreach (Indicator stair in floor.Stairs) {
                                Node stairNode = new Node(stair);
                                PairNN uToStairNode = new PairNN(u, stairNode);
                                PairNN stairNodeToExitNode = new PairNN(stairNode, exitNode);

                                if (minPath > wLocals[uToStairNode] + wGlobals[stairNodeToExitNode]) {
                                    minPath = wLocals[uToStairNode] + wGlobals[stairNodeToExitNode];
                                    u.Next = u.NextOptions.Find(option => option.StairNode == stairNode).Next;
                                }
                            }
                        }
                    }

                }
            }
        } 
    }
}