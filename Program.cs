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
using EvaFrame.Algorithm.NewAlgo;

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
<<<<<<< HEAD
<<<<<<< HEAD
        Simulator simulator = new Simulator(target, new MainAlgo(), new BasicConstantHazard(), visualization);
        simulator.RunSimulatorAsync(40, 2000);
=======
        //Simulator simulator = new Simulator(target, new LCDTAlgorithm(), new BasicConstantHazard(), visualization);
        //Simulator simulator = new Simulator(target, new MainAlgo(), new BasicConstantHazard(), visualization);
        Simulator simulator = new Simulator(target, new PlainDijikstra(), new BasicConstantHazard(), visualization);
<<<<<<< HEAD
        simulator.RunSimulatorAsync(200, 20000);
=======
        simulator.RunSimulatorAsync(50, 10000);
>>>>>>> Giang
>>>>>>> khanh
=======
        Simulator simulator = new Simulator(target, new PlainDijikstra(), new BasicConstantHazard(), visualization);
        simulator.RunSimulatorAsync(40, 2000);
>>>>>>> 26e170340bfdaa92df0c8b1c4b06df4ed884a52c
        app.Run(visualization.MainWindow);
    }
}
