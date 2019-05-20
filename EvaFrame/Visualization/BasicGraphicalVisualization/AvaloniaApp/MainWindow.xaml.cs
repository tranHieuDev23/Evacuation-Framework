using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;

namespace EvaFrame.Visualization.BasicGraphicalVisualization
{
    public class MainWindow : Window
    {
        private Building target;
        private Dictionary<Indicator, DijikstraAlgorithm.Data> distanceData;

        private string timeElapsed;
        private string remainingCount;
        private string inhabitantId;
        private string inhabitantSpeedMax;
        private string inhabitantFollowing;
        private string inhabitantLocation;
        private string inhabitantLocationLength;
        private string inhabitantLocationWidth;
        private string inhabitantLocationCapacity;
        private string inhabitantLocationDensity;
        private string inhabitantLocationTrustiness;
        private string inhabitantLocationCompletedPercentage;
        private string inhabitantLocationActualSpeed;

        public MainWindow(Building target)
        {
            this.target = target;
            DijikstraAlgorithm da = new DijikstraAlgorithm(new LengthOnlyFunction());
            distanceData = da.Run(target);
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationStart"></param>
        /// <param name="simulationLatest"></param>
        public void Update(DateTime simulationStart, DateTime simulationLatest)
        {
            timeElapsed = String.Format("Time elapsed: {0, 0:F5}s", simulationLatest.Subtract(simulationStart).TotalSeconds);
            remainingCount = String.Format("Remaining inhabitants: {0, 0:D}", target.Inhabitants.Count);
            
            Person inhabitant = FindInhabitantClosestToExit();
            inhabitantId = "Inhabitant Id: " + inhabitant.Id;
            inhabitantSpeedMax = "Inhabitant SpeedMax: " + inhabitant.SpeedMax.ToString();
            inhabitantFollowing = "Inhabitant is taking direction from indicator: " + inhabitant.Following.Id;
            if (inhabitant.Location == null)
            {
                inhabitantLocation = "Inhabitant is currently not running on any corridor.";
                inhabitantLocationLength =
                inhabitantLocationWidth =
                inhabitantLocationCapacity =
                inhabitantLocationDensity =
                inhabitantLocationTrustiness =
                inhabitantLocationCompletedPercentage =
                inhabitantLocationActualSpeed = "";
            }
            else
            {
                inhabitantLocation = "Inhabitant is currently running on corridor: " + inhabitant.Location.Id;
                inhabitantLocationLength = "Corridor length: " + inhabitant.Location.Length.ToString();
                inhabitantLocationWidth = "Corridor width: " + inhabitant.Location.Width.ToString();
                inhabitantLocationCapacity = "Corridor capacity: " + inhabitant.Location.Capacity.ToString();
                inhabitantLocationDensity = "Corridor density: " + inhabitant.Location.Density.ToString();
                inhabitantLocationTrustiness = "Corridor trustiness: " + inhabitant.Location.Trustiness.ToString();
                inhabitantLocationCompletedPercentage = "Inhabitant completedPercentage: " + inhabitant.CompletedPercentage.ToString();
                inhabitantLocationActualSpeed = "Inhabitant actualSpeed: " + inhabitant.CalculateActualSpeed(inhabitant.Location).ToString();
            }
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

            Console.WriteLine("Shortest distance from an inhabitant to an exit: " + bestDistance);
            return result;
        }
    }
}