using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EvaFrame.Models.Building;
using EvaFrame.Simulation;

namespace EvaFrame.Utilities.Callbacks
{
    /// <summary>
    /// <c>Callback</c> theo dõi quá trình chạy thuật toán, tính toán giá trị của các hàm số đánh giá hiện trạng tòa
    /// nhà trong quá trình chạy, và in ra kết quả dưới định dạng CSV khi quá trình giả lập kết thúc.
    /// </summary>
    public class FunctionTrackingCallback : ICallback
    {
        /// <summary>
        /// Interface hàm số đánh giá dựa trên tòa nhà.
        /// </summary>
        public interface IFunction
        {
            /// <value>Tên của hàm số. Giá trị read-only.</value>
            string Name { get; }

            /// <summary>
            /// Tính toán giá trị của hàm số.
            /// </summary>
            /// <param name="target">Tòa nhà mục tiêu.</param>
            /// <returns></returns>
            double Calculate(Building target);
        }


        private string resultFilepath;
        private List<IFunction> trackingFunction;
        private List<double> lastResults;
        private Dictionary<double, double[]> results;

        private Simulator simulator;

        /// <summary>
        /// Khởi tạo một đối tượng <c>FunctionTrackingCallback</c> mới.
        /// </summary>
        /// <param name="resultFilepath">Đường dẫn tới file kết quả in ra khi kết thúc quá trình chạy giả lập.</param>
        public FunctionTrackingCallback(string resultFilepath)
        {
            this.resultFilepath = resultFilepath;
            this.trackingFunction = new List<IFunction>();
            this.lastResults = new List<double>();
            this.results = new Dictionary<double, double[]>();
        }

        /// <summary>
        /// Thêm hàm số để theo dõi.
        /// </summary>
        /// <param name="function">Hàm số cần theo dõi.</param>
        public void AddFunction(IFunction function) { trackingFunction.Add(function); }


        void ICallback.Initialize(Simulator simulator)
        {
            this.simulator = simulator;
        }

        void ICallback.OnSimulationStart()
        {
            foreach (IFunction f in trackingFunction)
                lastResults.Add(f.Calculate(simulator.Target));
            results[0.0] = lastResults.ToArray();
        }

        void ICallback.OnSituationUpdated()
        {
            double timeElapsed = simulator.TimeElapsed;
            bool changeHappened = false;

            Task[] calculationTasks = new Task[trackingFunction.Count];
            for (int i = 0; i < trackingFunction.Count; i++)
            {
                int id = i;
                calculationTasks[id] = Task.Run(() =>
                {
                    IFunction f = trackingFunction[id];
                    double result = f.Calculate(simulator.Target);
                    if (result != lastResults[id])
                    {
                        changeHappened = true;
                        lastResults[id] = result;
                    }
                });
            }
            foreach (Task t in calculationTasks)
                t.Wait();

            if (!changeHappened)
                return;
            results[timeElapsed] = lastResults.ToArray();
        }

        void ICallback.OnAlgorithmUpdated() { }

        void ICallback.OnSimulationEnd()
        {
            using (StreamWriter file = new StreamWriter(resultFilepath))
            {
                string line = "Time Elapsed";
                foreach (IFunction f in trackingFunction)
                    line += '\t' + f.Name;
                file.WriteLine(line);

                foreach (KeyValuePair<double, double[]> entry in results)
                {
                    line = entry.Key.ToString();
                    foreach (double result in entry.Value)
                        line += '\t' + result.ToString();
                    file.WriteLine(line);
                }
            }
        }
    }
}