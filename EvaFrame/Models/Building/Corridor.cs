using System;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class <c>Corridor</c> mô tả hành lang kết nối giữa hai <c>Indicator</c> trong tòa nhà.
    /// </summary>
    public class Corridor
    {
        private string id;
        /// <value>String định danh của <c>Corridor</c>, có dạng [<c>I1.Id</c>]-[<c>I2.Id</c>].</value>
        public string Id { get { return id; } }

        private int floorId;
        /// <value>
        /// Chỉ số của tầng mà <c>Corridor</c> này đang ở trên. 
        /// Nếu <c>IsStairWay</c> bằng <c>true</c>, giá trị này không có ý nghĩa gì cả. 
        /// Trong trường hợp ngược lại, giá trị này là số nguyên trong khoảng [1, số tầng].
        /// </value>
        public int FloorId { get { return floorId; } }

        private bool isStairWay;
        /// <value>
        /// Trả lại <c>true</c> nếu như hành lang này là một cầu thang nối giữa hai Stair Node với nhau.
        /// </value>
        public bool IsStairway { get { return isStairWay; } }

        private Indicator i1, i2;
        /// <value><c>Indicator</c> thứ nhất của hành lang. Giá trị read-only.</value>
        public Indicator I1 { get { return i1; } }

        /// <value><c>Indicator</c> thứ hai của hành lang. Giá trị read-only.</value>
        public Indicator I2 { get { return i2; } }

        private double length;
        /// <value>Độ dài của hành lang. Giá trị read-only</value>
        public double Length { get { return length; } }

        private double width;
        /// <value>Độ rộng của hành lang. Giá trị read-only.</value>
        public double Width { get { return width; } }

        /// <value>Khả năng thông qua của hành lang. Giá trị read-only.</value>
        public double Capacity { get { return length * width; } }

        private double density;
        /// <value>
        /// Mật độ người đi qua trên hành lang. Throw <c>ArgumentOutOfRangeException</c> nếu như 
        /// được gán giá trị bằng một số âm.
        /// </value>
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
        /// <value>
        /// Giá trị độ tin tưởng của hành lang. Throw <c>ArgumentOutOfRangeException</c> nếu như 
        /// được gán giá trị bằng một số ngoài khoảng [0, 1].
        /// </value>
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

        /// <summary>
        /// Khởi tạo một hành lang mới.
        /// </summary>
        /// <param name="i1"><c>Indicator</c> thứ nhất.</param>
        /// <param name="i2"><c>Indicator</c> thứ hai.</param>
        /// <param name="isStairWay">Hành lang này có phải là cầu thang nối giữa các tầng với nhau không.</param>
        /// <param name="length">Độ dài của hành lang.</param>
        /// <param name="width">Độ rộng của hành lang.</param>
        /// <param name="density">Mật độ người đi qua ban đầu. Giá trị mặc định bằng 0 (không có ai đi qua).</param>
        /// <param name="trustiness">Độ tin tưởng của con đường ban đầu. Giá trị mặc định bằng 1 (con đường hoàn toàn không bị ảnh hưởng bởi thảm họa).</param>
        public Corridor(Indicator i1, Indicator i2, bool isStairWay, double length, double width, double density = 0, double trustiness = 1)
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

        /// <summary>
        /// Lấy <c>Indicator</c> đi tới trên <c>Corridor</c> này khi đã biết <c>Indicator</c> xuất phát.
        /// </summary>
        /// <param name="from"><c>Indicator</c> xuất phát.</param>
        /// <returns><c>Indicator</c> đi tới.</returns>
        public Indicator To(Indicator from)
        {
            if (from != i1 && from != i2)
                throw new ArgumentException("from is not an indicator of this corridor!", "from");
            return from == i1? i2 : i1;
        }
    }
}