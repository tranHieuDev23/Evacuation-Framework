using EvaFrame.Simulation;

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
        void Initialize(Simulator simulator);

        /// <summary>
        /// Hàm được gọi khi quá trình giả lập bắt đầu.
        /// </summary>
        /// <remarks>
        /// <param>
        /// Khi quá trình giả lập bắt đầu, hàm <c>OnSimulationStart()</c> của tất cả 
        /// các đối tượng <c>Callback</c> đang theo dõi <c>Simulator</c> sẽ được chạy 
        /// song song, tận dụng khả năng tính toán song song của máy tính.
        /// </param>
        /// <param>
        /// Điểm khác biệt giữa <c>Initialize()</c> và <c>OnSimulationStart()</c> là 
        /// <c>Initialize()</c> hướng tới mục đích khởi tạo đối tượng <c>Callback</c> 
        /// theo dõi một <c>Simulator</c> cụ thể, trong khi <c>OnSimulationStart()</c> 
        /// là một bước theo dõi của <c>Callback</c>. <c>Initialize()</c> được gọi khi 
        /// chưa bắt đầu giả lập - các đối tượng <c>Building</c>, <c>Algorithm</c> và <c>Hazard</c> 
        /// vẫn chưa được chuẩn bị xong. Trong khi đó, <c>OnSimulationStart()</c> được 
        /// gọi trước khi bước giả lập đầu tiên được diễn ra, khi tất cả các dữ liệu cần 
        /// theo dõi đều đã được chuẩn bị đầy đủ.
        /// </param>
        /// </remarks>
        void OnSimulationStart();

        /// <summary>
        /// Hàm được gọi sau mỗi bước cập nhật tình trạng thảm họa trong tòa nhà.
        /// </summary>
        /// <remarks>
        /// <param>
        /// Sau mỗi bước cập nhật tình trạng thảm họa trong tòa nhà, hàm 
        /// <c>OnSituationUpdated()</c> của tất cả các đối tượng <c>Callback</c> 
        /// đang theo dõi <c>Simulator</c> sẽ được chạy song song, tận dụng 
        /// khả năng tính toán song song của máy tính.
        /// </param>
        /// <param>
        /// Trong quá trình chạy hàm này, các giá trị sau của <c>Simulator</c> đang theo 
        /// dõi được bảo đảm giữ nguyên:
        /// <list type="bullet">
        /// <item>
        /// Các thuộc tính của tòa nhà <c>Simulator.Target</c>,  trừ thuộc tính đường 
        /// đang được chỉ thị <c>Next</c> của các <c>Indicator</c> trong tòa nhà.
        /// </item>
        /// <item>
        /// Các thuộc tính của thảm họa <c>Simulator.Hazard</c>.
        /// </item>
        /// </list>
        /// Các giá trị còn lại có thể bị thay đổi song song trong quá trình chạy hàm, 
        /// và không đảm bảo được tính chính xác khi tính toán do race condition. Người 
        /// dùng nên sử dụng hàm này để thực hiện các thao tác đo đạc số liệu trên tòa nhà,
        /// tính toán các hàm đánh giá dựa trên tình trạng vật lý của tòa nhà, vân vân...
        /// </param>
        /// </remarks>
        void OnSituationUpdated();

        /// <summary>
        /// Hàm được gọi sau mỗi bước chạy thuật toán.
        /// </summary>
        /// <remarks>
        /// <param>
        /// Sau mỗi bước chạy thuật toán, hàm  <c>OnAlgorithmUpdated()</c> của tất cả các đối 
        /// tượng <c>Callback</c> đang theo dõi <c>Simulator</c> sẽ được chạy song song, tận dụng 
        /// khả năng tính toán song song của máy tính.
        /// </param>
        /// <param>
        /// Trong quá trình chạy hàm này, các giá trị sau của <c>Simulator</c> đang theo 
        /// dõi được bảo đảm giữ nguyên:
        /// <list type="bullet">
        /// <item>
        /// Thuộc tính đường đang được chỉ thị <c>Next</c> của các <c>Indicator</c> trong tòa nhà
        /// <c>Simulator.Target</c>.
        /// </item>
        /// <item>
        /// Các thuộc tính của thảm họa <c>Simulator.Algorithm</c>.
        /// </item>
        /// </list>
        /// Các giá trị còn lại có thể bị thay đổi song song trong quá trình chạy hàm, 
        /// và không đảm bảo được tính chính xác khi tính toán do race condition. Người 
        /// dùng nên sử dụng hàm này để thực hiện các thao tác đo đạc số liệu của thuật toán,
        /// tính toán các hàm đánh giá dựa trên tình trạng thuật toán, vân vân...
        /// </param>
        /// </remarks>
        void OnAlgorithmUpdated();

        /// <summary>
        /// Hàm được gọi sau khi quá trình giả lập kết thúc.
        /// </summary>
        /// <remarks>
        /// <param>
        /// Khi quá trình giả lập kết thúc, hàm <c>OnSimulationEnd()</c> của tất cả 
        /// các đối tượng <c>Callback</c> đang theo dõi <c>Simulator</c> sẽ được chạy 
        /// song song, tận dụng khả năng tính toán song song của máy tính.
        /// </param>
        /// </remarks>
        void OnSimulationEnd();
    }
}