using System.Collections.Generic;
using System.Collections.ObjectModel;

using EvaFrame.Algorithm.LCDTAlgorithm;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class SubGraph {
        private List<Node> nodes, stairNodes;
        public ReadOnlyCollection<Node> Nodes{ get { return nodes.AsReadOnly(); } }
        public ReadOnlyCollection<Node> StairNodes{ get { return stairNodes.AsReadOnly(); } }
        private bool isFirstFloor;
        public bool IsFirstFloor { get {return isFirstFloor; } }

        public SubGraph(Floor floor, Building building = null) {
            if (building != null) {
                foreach (Indicator exitNode in building.Exits) {
                    Node node = new Node(exitNode);
                    nodes.Add(node);
                    node.IsExitNode = true;
                    stairNodes.Add(node);
                }

                isFirstFloor = true;
            }
            else isFirstFloor = false;

            foreach(Indicator indicator in floor.Indicators) {
                Node node = new Node(indicator);
                nodes.Add(node);
                
                if (indicator.IsStairNode == true) {
                    node.IsStairNode = true;
                    stairNodes.Add(node);
                }
            } 

            foreach(Node u in nodes) {
                foreach(Corridor cor in u.CorresspodingIndicator.Neighbors) {
                    Node v = nodes.Find(node => node.CorresspodingIndicator == cor.To);
                    u.Adjencents.Add(new Edge(u, v, cor.calcWeight()));
                }
            }

        }

    }
}