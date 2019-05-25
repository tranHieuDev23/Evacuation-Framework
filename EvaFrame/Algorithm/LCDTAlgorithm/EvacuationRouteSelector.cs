using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
    /// <summary>
    /// Đưa ra hướng đi an toàn và nhanh nhất có thể cho các Indicator.
    /// </summary>
    public class EvacuationRouteSelector {
        private Graph graph;
        private Dictionary<PairNN, double> wGlobals;
        
        /// <summary>
        /// Khởi tạo bộ chọn tuyến đường.
        /// </summary>
        public EvacuationRouteSelector() {
            this.graph = null;
            this.wGlobals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
        }

        /// <summary>
        /// Khởi tạo bộ chọn tuyến đường.
        /// </summary>
        /// <param name="graph">
        /// Đồ thị giữa các Stair Node với nhau và với Exit Node.
        /// </param>
        /// <param name="wGlobals">
        /// Trọng số giữa Exit Node tới các Stair Node tương ứng với Cross Graph.
        /// </param>
        public void initialize( Graph graph,
                                Dictionary<PairNN, double> wGlobals) {
            this.graph = graph;
            foreach (KeyValuePair<PairNN, double> item in wGlobals) {
                this.wGlobals[item.Key] = item.Value;
            }
        }
        
        /// <summary>
        /// Chọn cạnh tiếp theo mà indicator tương ứng sẽ chỉ tới.
        /// </summary>
        public void selectionPath() {
            Building building = graph.CrossGraph.Target;

            for (int i = 0; i < graph.SubGraphs.Count; ++i) {
                SubGraph subGraph = graph.SubGraphs[i];
                foreach (Node u in subGraph.Nodes) 
                if (!u.IsExitNode) {

                    double minPath = Double.PositiveInfinity;
                    if (i == 0) {
                        foreach (NodeOption option in u.NextOptions) {
                            double weightToS = option.WeightToS;
                            Node stairNode = option.StairNode;
                            if (stairNode.IsStairNode) continue;

                            if (minPath >= weightToS) {
                                minPath = weightToS;
                                u.Next = option.Next;
                            }
                        }
                    }
                    else {
                        foreach (NodeOption option in u.NextOptions) {
                            foreach (Node exitNode in graph.ExitNodes) {
                                double weightToS = option.WeightToS;
                                Node stairNode = option.StairNode;
                                PairNN stairNodeToExitNode = new PairNN(stairNode, exitNode);
                                
                                bool check = wGlobals.ContainsKey(stairNodeToExitNode);

                                if (minPath >= weightToS + wGlobals[stairNodeToExitNode]) {
                                    minPath = weightToS + wGlobals[stairNodeToExitNode];
                                    u.Next = option.Next;
                                }

                            }
                        }
                    }

                }
            }
        } 

        /// <summary>
        /// Thêm phương án chỉ hướng đi xuống tầng dưới và đi lên tầng trên cho các Stair Node.
        /// </summary>
        public void updateStairWay() {
            for (int i = 1; i < graph.CrossGraph.Target.Floors.Count; ++i) {
                Floor floor = graph.CrossGraph.Target.Floors[i];
                SubGraph upSubGraph = graph.SubGraphs[i];
                SubGraph downSubGraph = graph.SubGraphs[i-1];

                foreach (Indicator indicator in floor.Indicators)
                if (indicator.IsStairNode) 
                    foreach (Corridor cor in indicator.Neighbors)
                    if (cor.I1.IsStairNode && cor.I2.IsStairNode) {
                        if (cor.I1.Equals(indicator) && cor.I1.getFloorNumber() > cor.I2.getFloorNumber()) {
                            Node upStairNode = upSubGraph.Nodes.Find( node => node.CorresspodingIndicator == cor.I1);
                            Node downStairNode = downSubGraph.Nodes.Find( node => node.CorresspodingIndicator == cor.I2);

                            upStairNode.NextOptions.Add(new NodeOption(new Edge(cor), cor.LCDTWeight(), downStairNode));
                            downStairNode.NextOptions.Add(new NodeOption(new Edge(cor), cor.LCDTWeight(), upStairNode));
                        }
                    }
            }
        }
    }
}