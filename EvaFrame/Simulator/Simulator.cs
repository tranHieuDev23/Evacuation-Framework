using System;
using System.IO;
using System.Collections.Generic;
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
    public class Simulator
    {
        private class SimulationData
        {
            public readonly double timeElapsed;
            public readonly int remainingCount;
            public readonly int nonEmptyCorridorCount;
            public readonly double averageDensityOverCapacity;

            public SimulationData(double timeElapsed, Building building)
            {
                this.timeElapsed = timeElapsed;
                this.remainingCount = building.Inhabitants.Count;
                this.nonEmptyCorridorCount = 0;
                this.averageDensityOverCapacity = 0;

                foreach (Floor f in building.Floors)
                {
                    foreach (Corridor c in f.Corridors)
                        if (c.Density != 0)
                        {
                            this.nonEmptyCorridorCount++;
                            this.averageDensityOverCapacity += c.Density / c.Capacity;
                        }
                    foreach (Corridor c in f.Stairways)
                        if (c.Density != 0)
                        {
                            this.nonEmptyCorridorCount++;
                            this.averageDensityOverCapacity += c.Density / c.Capacity;
                        }
                }

                this.averageDensityOverCapacity /= this.nonEmptyCorridorCount;
            }
        }

        private Building target;
        private IAlgorithm algorithm;
        private IHazard hazard;
        private IVisualization visualization;

        /// <summary>
        /// Khởi tạo một đối tượng <c>Simulator</c> để mô phỏng thuật toán.
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        /// <param name="algorithm">Thuật toán được mô phỏng.</param>
        /// <param name="hazard">Thảm họa trong mô phỏng.</param>
        /// <param name="visualization"></param>
        /// <exception cref="System.ArgumentException">
        /// Throw nếu như trong các tham số truyền vào có giá trị <c>null</c>.
        /// </exception>
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
        /// <remarks>
        /// <para>
        /// Sau khi mô phỏng thuật toán kết thúc, dữ liệu về quá trình chạy của thuật toán sẽ được 
        /// lưu tại file tại địa chỉ đã chỉ định dưới định dạng csv. Dữ liệu này bao gồm các thời 
        /// điểm số lượng cư dân còn lại trong tòa nhà thay đổi, số lượng cư dân còn lại, số lượng 
        /// cạnh có người và tỉ số Density/Capacity trung bình trên các cạnh đó.
        /// </para>
        /// </remarks>
        /// <param name="situationUpdatePeriod">
        /// Thời gian giữa hai lần cập nhật tình trạng thảm họa và vị trí của cư dân (đơn vị ms).
        /// </param>
        /// <param name="algorithmUpdatePeriod">
        /// Thời gian giữa hai lần chạy thuật toán liên tiếp (đơn vị ms).
        /// </param>
        /// <param name="dataFilepath">
        /// Địa chỉ lưu file dữ liệu về quá trình chạy của thuật toán sau khi kết thúc.
        /// </param>
        /// <returns>
        /// Thời gian để toàn bộ cư dân trong tòa nhà di tản hết (đơn vị s).
        /// </returns>
        public double RunSimulator(long situationUpdatePeriod, long algorithmUpdatePeriod, string dataFilepath = "SimulationData.csv")
        {
            SimulationInitialize();
            return SimulationLoop(situationUpdatePeriod, algorithmUpdatePeriod, dataFilepath);
        }

        /// <summary>
        /// Khởi tạo một luồng mới và chạy mô phỏng thuật toán trên luồng này cho tới khi toàn bộ cư dân trong tòa nhà đã di tản hết.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Do hàm này non-blocking, người dùng có thể sử dụng khi cần dùng tới luồng chính
        /// của chương trình vào việc khác (ví dụ như khi sử dụng giao diện đồ họa).
        /// </para>
        /// <para>
        /// Sau khi mô phỏng thuật toán kết thúc, dữ liệu về quá trình chạy của thuật toán sẽ được 
        /// lưu tại file tại địa chỉ đã chỉ định dưới định dạng csv. Dữ liệu này bao gồm các thời 
        /// điểm số lượng cư dân còn lại trong tòa nhà thay đổi, số lượng cư dân còn lại, số lượng 
        /// cạnh có người và tỉ số Density/Capacity trung bình trên các cạnh đó.
        /// </para>
        /// </remarks>
        /// <param name="situationUpdatePeriod">
        /// Thời gian giữa hai lần cập nhật tình trạng thảm họa và vị trí của cư dân (đơn vị s).
        /// </param>
        /// <param name="algorithmUpdatePeriod">
        /// Thời gian giữa hai lần chạy thuật toán liên tiếp (đơn vị s).
        /// </param>
        /// <param name="dataFilepath">
        /// Địa chỉ lưu file dữ liệu về quá trình chạy của thuật toán sau khi kết thúc.
        /// </param>
        /// <returns>
        /// Đối tượng luồng đang chạy mô phỏng thuật toán.
        /// </returns>
        public Thread RunSimulatorAsync(double situationUpdatePeriod, double algorithmUpdatePeriod, string dataFilepath = "SimulationData.csv")
        {
            SimulationInitialize();
            Thread simulationThread = new Thread(() => SimulationLoop(situationUpdatePeriod, algorithmUpdatePeriod, dataFilepath));
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

        private double SimulationLoop(double situationUpdatePeriod, double algorithmUpdatePeriod, string dataFilepath)
        {
            double result = 0;
            double situationWait = 0;
            double algorithmWait = 0;
            DateTime simulationLast = DateTime.Now;
            List<SimulationData> dataList = new List<SimulationData>();
            SimulationData lastData = null;

            while (true)
            {
                DateTime now = DateTime.Now;
                double deltaTime = Math.Min(now.Subtract(simulationLast).TotalSeconds,
                                            Math.Min(situationWait, algorithmWait));

                result += deltaTime;
                simulationLast = now;

                situationWait -= deltaTime;
                algorithmWait -= deltaTime;

                if (situationWait <= 0)
                {
                    double updatePeriod = situationUpdatePeriod;
                    hazard.Update(updatePeriod);
                    target.MoveInhabitants(updatePeriod);
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            (callback) => visualization.Update(result)
                        )
                    );
                    situationWait = situationUpdatePeriod;
                }

                SimulationData newData = new SimulationData(result, target);
                if (lastData == null || (lastData != null && lastData.remainingCount != newData.remainingCount))
                {
                    dataList.Add(newData);
                    lastData = newData;
                }

                if (target.Inhabitants.Count == 0)
                    break;

                if (algorithmWait <= 0)
                {
                    algorithm.Run();
                    algorithmWait = algorithmUpdatePeriod;
                }
            }

            Console.WriteLine("Simulation finished! Time: " + result + "s");
            printReport(dataList, dataFilepath);
            return result;
        }

        private void printReport(List<SimulationData> dataList, string dataFilepath)
        {
            using (StreamWriter file = new StreamWriter(dataFilepath))
            {
                file.WriteLine("Time Elapsed,Remaining Count,Non-empty Corridor Count,Average Density Over Capacity");
                foreach (SimulationData data in dataList)
                    file.WriteLine(data.timeElapsed + ","
                                   + data.remainingCount + ","
                                   + data.nonEmptyCorridorCount + ","
                                   + data.averageDensityOverCapacity);
            }
        }
    }
}