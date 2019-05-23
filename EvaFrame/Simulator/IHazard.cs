using EvaFrame.Models.Building;

namespace EvaFrame.Simulator
{
    /// <summary>
    /// Interface cho các đối tượng mô tả thảm họa trong tòa nhà.
    /// </summary>
    interface IHazard
    {
        /// <summary>
        /// Khởi tạo thảm họa trên một tòa nhà cụ thể (VD: Chọn các điểm bắt đầu xảy cháy, tinh chỉnh mức độ
        /// ban đầu của thảm họa).
        /// </summary>
        /// <param name="target">Tòa nhà mục tiêu.</param>
        void Intialize(Building target);

        /// <summary>
        /// Chạy một lần cập nhật tình hình thảm họa trên tòa nhà đã khởi tạo (VD: Duyệt danh sách các đỉnh 
        /// đang xảy cháy, giảm trọng số tin cậy trên các cạnh kề với chúng).
        /// </summary>
        /// <param name="updatePeriod">Quãng thời gian được cập nhật, tính theo đơn vị s.</param>
        void Update(double updatePeriod);
    }
}