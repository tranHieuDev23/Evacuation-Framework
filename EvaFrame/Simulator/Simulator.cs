using System;
using System.Threading;
using EvaFrame.Models;
using EvaFrame.Models.Building;
using EvaFrame.Algorithm;
using EvaFrame.Visualization;

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
        private IVisualization visualization;

        /// <summary>
        /// Khởi tạo một đối tượng <c>Simulator</c> để mô phỏng thuật toán.
        /// Throw <c>ArgumentException</c> nếu như trong các tham số truyền vào có giá trị null.
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        /// <param name="algorithm">Thuật toán được mô phỏng.</param>
        /// <param name="hazard">Thảm họa trong mô phỏng.</param>
        /// <param name="visualization"></param>
        public Simulator(Building target, IAlgorithm algorithm, IHazard hazard, IVisualization visualization)
        {
            if (target == null)
                throw new ArgumentException("target cannot be null!", "target");
            if (algorithm == null)
                throw new ArgumentException("algorithm cannot be null!", "algorithm");
            if (hazard == null)
                throw new ArgumentException("hazard cannot be null!", "hazard");
            if (visualization == null)
                throw new ArgumentException("visualization cannot be null!", "visualization");

            this.target = target;
            this.algorithm = algorithm;
            this.hazard = hazard;
            this.visualization = visualization;
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
            SimulationInitialize();
            return SimulationLoop(situationUpdatePeriod, algorithmUpdatePeriod);
        }

        /// <summary>
        /// Khởi tạo một luồng mới và chạy mô phỏng thuật toán trên luồng này cho tới khi toàn bộ cư dân trong tòa nhà đã di tản hết.
        /// Hàm non-blocking, có thể sử dụng trong một số trường hợp như khi áp dụng giao diện đồ họa của thư viện.
        /// </summary>
        /// <param name="situationUpdatePeriod">
        /// Thời gian giữa hai lần cập nhật tình trạng thảm họa và vị trí của cư dân (đơn vị ms).
        /// </param>
        /// <param name="algorithmUpdatePeriod">
        /// Thời gian giữa hai lần chạy thuật toán liên tiếp (đơn vị ms).
        /// </param>
        /// <returns>
        /// Đối tượng luồng đang chạy mô phỏng thuật toán.
        /// </returns>
        public Thread RunSimulatorAsync(long situationUpdatePeriod, long algorithmUpdatePeriod)
        {
            SimulationInitialize();
            Thread simulationThread = new Thread(() => SimulationLoop(situationUpdatePeriod, algorithmUpdatePeriod));
            simulationThread.IsBackground = true;
            simulationThread.Start();
            return simulationThread;
        }

        private void SimulationInitialize()
        {
            hazard.Intialize(target);
            algorithm.Initialize(target);
            visualization.Initialize(target);
        }

        private double SimulationLoop(long situationUpdatePeriod, long algorithmUpdatePeriod)
        {
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
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            (callback) => visualization.Update(simulationLast.Subtract(simulationStart).TotalSeconds)
                        )
                    );
                    lastSituationUpdate = simulationLast;
                }

                if (algorithmWait >= algorithmUpdatePeriod)
                {
                    algorithm.Run();
                    lastAlgorithmRun = simulationLast;
                }
            }

            double result = simulationLast.Subtract(simulationStart).TotalSeconds;
            Console.WriteLine("Simulation finished! Time: " + result + "s");
            return result;
        }
    }
}