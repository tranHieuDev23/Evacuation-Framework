using EvaFrame.Simulator;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Simulator.Hazards;
using System;
using EvaFrame.Algorithm.LCDTAlgorithm;

class Program
{
    public static void Main(string[] args)
    {
        Building building = Building.LoadFromFile("data.bld");
        //Simulator simulator = new Simulator(building, new PlainDijikstra(), new BasicConstantHazard());
        Simulator simulator = new Simulator(building, new LCDTAlgorithm(), new BasicConstantHazard());
        double result = simulator.RunSimulator(2000, 10000);
    }
}
