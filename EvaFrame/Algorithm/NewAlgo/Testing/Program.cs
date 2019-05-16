﻿using System;
using System.Collections.Generic;
using System.IO;    
using EvaFrame.Algorithm.NewAlgo;
using EvaFrame.Algorithm.NewAlgo.VirtualGraph;
using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm.NewAlgo.Testing
{
    public class TestUtility
    {
        public List<Indicator> indicators = new List<Indicator>();

        // public List<Corridor> testCorridors = new List<Corridor>();
        public List<Node> nodes = new List<Node>();
        public List<Edge> edges = new List<Edge>();

        public static void Main(String[] args)
        {
            TestUtility test = new TestUtility();
            test.Initialize("test.dat");
            Utility widget = new Utility();
            Console.WriteLine(test.nodes[7].nextEdge.CorrespondingCorridor.Length);
            double density = widget.GetDensity(test.nodes[8].nextEdge, 1);
            double weight = widget.CalculateWeight(test.nodes[8], test.nodes[2], 1);
            Console.WriteLine(weight);
        }

        public void Initialize(String filepath)
        {
            try
            {
                String line;
                StreamReader sr = new StreamReader(filepath);
                int numberIndicator = Int32.Parse(sr.ReadLine());
                for (int i = 0; i < numberIndicator; i++)
                {
                    indicators.Add(new Indicator());
                    nodes.Add(new Node(indicators[i]));
                }
                while ((line = sr.ReadLine()) != null)
                {
                    String[] corridorData = line.Split(',');
                    int from = Int32.Parse(corridorData[0]);
                    int to = Int32.Parse(corridorData[1]);
                    double length = Double.Parse(corridorData[2]);
                    int numberPeople = Int32.Parse(corridorData[3]);
                    Corridor corridorFrom = new Corridor(indicators[from], indicators[to],
                                                     length, 10, numberPeople, 1);
                    Corridor corridorTo = new Corridor(indicators[to], indicators[from],
                                                     length, 10, numberPeople, 1);   
                    Adjacence adjacenceFrom = new Adjacence();
                    Edge edgeFrom = new Edge(corridorFrom, nodes[to]);
                    adjacenceFrom.node = nodes[to];
                    adjacenceFrom.edge = edgeFrom;
                    nodes[from].adjacences.Add(adjacenceFrom); 
                    Adjacence adjacenceTo = new Adjacence();
                    Edge edgeTo = new Edge(corridorTo, nodes[from]); 
                    adjacenceTo.node = nodes[from];
                    adjacenceTo.edge = edgeTo;
                    nodes[to].adjacences.Add(adjacenceTo);
                    nodes[to].next = nodes[from];
                    nodes[to].nextEdge = edgeTo;
                }
            }
            catch (Exception e)
            {
                
                throw new Exception("Exception occurred while reading file!", e);
            }
        }
    }
}