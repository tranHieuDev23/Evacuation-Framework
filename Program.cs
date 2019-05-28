using System;
using System.Threading;
using Avalonia;
using Avalonia.Logging.Serilog;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Algorithm.PFAlgorithm;
using EvaFrame.Simulator;
using EvaFrame.Simulator.Hazards;
using EvaFrame.Visualization.WindowVisualization;
using EvaFrame.Algorithm.LCDTAlgorithm;


class Program
{
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
        WindowVisualization visualization = new WindowVisualization();
        Simulator simulator = new Simulator(target, new PFAlgorithm(4, 40), new RandomNonCriticalHazard(new int[] {5, 6, 7, 8}, 0.3, 3), visualization);
        simulator.RunSimulatorAsync(0.1, 10, "SimulationData.csv");
        app.Run(visualization.MainWindow);
    }
}
