﻿using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{ 
    /// <summary>
    /// Thuật toán tìm đường thoát hiểm trong các tòa nhà lớn theo phương pháp mới, 
    /// áp dụng lên mô hình tòa nhà thông minh để tìm đường đi tối ưu nhất.
    /// </summary>
    public class MainAlgo : IAlgorithm
    {
        private const double PositiveInfinity = 1000000000;
        private Graph target;
        private bool done;

        /// <summary>
        /// Khởi tạo đối tượng thuật toán ban đầu, chưa được gắn với tòa nhà mục tiêu cụ thể nào.
        /// </summary>
        public MainAlgo() { target = null; done = false; }

        /// <summary>
        /// Nhận đầu vào thuật toán là một building và trả về đồ thị dùng cho thuật toán mới
        /// </summary>
        /// <param name="building">Tòa nhà nguồn khởi tạo cho đồ thị</param>
        void IAlgorithm.Initialize(Building building)
        {
            this.target = new Graph(building);
            this.done = false;
        }

        /// <summary>
        /// Định nghĩa kiểu Data cho heap để sử dụng trong Dijkstra
        /// </summary>
        public class Data : IComparable, ICloneable
        {
            ///<value> </value>
            public Node node;

            ///<value> khoảng cách từ <c>node</c> tới đỉnh root</value>
            public double weightToRoot;

            /// <summary>
            /// Khởi tạo một đối tượng <c>Data</c> mới.
            /// </summary>
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
                    node.comingNodes = null;
                    node.nComingPeople = 0;
                    node.weight = PositiveInfinity;
                    foreach (var adjacence in node.adjacences)
                    {
                        adjacence.passingWeight = PositiveInfinity;
                        adjacence.reaching = null;
                    }
                }
            }
            target.Root.reachedNode = target.Root;

        }

        void IAlgorithm.Run()
        {
            if (target == null || done)
                return;
            done = true;

            Utility utility = new Utility();
            //Các cấu trúc dữ liệu cần cho thuật toán
            MinHeap<Data> heap = new MinHeap<Data>();
                
            Setup();
            heap.Push(new Data(target.Root, target.Root.weight));
            /*foreach (var exit in target.Root.adjacences)
            {
                exit.node.nextEdge = exit.node.adjacences.Find(adj => adj.node == target.Root).edge;
                exit.node.next = target.Root;
                exit.node.reachedNode = target.Root;
                exit.node.weight = 0;
                heap.Push(new Data(exit.node, 0));
            }*/
            
            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Node u = data.node;
                double wu = data.weightToRoot;
                //Console.WriteLine(u.nextEdge.CorrespondingCorridor.Length);
                Console.WriteLine(u);

                if (u.label == true) 
                    break;

                if(u.weight != wu) 
                    continue;

                u.label = true;

                Node s = u.reachedNode;
                s.nComingPeople += u.nextEdge.numberPeople;
                s.comingNodes.Add(u);

                utility.UpdateComingNode(s, target.Root, heap);

                foreach (Adjacence v in u.adjacences)
                    if (v.node.label == true)
                        utility.UpdateComingPeople(u, v.edge, target.Root, heap);

                foreach (Adjacence v in u.adjacences)
                    if (v.node.label == false)
                    {
                        s = utility. FindCrossNode(v.node, v.edge);
                        s.nComingPeople += v.edge.numberPeople;
                        double w1 = utility.CalculateWeight(u, s, v.edge.numberPeople);
                        double w2 = utility.CalculateWeight(s, target.Root, s.nComingPeople);
                        double newW = v.edge.weight + w1 + w2;

                        foreach (Adjacence ad in v.node.adjacences)
                            if (ad.node == u)
                            {
                                ad.passingWeight = newW;
                                ad.reaching = s;
                            }

                        if (newW < v.node.weight)
                        {
                            v.node.nextEdge = v.node.adjacences.Find(adj => adj.node == u).edge;
                            v.node.weight = newW;
                            v.node.next = u;
                            v.node.reachedNode = s;
                            heap.Push(new Data(v.node, v.node.weight));
                        }

                        s.nComingPeople -= v.edge.numberPeople;
                    }
            }
            target.UpdateResultToBuilding();
        }
    }
}