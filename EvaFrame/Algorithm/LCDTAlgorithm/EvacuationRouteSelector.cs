using System;
using System.Collections.Generic;

using EvaFrame.Algorithm.LCDTAlgorithm.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm
{
    /// <summary>
    /// Đưa ra hướng đi an toàn và nhanh nhất có thể cho các Indicator.
    /// </summary>
    class EvacuationRouteSelector
    {
        private Graph graph;
        private Dictionary<PairNN, double> wGlobals;

        /// <summary>
        /// Khởi tạo bộ chọn tuyến đường.
        /// </summary>
        public EvacuationRouteSelector()
        {
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
        public void initialize(Graph graph,
                                Dictionary<PairNN, double> wGlobals)
        {
            this.graph = graph;
            foreach (KeyValuePair<PairNN, double> item in wGlobals)
            {
                this.wGlobals[item.Key] = item.Value;
            }
        }

        /// <summary>
        /// Chọn cạnh tiếp theo mà indicator tương ứng sẽ chỉ tới.
        /// </summary>
        public void selectionPath()
        {
            Building building = graph.CrossGraph.Target;

            for (int i = 0; i < graph.SubGraphs.Count; ++i)
            {
                SubGraph subGraph = graph.SubGraphs[i];
                foreach (Node u in subGraph.Nodes)
                {
                    if (u.IsExitNode)
                        continue;

                    double minPath = Double.PositiveInfinity;

                    foreach (NodeOption option in u.NextOptions)
                    {
                        double weightToS = option.WeightToS;
                        Node stairNode = option.StairNode;
                        foreach (Node exitNode in graph.ExitNodes)
                        {
                            PairNN stairNodeToExitNode = new PairNN(stairNode, exitNode);
                            double newPath = weightToS + wGlobals[stairNodeToExitNode];
                            if (minPath >= newPath)
                            {
                                minPath = newPath;
                                u.Next = option.Next;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Thêm phương án chỉ hướng đi xuống tầng dưới và đi lên tầng trên cho các Stair Node.
        /// </summary>
        public void updateStairWay()
        {
            for (int i = 1; i < graph.CrossGraph.Target.Floors.Count; ++i)
            {
                Floor floor = graph.CrossGraph.Target.Floors[i];
                SubGraph upSubGraph = graph.SubGraphs[i];
                SubGraph downSubGraph = graph.SubGraphs[i - 1];

                foreach (Indicator fromInd in floor.Stairs)
                {
                    foreach (Corridor cor in fromInd.Neighbors)
                    {
                        if (!cor.IsStairway)
                            continue;
                        Indicator toInd = cor.To(fromInd);
                        if (toInd.FloorId > fromInd.FloorId)
                            continue;
                        Node fromNode = upSubGraph.Nodes.Find(node => node.CorresspodingIndicator == fromInd);
                        Node toNode = downSubGraph.Nodes.Find(node => node.CorresspodingIndicator == toInd);
                        fromNode.NextOptions.Add(new NodeOption(new Edge(cor), cor.LCDTWeight(), toNode));
                        toNode.NextOptions.Add(new NodeOption(new Edge(cor), cor.LCDTWeight(), fromNode));
                    }
                }
            }
        }
    }
}