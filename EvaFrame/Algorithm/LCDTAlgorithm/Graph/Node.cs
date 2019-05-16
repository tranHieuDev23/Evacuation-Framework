using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    
    public class NodeOption {
        private Edge next;
        public Edge Next { get { return next; } }
        private double weightToS;
        public double WeightToS { get { return weightToS; } }
        private Node stairNode;
        public Node StairNode { get { return stairNode; } }

        public NodeOption() {}
        public NodeOption(Edge next, double weightToS, Node stairNode) {
            this.next = next;
            this.weightToS = weightToS;
            this.stairNode = stairNode;
        }
    }
    public class Node {
        private Indicator corresspondingIndicator;
        public Indicator CorresspodingIndicator{ get{ return corresspondingIndicator; } }

        public Node(Indicator indicator) {
            this.corresspondingIndicator = indicator;
            this.adjencents = new List<Edge>();
            this.isStairNode = false;
            this.isExitNode = false;
            this.next = null;
            this.nextOptions = new List<NodeOption>();
        }

        private bool isStairNode;
        public bool IsStairNode{ 
            get { return isStairNode; } 
            set { isStairNode = value; }
        }
        private bool isExitNode;
        public bool IsExitNode{ 
            get { return isExitNode; } 
            set { isExitNode = value; }
        }
        
        private List<Edge> adjencents;
        public List<Edge> Adjencents{ get { return adjencents; } }

        private Edge next;
        public Edge Next {
            get { return next; }
            set {
                if (value != null && !adjencents.Contains(value)) 
                    throw new InvalidOperationException("Invalid Edge");
                if (value != null)
                    corresspondingIndicator.Next = value.CorrespondingCorridor;
                next = value;
            }
        }

        private List<NodeOption> nextOptions;
        public List<NodeOption> NextOptions{
            get { return nextOptions; }
        }
    }

}