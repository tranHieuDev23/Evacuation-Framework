using Avalonia;
using Avalonia.Logging.Serilog;
using EvaFrame.Models.Building;
using EvaFrame.Algorithms;
using EvaFrame.Algorithms.PFAlgorithm;
using EvaFrame.Simulation;
using EvaFrame.Simulation.Hazards;
using EvaFrame.Visualization.WindowVisualization;
using EvaFrame.Utilities.Callbacks;

class Program
{
    class RemainingCountFunction : FunctionTrackingCallback.IFunction
    {
        string FunctionTrackingCallback.IFunction.Name { get => "Remaining Count"; }

        double FunctionTrackingCallback.IFunction.Calculate(Building target)
        {
            return target.Inhabitants.Count;
        }
    }

    class NonEmptyCorridorCountFunction : FunctionTrackingCallback.IFunction
    {
        string FunctionTrackingCallback.IFunction.Name { get => "Non-Empty Corridor Count"; }

        double FunctionTrackingCallback.IFunction.Calculate(Building target)
        {
            int nonEmptyCorridorCount = 0;
            foreach (Floor f in target.Floors)
            {
                foreach (Corridor c in f.Corridors)
                    if (c.Density != 0)
                        nonEmptyCorridorCount++;
                foreach (Corridor c in f.Stairways)
                    if (c.Density != 0)
                        nonEmptyCorridorCount++;
            }
            return nonEmptyCorridorCount;
        }
    }

    class AverageDensityOverCapacityFunction : FunctionTrackingCallback.IFunction
    {
        string FunctionTrackingCallback.IFunction.Name { get => "Average Density Over Capacity"; }

        double FunctionTrackingCallback.IFunction.Calculate(Building target)
        {
            int nonEmptyCorridorCount = 0;
            double result = 0.0;
            foreach (Floor f in target.Floors)
            {
                foreach (Corridor c in f.Corridors)
                    if (c.Density != 0)
                    {
                        nonEmptyCorridorCount++;
                        result += c.Density / c.Capacity;
                    }
                foreach (Corridor c in f.Stairways)
                    if (c.Density != 0)
                    {
                        nonEmptyCorridorCount++;
                        result += c.Density / c.Capacity;
                    }
            }
            result /= nonEmptyCorridorCount;
            return result;
        }
    }

    public static void Main(string[] args)
    {
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToDebug()
            .Start(AppMain, null);
    }

    private static void AppMain(Application app, string[] args)
    {
        Building target = Building.LoadFromFile("data.bld");
        IAlgorithm algorithm = new PFAlgorithm(4, 40);
        IHazard hazard = new RandomNonCriticalHazard(new int[] { 5, 6, 7, 8 }, 0.3, 3);
        WindowVisualization visualization = new WindowVisualization(1280, 720);

        FunctionTrackingCallback fcb = new FunctionTrackingCallback("SimulationData.csv");
        fcb.AddFunction(new RemainingCountFunction());
        fcb.AddFunction(new NonEmptyCorridorCountFunction());
        fcb.AddFunction(new AverageDensityOverCapacityFunction());

        Simulator simulator = new Simulator(target, algorithm, hazard, visualization);
        simulator.AddCallback(fcb);
        
        simulator.RunSimulatorAsync(0.1, 10);
        app.Run(visualization.MainWindow);
    }
}