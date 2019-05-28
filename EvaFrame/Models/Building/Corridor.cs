using System;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Interface mô tả hành lang kết nối giữa hai <c>Indicator</c> trong tòa nhà.
    /// </summary>
    /// <remarks>
    /// Trong sử dụng thực tế, các đối tượng hành lang sẽ được cung cấp thông qua class <c>Building</c> - người dùng không cần phải tự cài đặt interface này.
    /// </remarks>
    public interface Corridor
    {
        /// <value>String định danh của <c>Corridor</c>, có dạng [<c>I1.Id</c>]-[<c>I2.Id</c>]. Giá trị read-only.</value>
        string Id { get; }

        /// <value>
        /// Chỉ số của tầng mà <c>Corridor</c> này đang ở trên. Giá trị read-only.
        /// Nếu <c>IsStairWay</c> bằng <c>true</c>, giá trị này không có ý nghĩa gì cả. 
        /// Trong trường hợp ngược lại, giá trị này là số nguyên trong khoảng [1, số tầng].
        /// </value>
        int FloorId { get; }

        /// <value>
        /// Trả lại <c>true</c> nếu như hành lang này là một cầu thang nối giữa hai Stair Node với nhau. Giá trị read-only.
        /// </value>
        bool IsStairway { get; }

        /// <value><c>Indicator</c> thứ nhất của hành lang. Giá trị read-only.</value>
        Indicator I1 { get; }

        /// <value><c>Indicator</c> thứ hai của hành lang. Giá trị read-only.</value>
        Indicator I2 { get; }

        /// <value>Độ dài của hành lang. Giá trị read-only</value>
        double Length { get; }

        /// <value>Độ rộng của hành lang. Giá trị read-only.</value>
        double Width { get; }

        /// <value>Khả năng thông qua của hành lang. Giá trị read-only.</value>
        double Capacity { get; }

        /// <value>
        /// Mật độ người đi qua trên hành lang.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Throw nếu như được gán giá trị bằng một số âm.
        /// </exception>
        double Density { get; set; }

        /// <value>
        /// Giá trị độ tin tưởng của hành lang.
        /// </value>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Throw nếu như được gán giá trị bằng một số ngoài khoảng [0, 1].
        /// </exception>
        double Trustiness { get; set; }

        /// <summary>
        /// Trả lại <c>Indicator</c> đi tới trên <c>Corridor</c> này khi đã biết <c>Indicator</c> xuất phát.
        /// </summary>
        /// <param name="from"><c>Indicator</c> xuất phát.</param>
        /// <returns><c>Indicator</c> đi tới.</returns>
        /// <exception cref="System.ArgumentException">
        /// Throw nếu như <c>from</c> không phải là một trong hai <c>Indicator</c> của <c>Corridor</c> này.
        /// </exception>
        Indicator To(Indicator from);
    }

    public partial class Building
    {
        // Cài đặt cụ thể của interface Corridor bên trong Building.
        private class CorridorImpl : Corridor
        {
            private string id;
            public string Id { get { return id; } }

            private int floorId;
            public int FloorId { get { return floorId; } }

            private bool isStairWay;
            public bool IsStairway { get { return isStairWay; } }

            private Indicator i1, i2;
            public Indicator I1 { get { return i1; } }
            public Indicator I2 { get { return i2; } }

            private double length;
            public double Length { get { return length; } }

            private double width;
            public double Width { get { return width; } }

            public double Capacity { get { return length * width; } }

            private double density;
            public double Density
            {
                get { return density; }

                set
                {
                    if (value < 0)
                        throw new ArgumentOutOfRangeException("value", "Length must not be negative.");
                    density = value;
                }
            }

            private double trustiness;
            public double Trustiness
            {
                get { return trustiness; }

                set
                {
                    if (value < 0 || value > 1)
                        throw new ArgumentOutOfRangeException("value", "Trustiness must be in the range of [0, 1].");
                    trustiness = value;
                }
            }

            public CorridorImpl(Indicator i1, Indicator i2, bool isStairWay, double length, double width, double density = 0, double trustiness = 1)
            {
                this.id = i1.Id + "-" + i2.Id;
                if (isStairWay)
                    this.floorId = i1.FloorId;
                this.isStairWay = isStairWay;
                this.i1 = i1;
                this.i2 = i2;
                this.length = length;
                this.width = width;
                this.density = density;
                this.trustiness = trustiness;
            }

            public Indicator To(Indicator from)
            {
                if (from != i1 && from != i2)
                    throw new ArgumentException("from is not an indicator of this corridor!", "from");
                return from == i1 ? i2 : i1;
            }
        }
    }
}