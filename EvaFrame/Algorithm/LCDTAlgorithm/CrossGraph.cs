using EvaFrame.Models.Building;
using System.Collections.Generic;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class Node {

    }

    public class Edge {
        private Indicator from, to;
        public Edge(){}
        public Edge(Indicator from, Indicator to) {
            this.from = from;
            this.to = to;
        }



    }
    public class CrossGraph {
        private Building building;
        List<Edge> edges;

        public List<Edge> Edges{
            get {return edges;}
        }

        public CrossGraph() {}
        public CrossGraph(Building building) {
            this.building = building;
        }

        public void buildGraph() {
            foreach (Indicator exit in building.Exits) {
                foreach (Indicator stairNode in building.Floors[0].Stairs) {
                    edges.Add(new Edge(exit, stairNode));
                }
            }

            foreach (Floor floor in building.Floors) {
                foreach (Indicator stairNode in floor.Stairs) {
                    foreach (Corridor cor in stairNode.Neighbors)
                    if (cor.To.IsStairNode) {

                    }
                }
            }
        }
    }
}