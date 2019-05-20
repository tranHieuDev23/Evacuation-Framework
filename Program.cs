using EvaFrame.Simulator;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Simulator.Hazards;
using EvaFrame.Algorithm.NewAlgo;
using EvaFrame.Visualization.BasicGraphicalVisualization;
using System;

class Program
{
    public static void Main(string[] args)
    {
        Building building = Building.LoadFromFile("data.bld");
        Simulator simulator = new Simulator(building, new MainAlgo(), new NullHazard(), new BasicGraphicalVisualization());
        double result = simulator.RunSimulator(200, 10000);
    }
}
