using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    public class Edge {
        private Corridor correspondingCorridor;
        public Corridor CorrespondingCorridor { get{ return correspondingCorridor; } }
        private Node from;
        public Node From{ get {return from; } }
        private Node to;
        public Node To{ get { return to; } }
        private double weight;
        public double Weight{ get{ return weight; } }
        public Edge(){}
        public Edge(Node from, Node to, double weight) {
            this.from = from;
            this.to = to;
            this.weight = weight;
        }
        public Edge(Corridor cor) {
            this.correspondingCorridor = cor;
        }
    }
}