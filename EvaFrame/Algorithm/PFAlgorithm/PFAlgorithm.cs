using System;
using System.Collections.Generic;
using System.Threading;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.PFAlgorithm.VirtualGraph;

namespace EvaFrame.Algorithm.PFAlgorithm
{
    /// <summary>
    /// Class thực hiện thuật toán LCDT-PF (Predict Future) - thuật toán cải tiến của nhóm.
    /// </summary>
    public partial class PFAlgorithm : IAlgorithm
    {
        private Graph target;
        private double averageInhabitantSpeed, predictionPeriod;

        /// <summary>
        /// Khởi tạo đối tượng thuật toán <c>PFAlgorithm</c> mới.
        /// </summary>
        /// <param name="averageInhabitantSpeed">Vận tốc trung bình của người dân, sử dụng trong thuật toán dự đoán.</param>
        /// <param name="predictionPeriod">Khoảng thời gian dự đoán trong tương lai.</param>
        public PFAlgorithm(double averageInhabitantSpeed, double predictionPeriod)
        {
            this.averageInhabitantSpeed = averageInhabitantSpeed;
            this.predictionPeriod = predictionPeriod;
        }

        void IAlgorithm.Initialize(Building target)
        {
            this.target = new Graph(target);
        }

        /// <summary>
        /// Đội tượng được đặt vào heap, gồm có đỉnh và trọng số quãng đường
        /// tốt nhất từ đỉnh đó tới root
        /// </summary>
        private class Data : IComparable, ICloneable
        {
            /// <summary>
            /// <c>node</c> tương ứng đối tượng
            /// </summary>
            public Node node;

            /// <summary>
            /// Trọng số quãng đường ngắn nhất tới root từ đỉnh này
            /// </summary>
            public double weightToRoot;
            /// <summary>
            /// Khởi tạo đối tượng với đỉnh và trọng số được truyền vào
            /// </summary>
            /// <param name="node"><c>node tương ứng</c></param>
            /// <param name="weightToRoot">trọng số của quáng đường từ <c>node</c> tới <c>root</c></param>
            public Data(Node node, double weightToRoot)
            {
                this.node = node;
                this.weightToRoot = weightToRoot;
            }

            int IComparable.CompareTo(object obj)
            {
                if (obj.GetType() != typeof(Data))
                    throw new ArgumentException("obj is not the same type as this instance.");
                Data data = obj as Data;
                return weightToRoot.CompareTo(data.weightToRoot);
            }

            object ICloneable.Clone() { return new Data(node, weightToRoot); }
        }

        /// <summary>
        /// Khởi tạo các giá trị thích hợp để thực hiện chạy thuật toán
        /// </summary>
        void Setup()
        {
            foreach (var subGraph in target.FloorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    node.label = false;
                    node.next = null;
                    node.nextEdge = null;
                    node.reachedNode = null;
                    node.comingNodes.Clear();
                    node.nComingPeople = 0;
                    node.weight = double.PositiveInfinity;
                    foreach (var adjacence in node.adjacences)
                    {
                        adjacence.passingWeight = double.PositiveInfinity;
                        adjacence.reaching = null;
                    }
                }
            }
            target.Root.reachedNode = target.Root;
        }

        void IAlgorithm.Run()
        {
            //Các cấu trúc dữ liệu cần cho thuật toán
            MinHeap<Data> heap = new MinHeap<Data>();

            Setup();
            /*Đưa các Exit Node vào heap */
            foreach (var exit in target.Root.adjacences)
            {
                exit.node.nextEdge = exit.node.adjacences.Find(adj => adj.node == target.Root).edge;
                exit.node.next = target.Root;
                exit.node.reachedNode = target.Root;
                exit.node.weight = 0;
                Adjacence adjacence = exit.node.adjacences.Find(adj => adj.node == target.Root);
                adjacence.passingWeight = 0;
                adjacence.reaching = target.Root;
                target.Root.label = true;
                heap.Push(new Data(exit.node, 0));
            }

            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Node u = data.node;
                double wu = data.weightToRoot;

                if (u.label == true)
                    continue;

                if (u.weight != wu)
                    continue;

                u.label = true;

                /*cập nhât thông tin của đỉnh mới được gán nhãn cho đinh nó sẽ tới được */
                Node s = u.reachedNode;

                if (s != target.Root)
                {
                    s.nComingPeople += (int)u.nextEdge.CorrespondingCorridor.Density;
                    s.comingNodes.Add(u);
                    UpdateComingNode(s, target.Root, heap);
                }

                /*---------------------------------------------------------------------- */

                foreach (Adjacence v in u.adjacences)
                {
                    /*Cập nhật lượng người ở giữa 2 đỉnh được gán nhãn cho đỉnh tới được 
                    trong tương lại */

                    if (v.node.label == true && v.node != u.next)
                        UpdateComingPeople(u, v.edge, target.Root, heap);
                }

                /*Tính toán trọng số con đường các đỉnh kề đi qua đỉnh mới được gán nhãn
                và cập nhật lại con đường tốt nhất */
                foreach (Adjacence v in u.adjacences)
                    if (v.node.label == false)
                    {
                        Edge toU = v.node.adjacences.Find(adj => adj.node == u).edge; // Tìm cạnh mà đi từ đỉnh v tới u

                        s = FindCrossNode(v.node, toU);
                        s.nComingPeople += toU.numberPeople;
                        double w1 = CalculateWeight(u, s, toU.numberPeople, 0);
                        double w2 = CalculateWeight(s, target.Root, s.nComingPeople, w1);
                        double newW = v.edge.weight + w1 + w2;

                        foreach (Adjacence ad in v.node.adjacences)
                            if (ad.node == u)
                            {
                                ad.passingWeight = newW;
                                ad.reaching = s;
                            }

                        if (newW < v.node.weight)
                        {
                            v.node.weight = newW;
                            v.node.next = u;
                            v.node.nextEdge = toU;
                            v.node.reachedNode = s;
                            heap.Push(new Data(v.node, v.node.weight));
                        }

                        s.nComingPeople -= v.edge.numberPeople;
                    }
                /*-------------------------------------------------------------------------------------- */
            }

            target.UpdateResultToBuilding();
        }

        /// <summary>
        /// Hàm dùng để cập nhật thường xuyên xử lý tình hướng hỏa hoạn thay đổi nhanh bất thường.
        /// </summary>
        public void CheckCondition()
        {
            Queue<Node> queue = new Queue<Node>();
            foreach (var subGraph in target.FloorGraphs)
            {
                foreach (var node in subGraph.Nodes)
                {
                    node.label = false;
                    if (node.CorrespondingIndicator.IsExitNode)
                    {
                        queue.Enqueue(node);
                    }
                }
            }

            while (queue.Count != 0)
            {
                Node node = queue.Dequeue();
                TackleIncidence(node, target.Root);
                foreach (var adj in node.adjacences)
                {
                    if (adj.node.label) continue;
                    queue.Enqueue(adj.node);
                    adj.node.label = true;
                }
            }
        }
    }
}