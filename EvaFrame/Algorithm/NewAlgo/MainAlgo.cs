using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{ 
    public class MainAlgo : IAlgorithm
    {
        private const double PositiveInfinity = 1000000000;
        private Graph target;

        void IAlgorithm.Initialize(Building target)
        {
            this.target = new Graph(target);
        }

        public class Data : IComparable, ICloneable
        {
            public Node node;
            public double weightToRoot;

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
            Utility utility = new Utility();
            //Các cấu trúc dữ liệu cần cho thuật toán
            MinHeap<Data> heap = new MinHeap<Data>();
                
            Setup();
            // heap.Push(new Data(target.Root, target.Root.weight));
            foreach (var exit in target.Root.adjacences)
            {
                exit.node.nextEdge = exit.node.adjacences.Find(adj => adj.node == target.Root).edge;
                exit.node.next = target.Root;
                exit.node.reachedNode = target.Root;
                exit.node.weight = 0;
                heap.Push(new Data(exit.node, 0));
            }
            
            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Node u = data.node;
                double wu = data.weightToRoot;
                Console.WriteLine(u.nextEdge.CorrespondingCorridor.Length);

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