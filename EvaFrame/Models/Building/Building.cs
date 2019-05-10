using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class mô tả tòa nhà trong mô hình LCDT. Các thuật toán sẽ thực hiện tính toán trên đối tượng
    /// thuộc class này.
    /// </summary>
    class Building
    {
        private List<Floor> floors;
        /// <value>Danh sách các tầng của tòa nhà. Giá trị read-only.</value>
        public ReadOnlyCollection<Floor> Floors { get { return floors.AsReadOnly(); } }

        private List<Indicator> exits;
        /// <value>Danh sách các Exit Node của tòa nhà. Giá trị read-only.</value>
        public ReadOnlyCollection<Indicator> Exits { get { return exits.AsReadOnly(); } }

        private List<Person> inhabitants;
        /// <value>Danh sách cư dân còn lại trong tòa nhà. Giá trị read-only.</value>
        public ReadOnlyCollection<Person> Inhabitants { get { return inhabitants.AsReadOnly(); } }

        /// <summary>
        /// Xây dựng một đối tượng <c>Building</c> từ file.
        /// </summary>
        /// <param name="filepath">Đường dẫn tới file.</param>
        /// <returns>Đối tượng được xây dựng.</returns>
        public static Building LoadFromFile(string filepath)
        {
            return null;
        }

        /// <summary>
        /// Lưu thông tin của đối tượng hiện tại lên file.
        /// </summary>
        /// <param name="filepath">Đường dẫn tới file.</param>
        public static void SaveToFile(string filepath)
        {

        }

        /// <summary>
        /// Cập nhật sự di chuyển của cư dân trong tòa nhà sau một khoảng thời gian. Nếu cư dân di 
        /// chuyển ra khỏi tòa nhà, đối tượng tương ứng sẽ được loại bỏ ra khỏi danh sách.
        /// <c>Inhabitants</c>.
        /// </summary>
        /// <param name="updatePeriod">Khoảng thời gian di chuyển.</param>
        public void MoveInhabitants(double updatePeriod)
        {
            foreach (Person p in inhabitants)
            {
                double remainingTime = updatePeriod;
                while (true)
                {
                    Corridor position = p.Location;
                    double distanceLeft = position.Length * (1 - p.CompletedPercentage);
                    double speed = calculateSpeed(p, position);
                    // Nếu như người này không kịp di chuyển khỏi hành lang trong khoảng thời gian còn lại.
                    if (speed * remainingTime < distanceLeft) 
                    {
                        distanceLeft -= speed * remainingTime;
                        p.CompletedPercentage = 1.0 - distanceLeft / position.Length;
                        break;
                    }
                    // Nếu như người này kịp di chuyển khỏi hành lang
                    position.Density --;

                    // Nếu như người này tới được Exit Node.
                    if (exits.Contains(position.To))
                    {
                        inhabitants.Remove(p);
                        break;
                    }
                    // Nếu như người này vẫn chưa ra tới Exit Node.
                    remainingTime -= distanceLeft / speed;
                    p.Location = position.To.Next;
                    p.Location.Density ++;
                    p.CompletedPercentage = 0;
                }
            }
        }

        /// <summary>
        /// Tính toán tốc độ di chuyển của cư dân trên một hành lang cụ thể, phụ thuộc vào mật độ người và
        /// tình trang của hành lang đó.
        /// </summary>
        /// <param name="person">Cư dân cần tính toán</param>
        /// <param name="corridor">Hành lang đang di chuyển</param>
        /// <returns></returns>
        private double calculateSpeed(Person person, Corridor corridor)
        {
            return corridor.Trustiness * person.SpeedMax
                * ((corridor.Capacity - corridor.Density + 1) / corridor.Capacity);
        }
    }
}