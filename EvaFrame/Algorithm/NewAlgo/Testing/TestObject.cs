using System;
using System.IO;
using System.Collections.Generic;
using EvaFrame.Algorithm.NewAlgo;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo.Testing
{
    public class TestObject
    {
        public Utility utility = new Utility();
        public List<Indicator> indicators = new List<Indicator>();
        public List<Corridor> corridors = new List<Corridor>();

        public List<Node> nodes = new List<Node>();
        public List<Edge> edges = new List<Edge>();

        public TestObject()
        {
            
        }

        public void CreatFromFile(String filepath)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                int numberIndicator = Int32.Parse(sr.ReadLine());
                for (int i = 0; i < numberIndicator; i++)
                {
                    indicators.Add(new Indicator());
                    nodes.Add(new Node(indicators[i]));
                }
            }
        }

    }
}