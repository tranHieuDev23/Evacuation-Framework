using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;

namespace EvaFrame.Visualization.TextVisualization
{
    /// <summary>
    /// Mô tả toà nhà bằng việc in ra một số thông tin đơn giản của tòa nhà trên giao diện command line.
    /// </summary>
    public class TextVisualization : IVisualization
    {
        private Building target;
        private Dictionary<Indicator, DijikstraAlgorithm.Data> distanceData;

        void IVisualization.Initialize(Building target)
        {
            this.target = target;
            this.distanceData = new DijikstraAlgorithm(new LengthOnlyFunction()).Run(target);
        }

        void IVisualization.Update(double timeElapsed)
        {   
            Console.WriteLine("-------------------------");
            Console.WriteLine(String.Format("Time elapsed: {0, 0:F5}s", timeElapsed));
            Console.WriteLine(String.Format("Remaining inhabitants: {0, 0:D}", target.Inhabitants.Count));
            Console.WriteLine("Information about the inhabitant closest to an exit:");
            PrintInhabitantData(FindInhabitantClosestToExit());
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
                    distance = distanceData[p.Location.To(p.Following)].DistanceToExit + p.Location.Length * (1 - p.CompletedPercentage);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    result = p;
                }
            }

            Console.WriteLine("Shortest distance from an inhabitant to an exit: " + bestDistance);
            return result;
        }

        private void PrintInhabitantData(Person inhabitant)
        {
            Console.WriteLine("Inhabitant Id: " + inhabitant.Id);
            Console.WriteLine("Inhabitant SpeedMax: " + inhabitant.SpeedMax);
            Console.WriteLine("Inhabitant is taking direction from indicator: " + inhabitant.Following.Id);
            if (inhabitant.Location == null)
            {
                Console.WriteLine("Inhabitant is currently not running on any corridor.");
                return;
            }
            Console.WriteLine("Inhabitant is currently running on corridor: " + inhabitant.Location.Id);
            Console.WriteLine("Corridor length: " + inhabitant.Location.Length);
            Console.WriteLine("Corridor width: " + inhabitant.Location.Width);
            Console.WriteLine("Corridor capacity: " + inhabitant.Location.Capacity);
            Console.WriteLine("Corridor density: " + inhabitant.Location.Density);
            Console.WriteLine("Corridor trustiness: " + inhabitant.Location.Trustiness);
            Console.WriteLine("Inhabitant completedPercentage: " + inhabitant.CompletedPercentage);
            Console.WriteLine("Inhabitant actualSpeed: " + inhabitant.CalculateActualSpeed(inhabitant.Location));
        }
    }
}