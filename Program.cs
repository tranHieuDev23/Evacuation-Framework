﻿using System;
using System.Threading;
using Avalonia;
using Avalonia.Logging.Serilog;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm.PlainDijikstra;
using EvaFrame.Simulator;
using EvaFrame.Simulator.Hazards;
using EvaFrame.Visualization.WindowVisualization;

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
        Simulator simulator = new Simulator(target, new PlainDijikstra(), new BasicConstantHazard(), visualization);
        simulator.RunSimulatorAsync(40, 2000);
        app.Run(visualization.MainWindow);
    }
}
