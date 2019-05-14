using EvaFrame.Simulator;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using System;

class Program
{
    class MyHazard: IHazard
    {
        void IHazard.Intialize(Building target)
        {

        }

        void IHazard.Update(double updatePeriod)
        {

        }
    }

    public static void Main(string[] args)
    {
        Building building = Building.LoadFromFile("data.bld");
        Simulator simulator = new Simulator(building, new PlainDijikstra(), new MyHazard());
        double result = simulator.RunSimulator(2000, 10000);
    }
}
