using System;

using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    class Graph {
        private Building correspondingBuilding;
        private List<SubGraph> subGraphs;
        private List<Node> nodes;

        Graph() {}
        Graph(Building building) {
            this.correspondingBuilding = building;   
            this.subGraphs = new List<SubGraph>();
            this.nodes = new List<Node>();
        } 

        void initialize() {
            foreach(Floor floor in correspondingBuilding.) 
        }
    }
}