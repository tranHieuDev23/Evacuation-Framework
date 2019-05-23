using System;
using System.Collections.Generic;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm;
using EvaFrame.Utilities;
using EvaFrame.Utilities.WeightFunctions;

namespace EvaFrame.Simulator
{
    /// <summary>
    /// Class thực hiện mô phỏng thuật toán.
    /// </summary>
    class Simulator
    {
        private Building target;
        private IAlgorithm algorithm;
        private IHazard hazard;
        private Dictionary<Indicator, DijikstraAlgorithm.Data> distanceData;

        /// <summary>
        /// Khởi tạo một đối tượng <c>Simulator</c> để mô phỏng thuật toán.
        /// Throw <c>ArgumentException</c> nếu như trong các tham số truyền vào có giá trị null.
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        /// <param name="algorithm">Thuật toán được mô phỏng.</param>
        /// <param name="hazard">Thảm họa trong mô phỏng.</param>
        public Simulator(Building target, IAlgorithm algorithm, IHazard hazard)
        {
            if (target == null)
                throw new ArgumentException("target cannot be null!", "target");
            if (algorithm == null)
                throw new ArgumentException("algorithm cannot be null!", "algorithm");
            if (hazard == null)
                throw new ArgumentException("hazard cannot be null!", "hazard");

            this.target = target;
            this.algorithm = algorithm;
            this.hazard = hazard;
            this.distanceData = null;
        }

        /// <summary>
        /// Chạy mô phỏng thuật toán cho tới khi toàn bộ cư dân trong tòa nhà đã di tản hết.
        /// </summary>
        /// <param name="situationUpdatePeriod">
        /// Thời gian giữa hai lần cập nhật tình trạng thảm họa và vị trí của cư dân (đơn vị ms).
        /// </param>
        /// <param name="algorithmUpdatePeriod">
        /// Thời gian giữa hai lần chạy thuật toán liên tiếp (đơn vị ms).
        /// </param>
        /// <returns>
        /// Thời gian để toàn bộ cư dân trong tòa nhà di tản hết (đơn vị s).
        /// </returns>
        public double RunSimulator(long situationUpdatePeriod, long algorithmUpdatePeriod)
        {
            IntializeDistance();
            hazard.Intialize(target);
            algorithm.Initialize(target);

            DateTime simulationStart = DateTime.Now;
            DateTime lastSituationUpdate = DateTime.MinValue;
            DateTime lastAlgorithmRun = DateTime.MinValue;
            DateTime simulationLast;
            while (true)
            {
                simulationLast = DateTime.Now;
                if (target.Inhabitants.Count == 0)
                    break;

                double situationWait = simulationLast.Subtract(lastSituationUpdate).TotalMilliseconds;
                double algorithmWait = simulationLast.Subtract(lastAlgorithmRun).TotalMilliseconds;

                if (situationWait >= situationUpdatePeriod)
                {
                    double updatePeriod = situationWait / 1000;
                    hazard.Update(updatePeriod);
                    target.MoveInhabitants(updatePeriod);
                    lastSituationUpdate = simulationLast;
                    PrintStatus(simulationStart, simulationLast);
                }

                if (algorithmWait >= algorithmUpdatePeriod)
                {
                    algorithm.Run();
                    lastAlgorithmRun = simulationLast;
                }
            }

            return simulationLast.Subtract(simulationStart).TotalSeconds;
        }

        private void IntializeDistance()
        {
            DijikstraAlgorithm da = new DijikstraAlgorithm(new LengthOnlyFunction());
            distanceData = da.Run(target);
        }

        private void PrintStatus(DateTime simulationStart, DateTime simulationLatest)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine(String.Format("Time elapsed: {0, 0:F5}s", simulationLatest.Subtract(simulationStart).TotalSeconds));
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