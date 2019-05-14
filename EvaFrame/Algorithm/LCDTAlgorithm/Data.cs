using System;
using EvaFrame.Utilities;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.LCDTAlgorithm {
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
}