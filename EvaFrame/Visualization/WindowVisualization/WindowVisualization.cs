using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using EvaFrame.Visualization;

namespace EvaFrame.Visualization.WindowVisualization
{
    public class WindowVisualization: IVisualization
    {
        private Building target;
        private Dictionary<Indicator, DijikstraAlgorithm.Data> distanceData;
        private MainWindow mainWindow = new MainWindow();
        public MainWindow MainWindow {get => mainWindow;}

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