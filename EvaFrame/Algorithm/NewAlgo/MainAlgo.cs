using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{
    class MainAlgo : IAlgorithm
    {
        private Graph target;
        void IAlgorithm.Initialize(Building target)
        {

        }

        private class Data : IComparable, ICloneable
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

        void IAlgorithm.Run()
        {
            //Các cấu trúc dữ liệu cần cho thuật toán
            MinHeap<Data> heap = new MinHeap<Data>();

            //Gán label tất cả các đỉnh bằng False;
            //foreach (var u in target.node)
            //{
            //    u.label = false;
            //}

            heap.Push(target.root);

            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Node u = data.node;
                if (u.label == true) break;
                u.label = true;

                Node s = u.ReachedNode;
                s.nComingPeople += u.nextEdge.numberPeople;
                s.ComingNodes.Add(u);

                Utility.UpdateComingNode(s);

                foreach (Adjacence v in u.adjacences)
                    if (v.node.label == true)
                        Utility.UpdateComingPeople(v.egde);

                foreach (Adjacence v in u.adjacences)
                    if (v.node.label == false)
                    {
                        s = Utility. FindCrossNode(v.node, v.edge);
                        s.nComingPeople += v.edge.numberPeople;

                        double w1 = Utility.CalculateWeight(u, s, v.edge.numberPeople);
                        double w2 = Utility.CalculateWeight(s, target.root, s.nComingPeople);
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
                            v.node.ReachedNode = s;
                            heap.Push(v.node);
                        }

                        s.nComingPeople -= v.edge.numberPeople;
                    }

            }

        }
    }
}