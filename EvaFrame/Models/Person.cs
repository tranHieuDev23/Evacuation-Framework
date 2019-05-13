using EvaFrame.Models.Building;

namespace EvaFrame.Models
{
    /// <summary>
    /// Class mô tả cư dân trong tòa nhà.
    /// </summary>
    class Person
    {
        private double speedMax;
        /// <value>
        /// Vận tốc tối đa của cư dân, trong môi trường tối ưu (hành lang có độ tin cậy 1, không có người nào khác đi qua).
        /// </value>
        public double SpeedMax { get { return speedMax; } }

        private Indicator following;
        /// <value>
        /// <c>Indicator</c> mà người này đang tuân theo chỉ dẫn.
        /// </value>
        public Indicator Following
        {
            get { return following; }
            set { following = value; }
        }

        private Corridor location;
        /// <value>
        /// Hành lang mà người này đang di chuyển trên. 
        /// Bằng <c>null</c> vào thời điểm khởi tạo (người này chưa nhận được chỉ dẫn từ <c>Indicator</c> nào cả).
        /// </value>
        public Corridor Location
        {
            get { return location; }
            set { location = value; }
        }

        private double completedPercentage;
        /// <value>
        /// Mức độ hoàn thành hành lang hiện tại của người này, nằm trong khoảng [0, 1]. 
        /// 0 có nghĩa là là vừa xuất phát tại <c>Indicator</c> From. 
        /// Nếu như <c>Location</c> bằng null, giá trị này không có ý nghĩa.
        /// </value>
        public double CompletedPercentage
        {
            get { return completedPercentage; }
            set { completedPercentage = value; }
        }

        /// <summary>
        /// Khởi tại một đối tượng cư dân mới.
        /// </summary>
        /// <param name="speedMax">Vận tốc tối đa của người này trong môi trường tối ưu.</param>
        /// <param name="following">Indicator đầu tiên người này nhận chỉ dẫn.</param>
        public Person(double speedMax, Indicator following)
        {
            this.speedMax = speedMax;
            this.following = following;
            this.location = null;
        }

        /// <summary>
        /// Tính toán tốc độ di chuyển của người này trên một hành lang cụ thể, 
        /// phụ thuộc vào mật độ người và tình trang của hành lang đó.
        /// </summary>
        /// <param name="corridor">Hành lang đang di chuyển</param>
        /// <returns></returns>
        public double calculateActualSpeed(Corridor corridor)
        {
            return corridor.Trustiness * speedMax
                * ((corridor.Capacity - corridor.Density + 1) / corridor.Capacity);
        }

        /// <summary>
        /// Cập nhật sự di chuyển của người này trong tòa nhà sau một khoảng thời gian. Trả về <c>true</c>
        /// nếu như người này di chuyển tới được một Exit Node trong khoảng thời gian đã cho.
        /// </summary>
        /// <param name="updatePeriod">Khoảng thời gian di chuyển.</param>
        /// <returns>
        /// Trả về <c>true</c> nếu như người này di chuyển tới được một Exit Node trong khoảng thời gian đã cho.
        /// </returns>
        public bool evacuate(double updatePeriod)
        {
            // Nếu như người này chưa nhận được chỉ dẫn từ <c>Indicator</c> nào cả.
            if (location == null)
            {
                // Nếu như <c>Indicator</c> đang tuân theo chưa có chỉ thị gì.
                if (following.Next == null)
                    return false;
                location = following.Next;
                completedPercentage = 0;
            }

            double remainingTime = updatePeriod;
            while (true)
            {
                double distanceLeft = location.Length * (1 - completedPercentage);
                double speed = calculateActualSpeed(location);
                // Nếu như người này không kịp di chuyển khỏi hành lang trong khoảng thời gian còn lại.
                if (speed * remainingTime < distanceLeft)
                {
                    distanceLeft -= speed * remainingTime;
                    completedPercentage = 1.0 - distanceLeft / location.Length;
                    break;
                }
                // Nếu như người này kịp di chuyển khỏi hành lang.
                location.Density--;

                // Nếu như người này tới được Exit Node.
                if (location.To.IsExitNode)
                    return true;

                // Nếu như người này vẫn chưa ra tới Exit Node.
                remainingTime -= distanceLeft / speed;
                following = location.To;
                location = following.Next;
                location.Density++;
                completedPercentage = 0;
            }
            return false;
        }
    }
}