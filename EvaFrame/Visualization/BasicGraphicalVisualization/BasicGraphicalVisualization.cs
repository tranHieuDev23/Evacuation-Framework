using System;
using EvaFrame.Models.Building;
using EvaFrame.Visualization;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace EvaFrame.Visualization.BasicGraphicalVisualization
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicGraphicalVisualization : IVisualization
    {
        private Building target;
        private MainWindow mainWindow;

        void IVisualization.Initialize(Building target)
        {
            this.target = target;
            this.mainWindow = new MainWindow(target);
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .Start(AppMain, null);
        }

        void IVisualization.Update(DateTime simulationStart, DateTime simulationLatest)
        {
            mainWindow.Update(simulationStart, simulationLatest);
        }

        private void AppMain(Application app, string[] args)
        {
            app.Run(this.mainWindow);
        }
    }
}