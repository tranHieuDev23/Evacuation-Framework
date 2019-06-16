using EvaFrame.Simulator;

namespace EvaFrame.Utilities.Callbacks
{
    /// <summary>
    /// Interface của các đối tượng <c>Callback</c> hỗ trợ theo dõi quá trình giả lập thuật toán.
    /// </summary>
    /// <remarks>
    /// Các đối tượng <c>Callback</c> có thể được thêm vào <c>Simulator</c> thông qua hàm <c>Simulator.AddCallback(Callback)</c>.
    /// Khi đó, các hàm của đối tượng <c>Callback</c> sẽ được gọi vào các thời điểm cụ thể, giúp người dùng dễ dàng truy cập, tính toán
    /// và theo dõi tình trạng giả lập của thuật toán.
    /// </remarks>
    public interface ICallback
    {
        /// <summary>
        /// Khởi tạo đối tượng <c>Callback</c> gắn liền với một <c>Simulator</c> cụ thể.
        /// </summary>
        /// <remarks>
        /// Cài đặt cơ bản của hàm này chỉ bao gồm việc gán giá trị cho thuộc tính <c>Simulator</c> của <c>Callback</c>.
        /// Người dùng có thể viết lại hàm này để mở rộng thêm chức năng trong quá trình khởi tạo.
        /// </remarks>
        /// <param name="simulator"></param>
        void Initialize(Simulator.Simulator simulator);

        /// <summary>
        /// Hàm được gọi khi quá trình giả lập bắt đầu.
        /// </summary>
        /// <remarks>
        /// Điểm khác biệt giữa <c>Initialize()</c> và <c>OnSimulationStart()</c> là <c>Initialize()</c> hướng tới mục đích 
        /// khởi tạo đối tượng <c>Callback</c>, trong khi <c>OnSimulationStart()</c> là một bước theo dõi của <c>Callback</c>.
        /// <c>Initialize()</c> được gọi khi khởi tạo giả lập - các đối tượng <c>Building</c>, <c>Algorithm</c> và <c>Hazard</c> 
        /// vẫn chưa được chuẩn bị xong. Trong khi đó, <c>OnSimulationStart()</c> được gọi trước khi bước giả lập đầu tiên được diễn ra, 
        /// khi tất cả các dữ liệu cần theo dõi đều đã được chuẩn bị đầy đủ.
        /// </remarks>
        void OnSimulationStart();

        /// <summary>
        /// Hàm được gọi sau mỗi bước cập nhật tình trạng thảm họa trong tòa nhà.
        /// </summary>
        void OnSituationUpdate();

        /// <summary>
        /// Hàm được gọi sau mỗi bước chạy thuật toán.
        /// </summary>
        void OnAlgorithmUpdate();

        /// <summary>
        /// Hàm được gọi khi quá trình giả lập kết thúc.
        /// </summary>
        void OnSimulationEnd();
    }
}