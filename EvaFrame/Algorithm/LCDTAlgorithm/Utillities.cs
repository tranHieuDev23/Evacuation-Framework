using System;
using System.Linq;
using System.Collections.Generic;

using EvaFrame.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm.Utilities {
    public class Data: IComparable, ICloneable {
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

    public class DataI: IComparable, ICloneable {
        public Indicator node;
        public double weightToExit;

        public DataI(Indicator node, double weightToExit)
        {
            this.node = node;
            this.weightToExit = weightToExit;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj.GetType() != typeof(DataI))
                throw new ArgumentException("obj is not the same type as this instance.");
            DataI data = obj as DataI;
            return weightToExit.CompareTo(data.weightToExit);
        }

        object ICloneable.Clone() { return new DataI(node, weightToExit); }
    }

    public class PairNN {
        private Node first, second;
        public PairNN() {}
        public PairNN(Node first, Node second) {
            this.first = first;
            this.second = second;
        }
        
        public Node First{
            get { return first;}
            set {
                first = value;
            }}
        public Node Second{
            get{ return second; }
            set{
                second = value;
            }}
    }

    public class NodeEqualityComparer: IEqualityComparer<PairNN> {
        public bool Equals(PairNN p1, PairNN p2) {
            if (p1 == null && p2 == null) return true;
            if (p1 == null || p2 == null) return false;
            //if (p1.First == p2.First && p1.Second == p2.Second) return true;
            if (p1.First.CorresspodingIndicator.Equals(p2.First.CorresspodingIndicator) 
                && p1.Second.CorresspodingIndicator.Equals(p2.Second.CorresspodingIndicator)) return true;
            return false;
        }

        public int GetHashCode(PairNN p) {
            if (p.First == null || p.Second == null) return 0;
            int hCode = p.First.CorresspodingIndicator.GetHashCode() ^ p.Second.CorresspodingIndicator.GetHashCode();
            return hCode.GetHashCode();    
        }
    }

    /* public static class CaculatinExtensionMethod {
        public static double calcWeight(this Corridor cor) {
            double w = cor.Length / (cor.Trustiness * (cor.Capacity - cor.Density + 1));
            return w;
        }
    }*/

    public static class ExtensionMethod {

        public static double calcWeight(this Corridor cor) {
            double w = cor.Length / (cor.Trustiness * (cor.Capacity - cor.Density + 1));
            return w;
        }

        public static double CacheWeight(this Corridor cor) {
            return cor.Length / cor.Width;
        }

        public static int getFloorNumber(this Indicator indicator) {
            string[] arr = indicator.Id.Split('@');

            return System.Convert.ToInt32(arr[1]);
        }

        public static Corridor CorClone(this Corridor item) {
            Corridor newItem = new Corridor(item.From, item.To, item.IsStairway, item.Length, item.Width, item.Density, item.Trustiness);
            return newItem;
        }

    }
    
}