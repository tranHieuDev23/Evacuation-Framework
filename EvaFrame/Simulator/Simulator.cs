using System;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm;

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

        /// <summary>
        /// Khởi tạo một đối tượng <c>Simulator</c> để mô phỏng thuật toán.
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        /// <param name="algorithm">Thuật toán được mô phỏng.</param>
        /// <param name="hazard">Thảm họa trong mô phỏng.</param>
        public Simulator(Building target, IAlgorithm algorithm, IHazard hazard)
        {
            this.target = target;
            this.algorithm = algorithm;
            this.hazard = hazard;
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
                double algorithmWait = simulationLast.Subtract(lastSituationUpdate).TotalMilliseconds;

                if (situationWait >= situationUpdatePeriod)
                {
                    hazard.Update(situationUpdatePeriod / 1000);
                    target.MoveInhabitants(situationUpdatePeriod / 1000);
                    lastSituationUpdate = simulationLast;
                    Console.WriteLine("Remaining inhabitants: " + target.Inhabitants.Count);

                    if (target.Inhabitants[0].Location != null)
                    {
                        Console.WriteLine("First inhabitant location: " + target.Inhabitants[0].Location.Id);
                        Console.WriteLine("First inhabitant location length: " + target.Inhabitants[0].Location.Length);
                        Console.WriteLine("First inhabitant location width: " + target.Inhabitants[0].Location.Width);
                        Console.WriteLine("First inhabitant location capacity: " + target.Inhabitants[0].Location.Capacity);
                        Console.WriteLine("First inhabitant location density: " + target.Inhabitants[0].Location.Density);
                        Console.WriteLine("First inhabitant location trustiness: " + target.Inhabitants[0].Location.Trustiness);
                        Console.WriteLine("First inhabitant completedPercentage: " + target.Inhabitants[0].CompletedPercentage);
                        Console.WriteLine("First inhabitant SpeedMax: " + target.Inhabitants[0].SpeedMax);
                        Console.WriteLine("First inhabitant actualSpeed: " + target.Inhabitants[0].CalculateActualSpeed(target.Inhabitants[0].Location));
                    }
                }

                if (algorithmWait >= algorithmUpdatePeriod)
                {
                    algorithm.Run();
                    lastAlgorithmRun = simulationLast;
                }
            }

            return simulationLast.Subtract(simulationStart).TotalSeconds;
        }
    }
}