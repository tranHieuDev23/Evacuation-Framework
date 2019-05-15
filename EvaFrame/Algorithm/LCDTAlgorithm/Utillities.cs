using System;
using EvaFrame.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Utilities {
    class Data: IComparable, ICloneable {
        public Node node;
        public double weightToExit;

        public Data(Node node, double weightToExit)
        {
            this.node = node;
            this.weightToExit = weightToExit;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj.GetType() != typeof(Data))
                throw new ArgumentException("obj is not the same type as this instance.");
            Data data = obj as Data;
            return weightToExit.CompareTo(data.weightToExit);
        }

        object ICloneable.Clone() { return new Data(node, weightToExit); }
    }

    public class Pair<T, U> {
        public Pair() {
        }

        public Pair(T first, U second) {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    public class PairII {
        public PairII() {}
        public PairII(Indicator first, Indicator second) {
            this.First = first;
            this.Second = second;
        }
        
        public Indicator First{get; set;}
        public Indicator Second{get; set;}
    }

    public class PairNN {
        public PairNN() {}
        public PairNN(Node first, Node second) {
            this.First = first;
            this.Second = second;
        }
        
        public Node First{get; set;}
        public Node Second{get; set;}
    }

    public static class CaculatinExtensionMethod {
        public static double calcWeight(this Corridor cor) {
            double w = cor.Length / (cor.Trustiness * (cor.Capacity - cor.Density + 1));
            return w;
        }


    }
    
}