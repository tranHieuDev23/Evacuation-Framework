using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Visualization;
using Avalonia.Controls;

namespace EvaFrame.Visualization.WindowVisualization
{
    /// <summary>
    /// Mô tả tòa nhà bằng giao diện đồ họa đa nền tảng, hỗ trợ bởi thư viên Avalonia.
    /// </summary>
    public class WindowVisualization: IVisualization
    {
        private Building target;
        private Dictionary<Indicator, DijikstraAlgorithm.Data> distanceData;
        private MainWindow mainWindow = new MainWindow();

        /// <value>
        /// Cửa sổ làm việc chính của mô tả. Giá trị này được trả về để sử dụng trong hàm <c>Application.Run()</c>.
        /// </value>
        public Window MainWindow {get => mainWindow;}

        void IVisualization.Initialize(Building target)
        {
            this.target = target;
            this.distanceData = new DijikstraAlgorithm(new LengthOnlyFunction()).Run(target);
        }

        void IVisualization.Update(double timeElapsed)
        {
            mainWindow.UpdateContent(timeElapsed, target.Inhabitants.Count, FindInhabitantClosestToExit());
        }

        private Person FindInhabitantClosestToExit()
        {
            Person result = null;
            double bestDistance = Double.PositiveInfinity;

            foreach (Person p in target.Inhabitants)
            {
                double distance;
                if (p.Location == null)
                    distance = distanceData[p.Following].DistanceToExit;
                else
                    distance = distanceData[p.Location.To].DistanceToExit + p.Location.Length * (1 - p.CompletedPercentage);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    result = p;
                }
            }

            return result;
        }
    }
}