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
        Simulator simulator = new Simulator(null, new MyAlgorithm(), new MyHazard());
        double result = simulator.RunSimulator(0.2, 0.2);
        Console.Write(result);
    }
}
