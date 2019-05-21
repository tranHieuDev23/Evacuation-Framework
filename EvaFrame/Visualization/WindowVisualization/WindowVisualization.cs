using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Visualization;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class WindowVisualization: IVisualization
    {
        private MainWindow mainWindow = new MainWindow();
        public MainWindow MainWindow {get => mainWindow;}

        void IVisualization.Initialize(Building target)
        {

        }

        void IVisualization.Update(double timeElapsed)
        {

        }
    }
}