﻿using System;
using System.Collections.Generic;
using EvaFrame.Algorithm;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;

namespace EvaFrame.Algorithm.NewAlgo
{ 
    public class MainAlgo : IAlgorithm
    {
        private Graph target;

        void IAlgorithm.Initialize(Building target)
        {
            
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

        void IAlgorithm.Run()
        {
            Utility utility = new Utility();
            //Các cấu trúc dữ liệu cần cho thuật toán
            MinHeap<Data> heap = new MinHeap<Data>();

            heap.Push(new Data(target.Root, target.Root.weight));

            while (heap.Count > 0)
            {
                Data data = heap.Top();
                heap.Pop();

                Node u = data.node;
                double wu = data.weightToRoot;

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

        }
    }
}