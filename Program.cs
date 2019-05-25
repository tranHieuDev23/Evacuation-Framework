using System;
using System.Threading;
using Avalonia;
using Avalonia.Logging.Serilog;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Algorithm.NewAlgo;
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
        //Simulator simulator = new Simulator(target, new LCDTAlgorithm(), new BasicConstantHazard(), visualization);
        //Simulator simulator = new Simulator(target, new MainAlgo(), new BasicConstantHazard(), visualization);
        Simulator simulator = new Simulator(target, new PlainDijikstra(), new BasicConstantHazard(), visualization);
        simulator.RunSimulatorAsync(50, 10000);
        app.Run(visualization.MainWindow);
    }
}
