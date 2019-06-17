using System;
using System.Threading;
using EvaFrame.Models.Building;
using EvaFrame.Algorithms;
using EvaFrame.Visualization;
using EvaFrame.Utilities.Callbacks;

namespace EvaFrame.Simulation
{
    /// <summary>
    /// Class thực hiện mô phỏng thuật toán.
    /// </summary>
    public class Simulator
    {
        private Building target;
        public Building Target { get => target; }

        private IAlgorithm algorithm;
        public IAlgorithm Algorithm { get => algorithm; }

        private IHazard hazard;
        public IHazard Hazard { get => hazard; }

        private IVisualization visualization;
        public IVisualization Visualization { get => visualization; }

        private double timeElapsed;
        public double TimeElapsed { get => timeElapsed; }

        private event EventHandler simulationStart;
        private event EventHandler situationUpdated;
        private event EventHandler algorithmUpdated;
        private event EventHandler simulationEnd;

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

        public void AddCallback(ICallback callback)
        {
            callback.Initialize(this);
            simulationStart += (object o, EventArgs args) => callback.OnSimulationStart();
            situationUpdated += (object o, EventArgs args) => callback.OnSituationUpdate();
            algorithmUpdated += (object o, EventArgs args) => callback.OnAlgorithmUpdate();
            simulationEnd += (object o, EventArgs args) => callback.OnSimulationEnd();
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
        /// <returns>
        /// Đối tượng luồng đang chạy mô phỏng thuật toán.
        /// </returns>
        public Thread RunSimulatorAsync(double situationUpdatePeriod, double algorithmUpdatePeriod)
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

        private double SimulationLoop(double situationUpdatePeriod, double algorithmUpdatePeriod)
        {
            timeElapsed = 0;
            double situationWait = 0;
            double algorithmWait = 0;
            DateTime simulationLast = DateTime.Now;

            simulationStart?.Invoke(this, EventArgs.Empty);
            while (true)
            {
                DateTime now = DateTime.Now;
                double deltaTime = Math.Min(now.Subtract(simulationLast).TotalSeconds,
                                            Math.Min(situationWait, algorithmWait));

                timeElapsed += deltaTime;
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
                            (callback) => visualization.Update(timeElapsed)
                        )
                    );
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            (callback) => situationUpdated?.Invoke(this, EventArgs.Empty)
                        )
                    );

                    situationWait = situationUpdatePeriod;
                }

                if (target.Inhabitants.Count == 0)
                    break;

                if (algorithmWait <= 0)
                {
                    algorithm.Run();
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(
                            (callback) => algorithmUpdated?.Invoke(this, EventArgs.Empty)
                        )
                    );
                    algorithmWait = algorithmUpdatePeriod;
                }
            }

            Console.WriteLine("Simulation finished! Time: " + timeElapsed + "s");
            simulationEnd?.Invoke(this, EventArgs.Empty);
            return timeElapsed;
        }
    }
}