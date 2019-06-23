using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        /// <value>Đối tượng tòa nhà đang được giả lập.</value>
        public Building Target { get => target; }

        private IAlgorithm algorithm;
        /// <value>Thuật toán đang được giả lập.</value>
        public IAlgorithm Algorithm { get => algorithm; }

        private IHazard hazard;
        /// <value>Thảm họa đang được giả lập.</value>
        public IHazard Hazard { get => hazard; }

        private IVisualization visualization;
        /// <value>Đối tượng biểu diễn tình trạng tòa nhà được sử dụng.</value>
        public IVisualization Visualization { get => visualization; }

        private double timeElapsed;
        /// <value>Quãng thời gian đã trôi qua, tính từ lúc bắt đầu quá trình giả lập.</value>
        public double TimeElapsed { get => timeElapsed; }

        private List<ICallback> callbacks;

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
            this.callbacks = new List<ICallback>();
            this.timeElapsed = 0;
        }

        /// <summary>
        /// Thêm một đối tượng <c>Callback</c> vào để theo dõi quá trình giả lập.
        /// </summary>
        /// <param name="callback"></param>
        public void AddCallback(ICallback callback)
        {
            callback.Initialize(this);
            callbacks.Add(callback);
        }

        /// <summary>
        /// Chạy mô phỏng thuật toán cho tới khi toàn bộ cư dân trong tòa nhà đã di tản hết.
        /// </summary>
        /// <remarks>
        /// Do hàm này có tính blocking, người dùng có thể sử dụng hàm này khi không cần tới luồng
        /// chính của chương trình vào việc khác (ví dụ như khi giao diện command line).
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
        /// Do hàm này non-blocking, người dùng có thể sử dụng khi cần dùng tới luồng chính
        /// của chương trình vào việc khác (ví dụ như khi sử dụng giao diện đồ họa).
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
            List<Task> onStartTasks = new List<Task>();
            foreach (ICallback c in callbacks)
                onStartTasks.Add(Task.Run(c.OnSimulationStart));
            foreach (Task t in onStartTasks)
                t.Wait();
            onStartTasks.Clear();

            double situationWait = 0;
            double algorithmWait = 0;
            DateTime simulationLast = DateTime.Now;
            List<Task> onSituationUpdatedTasks = new List<Task>();
            List<Task> onAlgorithmUpdatedTasks = new List<Task>();
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
                    foreach (Task t in onSituationUpdatedTasks)
                        t.Wait();
                    onSituationUpdatedTasks.Clear();

                    double updatePeriod = situationUpdatePeriod;
                    hazard.Update(updatePeriod);
                    target.MoveInhabitants(updatePeriod);

                    onSituationUpdatedTasks.Add(Task.Run(() => visualization.Update(timeElapsed)));
                    foreach (ICallback c in callbacks)
                        onSituationUpdatedTasks.Add(Task.Run(c.OnSituationUpdated));

                    situationWait = situationUpdatePeriod;
                }

                if (target.Inhabitants.Count == 0)
                    break;

                if (algorithmWait <= 0)
                {
                    foreach (Task t in onAlgorithmUpdatedTasks)
                        t.Wait();
                    onAlgorithmUpdatedTasks.Clear();

                    algorithm.Run();

                    foreach (ICallback c in callbacks)
                        onAlgorithmUpdatedTasks.Add(Task.Run(c.OnAlgorithmUpdated));

                    algorithmWait = algorithmUpdatePeriod;
                }
            }

            foreach (Task t in onSituationUpdatedTasks)
                t.Wait();
            onSituationUpdatedTasks.Clear();
            foreach (Task t in onAlgorithmUpdatedTasks)
                t.Wait();
            onAlgorithmUpdatedTasks.Clear();

            List<Task> onEndTasks = new List<Task>();
            foreach (ICallback c in callbacks)
                onEndTasks.Add(Task.Run(c.OnSimulationEnd));
            foreach (Task t in onEndTasks)
                t.Wait();

            Console.WriteLine("Simulation finished! Time: " + timeElapsed + "s");
            return timeElapsed;
        }
    }
}