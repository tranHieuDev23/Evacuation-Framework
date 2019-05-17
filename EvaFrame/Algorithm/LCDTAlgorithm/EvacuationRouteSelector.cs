using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    public class EvacuationRouteSelector {
        private CrossGraph crossGraph;
        private Dictionary<PairNN, double> wGlobals;
        
        /// <summary>
        /// Khởi tạo bộ chọn tuyến đường.
        /// </summary>
        public EvacuationRouteSelector() {
            this.crossGraph = null;
            this.wGlobals = new Dictionary<PairNN, double>(new NodeEqualityComparer());
        }

        /// <summary>
        /// Khởi tạo bộ chọn tuyến đường.
        /// </summary>
        /// <param name="crossGraph">
        /// Đồ thị giữa các Stair Node với nhau và với Exit Node.
        /// </param>
        /// <param name="wGlobals">
        /// Trọng số giữa Exit Node tới các Stair Node tương ứng với Cross Graph.
        /// </param>
        public void initialize( CrossGraph crossGraph,
                                Dictionary<PairNN, double> wGlobals) {
            this.crossGraph = crossGraph;
            this.wGlobals = wGlobals;
        }
        
        /// <summary>
        /// Chọn cạnh tiếp theo mà indicator tương ứng sẽ chỉ tới.
        /// </summary>
        /// <param name="building">
        /// Thông số của toà nhà.
        /// </param>
        public void selectionPath() {
            Building building = crossGraph.Target;

            for (int i = 0; i < building.Floors.Count; ++i) {
                Floor floor = building.Floors[i];
                foreach (Indicator indicator in floor.Indicators) 
                if (!indicator.IsExitNode) {
                    Node u = new Node(indicator);

                    double minPath = Double.PositiveInfinity;
                    if (i == 0) {
                        foreach (NodeOption option in u.NextOptions) {
                            double weightToS = option.WeightToS;
                            Node stairNode = option.StairNode;
                            if (stairNode.IsStairNode) continue;

                            if (minPath > weightToS) {
                                minPath = weightToS;
                                u.Next = option.Next;
                            }
                        }
                    }
                    else {
                        foreach (NodeOption option in u.NextOptions) {
                            foreach (Indicator exit in building.Exits) {
                                Node exitNode = new Node(exit);
                                double weightToS = option.WeightToS;
                                Node stairNode = option.StairNode;
                                PairNN stairNodeToExitNode = new PairNN(stairNode, exitNode);

                                if (minPath > weightToS + wGlobals[stairNodeToExitNode]) {
                                    minPath = weightToS + wGlobals[stairNodeToExitNode];
                                    u.Next = option.Next;
                                }

                            }
                        }
                    }

                }
            }
        } 
    }
}