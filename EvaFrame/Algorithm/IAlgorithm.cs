using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm
{
    /// <summary>
    /// Interface cho các thuật toán tìm đường thoát hiểm trong mô hình LCDT.
    /// </summary>
    public interface IAlgorithm
    {
        /// <summary>
        /// Khởi tạo thuật toán trên một tòa nhà cụ thể. 
        /// </summary>
        /// <remarks>
        /// Trong cài đặt cụ thể, lập trình viên có thể xây dựng mô hình tính toán của thuật toán tại đây 
        /// (VD: cross-graph với trọng số w trong thuật toán LCDT-GV) hoặc để trống nếu không cần thiết 
        /// (VD: thuật toán Dijikstra bình thường).
        /// </remarks>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        void Initialize(Building target);

        /// <summary>
        /// Chạy một lần thuật toán trên tòa nhà đã khởi tạo. 
        /// Hàm này được gọi sau khi đã gọi hàm <c>Initialize()</c>.
        /// </summary>
        /// <remarks>
        /// Hàm này được gọi sau hàm <c>Initialize()</c>, do đó mục tiêu tính toán của đối tượng triển khai 
        /// cũng phải được xác định từ thời điểm gọi hàm <c>Initialize()</c>.
        /// </remarks>
        void Run();
    }
}