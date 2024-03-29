using System;
using EvaFrame.Models.Building;

namespace EvaFrame.Models
{
    /// <summary>
    /// Class mô tả cư dân trong tòa nhà.
    /// </summary>
    public class Person
    {
        private string id;
        /// <value>
        /// String định danh của cư dân. Giá trị read-only.
        /// </value>
        public string Id { get { return id; } }

        private double speedMax;
        /// <value>
        /// Vận tốc tối đa của cư dân trong môi trường tối ưu (hành lang có độ tin cậy 1, không có người nào khác đi qua). Giá trị read-only.
        /// </value>
        public double SpeedMax { get { return speedMax; } }

        private Indicator following;
        /// <value>
        /// <c>Indicator</c> mà người này đang tuân theo chỉ dẫn. Giá trị read-only.
        /// </value>
        public Indicator Following { get { return following; } }

        private Corridor location;
        /// <value>
        /// Hành lang mà người này đang di chuyển trên. Giá trị read-only.
        /// Bằng <c>null</c> nếu như người này chưa nhận được chỉ dẫn từ <c>Indicator</c>, 
        /// hoặc chỉ dẫn hiện tại của <c>Indicator</c> dẫn tới một hành lang bị tắc, 
        /// không thể thêm người vào được.
        /// </value>
        public Corridor Location { get { return location; } }

        private double completedPercentage;
        /// <value>
        /// Mức độ hoàn thành hành lang hiện tại của người này, nằm trong khoảng [0, 1]. Giá trị read-only.
        /// 0 có nghĩa là là vừa xuất phát tại <c>Indicator</c> From. 
        /// Nếu như <c>Location</c> bằng <c>null</c>, giá trị này không có ý nghĩa.
        /// </value>
        public double CompletedPercentage { get { return completedPercentage; } }

        /// <summary>
        /// Khởi tại một đối tượng cư dân mới.
        /// </summary>
        /// <param name="id">String định danh của cư dân.</param>
        /// <param name="speedMax">Vận tốc tối đa của cư dân trong môi trường tối ưu.</param>
        /// <param name="following"><c>Indicator</c> đầu tiên người này nhận chỉ dẫn.</param>
        public Person(string id, double speedMax, Indicator following)
        {
            this.id = id;
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
        public double CalculateActualSpeed(Corridor corridor)
        {
            double MIN_SPEED = 0.5;
            double result = corridor.Trustiness * speedMax
                * ((corridor.Capacity - corridor.Density + 1) / corridor.Capacity);
            return Math.Max(result, MIN_SPEED);
        }

        private Corridor oldLocation = null;
        /// <summary>
        /// Cập nhật sự di chuyển của người này trong tòa nhà sau một khoảng thời gian. Trả về <c>true</c>
        /// nếu như người này di chuyển tới được một Exit Node trong khoảng thời gian đã cho.
        /// </summary>
        /// <param name="updatePeriod">Khoảng thời gian di chuyển.</param>
        /// <returns>
        /// Trả về <c>true</c> nếu như người này di chuyển tới được một Exit Node trong khoảng thời gian đã cho.
        /// </returns>
        public bool Evacuate(double updatePeriod)
        {
            double remainingTime = updatePeriod;

            while (true)
            {
                // Nếu như người này đang đứng ở Exit Node.
                if (following.IsExitNode)
                {
                    if (oldLocation != null)
                        oldLocation.Density--;
                    return true;
                }

                // Nếu như người này chưa nhận được chỉ dẫn từ Indicator.
                if (location == null)
                {
                    // Nếu như Indicator đang tuân theo chưa có chỉ thị gì.
                    if (following.Next == null)
                        return false;
                    // Hoặc nếu đường đi được chỉ bởi Indicator này đang đầy.
                    if (following.Next.Density + 1 > following.Next.Capacity)
                        return false;
                    // Nhận chỉ dẫn từ Indicator.
                    location = following.Next;
                    location.Density++;
                    // Nếu như người này di chuyển từ Corridor cũ sang 
                    // Corridor mới, giảm mật độ người trên Corridor cũ.
                    if (oldLocation != null)
                        oldLocation.Density--;
                }

                double distanceLeft = location.Length * (1 - completedPercentage);
                double speed = CalculateActualSpeed(location);
                // Nếu như người này không kịp di chuyển khỏi hành lang trong khoảng thời gian còn lại.
                if (speed * remainingTime < distanceLeft)
                {
                    distanceLeft -= speed * remainingTime;
                    completedPercentage = 1.0 - distanceLeft / location.Length;
                    break;
                }

                // Nếu như người này kịp di chuyển khỏi hành lang.
                oldLocation = location;
                remainingTime -= distanceLeft / speed;
                following = location.To(following);
                location = null;
                completedPercentage = 0;
            }
            return false;
        }
    }
}