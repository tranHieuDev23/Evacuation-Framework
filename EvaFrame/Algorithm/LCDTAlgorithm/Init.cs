

namespace EvaFrame.Algorithm.LCDTAlgorithm {

    /// <summary>
    /// Các thông số khởi tạo.
    /// </summary>
    public static class Init{
        /// <summary>
        /// Hệ số để tính ScoreKPath.
        /// </summary>
        public static double Alpha = 0.15f;
        /// <summary>
        /// Hệ số tắc nghẽn đám đông (crowd congestion).
        /// </summary>
        public static double Beta = 0.6;
        /// <summary>
        /// Độ tin tưởng nhỏ nhất.
        /// </summary>
        public static double TrustnessThreshold = 0.3f;
    }
    
}