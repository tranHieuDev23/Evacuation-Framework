using System;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class <c>Corridor</c> mô tả hành lang kết nối giữa hai <c>Indicator</c> trong tòa nhà.
    /// </summary>
    class Corridor
    {
        private Indicator from, to;
        /// <value><c>Indicator</c> xuất phát của hành lang. Giá trị read-only.</value>
        public Indicator From { get { return from; } }
        /// <value><c>Indicator</c> kết thúc của hành lang. Giá trị read-only.</value>
        public Indicator To { get { return to; } }

        private double length;
        /// <value>Độ dài của hành lang. Giá trị read-only</value>
        public double Length { get { return length; } }

        private double capacity;
        /// <value>Khả năng thông qua của hành lang. Giá trị read-only.</value>
        public double Capacity { get { return capacity; } }

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
        /// <param name="from"><c>Indicator</c> xuất phát.</param>
        /// <param name="to"><c>Indicator</c> kết thúc.</param>
        /// <param name="length">Độ dài của hành lang.</param>
        /// <param name="capacity">Khả năng thông qua của hành lang.</param>
        /// <param name="density">Mật độ người đi qua ban đầu. Giá trị mặc định bằng 0 (không có ai đi qua).</param>
        /// <param name="trustiness">Độ tin tưởng của con đường ban đầu. Giá trị mặc định bằng 1 (con đường hoàn toàn không bị ảnh hưởng bởi thảm họa).</param>
        public Corridor(Indicator from, Indicator to, double length, double capacity, double density = 0, double trustiness = 1)
        {
            this.from = from;
            this.to = to;
            this.length = length;
            this.capacity = capacity;
            this.density = density;
            this.trustiness = trustiness;
        }
    }
}