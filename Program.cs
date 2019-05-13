using EvaFrame.Simulator;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm;
using System;

class Program
{
    class MyAlgorithm : IAlgorithm
    {
        void IAlgorithm.Intialize(Building target)
        {

        }

        void IAlgorithm.Run()
        {

        }
    }

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
        Console.WriteLine(building.Floors.Count);
        Console.WriteLine(building.Exits.Count);
        Console.WriteLine(building.Inhabitants.Count);
    }
}
