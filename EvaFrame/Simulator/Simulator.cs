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
        /// Thời gian giữa hai lần cập nhật tình trạng thảm họa và vị trí của cư dân.
        /// </param>
        /// <param name="algorithmUpdatePeriod">
        /// Thời gian giữa hai lần chạy thuật toán liên tiếp.
        /// </param>
        /// <returns>
        /// Thời gian để toàn bộ cư dân trong tòa nhà di tản hết.
        /// </returns>
        public double RunSimulator(double situationUpdatePeriod, double algorithmUpdatePeriod)
        {
            hazard.Intialize(target);
            algorithm.Intialize(target);
            double result = 0;
            return result;
        }
    }
}