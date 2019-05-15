using EvaFrame.Simulator;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Simulator.Hazards;
using System;

class Program
{
    public static void Main(string[] args)
    {
        Building building = Building.LoadFromFile("data.bld");
        Simulator simulator = new Simulator(building, new PlainDijikstra(), new BasicConstantHazard());
        double result = simulator.RunSimulator(2000, 10000);
    }
}
