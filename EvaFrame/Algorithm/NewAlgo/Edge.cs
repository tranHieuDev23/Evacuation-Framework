using System;
using System.Collections.Generic;

namespace EvaFrame.Algorithm.NewAlgo
{
    class Edge
    {
        internal List<Edge> inComing;
        internal Node from;

        internal Node to;

        internal Node reaching;

        internal double lenght;

        internal double width;

        internal double trustNess;

        internal double density;

        internal int numberPeople;

        internal double weight;

        internal double constW1;

        internal double constW2;

        internal double trivialW1;

        internal double trivialW2;
    }
}