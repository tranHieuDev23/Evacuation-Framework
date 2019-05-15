using System;
using EvaFrame.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Utilities {
    class Data: IComparable, ICloneable {
        public Indicator indicator;
        public double weightToExit;

        public Data(Indicator indicator, double weightToExit)
        {
            this.indicator = indicator;
            this.weightToExit = weightToExit;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj.GetType() != typeof(Data))
                throw new ArgumentException("obj is not the same type as this instance.");
            Data data = obj as Data;
            return weightToExit.CompareTo(data.weightToExit);
        }

        object ICloneable.Clone() { return new Data(indicator, weightToExit); }
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

    public static class CaculatinExtensionMethod {
        public static double calcWeight(this Corridor cor) {
            double w = cor.Length / (cor.Trustiness * (cor.Capacity - cor.Density + 1));
            return w;
        }


    }
    
}