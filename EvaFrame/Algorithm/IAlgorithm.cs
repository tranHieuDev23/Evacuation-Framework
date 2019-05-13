using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm
{
    /// <summary>
    /// Interface cho các thuật toán tìm đường thoát hiểm trong mô hình LCDT.
    /// </summary>
    interface IAlgorithm
    {
        /// <summary>
        /// Khởi tạo thuật toán trên một tòa nhà cụ thể. Trong cài đặt cụ thể, lập trình viên có thể
        /// xây dựng mô hình tính toán của thuật toán tại đây (VD: cross-graph với trọng số w trong
        /// thuật toán LCDT-GV) hoặc để trống nếu không cần thiết (VD: thuật toán Dijikstra bình thường).
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        void Initialize(Building target);

        /// <summary>
        /// Chạy một lần thuật toán trên tòa nhà đã khởi tạo. Hàm này được gọi sau khi đã gọi hàm 
        /// Initialize().
        /// </summary>
        void Run();
    }
}