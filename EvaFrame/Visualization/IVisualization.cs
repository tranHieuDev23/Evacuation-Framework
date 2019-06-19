using EvaFrame.Models.Building;

namespace EvaFrame.Visualization
{
    /// <summary>
    /// Interface cho các đối tượng mô tả tình trạng tòa nhà (thông qua command line hoặc thông qua giao diện đồ họa).
    /// </summary>
    public interface IVisualization
    {
        /// <summary>
        /// Khởi tạo mô tả trên một tòa nhà cụ thể. 
        /// </summary>
        /// <remarks>
        /// Lập trình viên có thể cài đặt các thao tác chuẩn bị cần thiết tại đây (ví dụ lưu trữ 
        /// dữ liệu khoảng cách từ các <c>Indicator</c> trên tòa nhà tới Exit Node gần nhất, để 
        /// in ra thông tin về cư dân ở gần lối thoát nhất).
        /// </remarks>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        void Initialize(Building target);

        /// <summary>
        /// Cập nhật lại mô tả trên tòa nhà.
        /// </summary>
        /// <remarks>
        /// Hàm này được gọi sau khi đã gọi hàm <c>Initialize()</c>, và đối tượng cần mô tả lại 
        /// tòa nhà đã được chuẩn bị từ trước.
        /// </remarks>
        /// <param name="timeElapsed">Thời gian kể từ lúc bắt đầu mô phỏng thuật toán, tính theo đơn vị s.</param>
        void Update(double timeElapsed);
    }
}