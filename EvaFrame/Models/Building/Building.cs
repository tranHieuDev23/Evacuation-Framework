using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EvaFrame.Models.Building
{
    /// <summary>
    /// Class mô tả tòa nhà trong mô hình LCDT. 
    /// Các thuật toán sẽ thực hiện tính toán trên đối tượng thuộc class này.
    /// </summary>
    public partial class Building
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

        private Building()
        {
            floors = new List<Floor>();
            exits = new List<Indicator>();
            inhabitants = new List<Person>();
        }

        /// <summary>
        /// Xây dựng một đối tượng <c>Building</c> từ file.
        /// </summary>
        /// <param name="filepath">Đường dẫn tới file.</param>
        /// <returns>Đối tượng <c>Building</c> được xây dựng.</returns>
        /// <exception cref="System.Exception">
        /// Throw nếu như xảy ra Exception trong quá trình đọc file.
        /// </exception>
        public static Building LoadFromFile(string filepath)
        {
            try
            {
                Building result = new Building();
                using (StreamReader sr = new StreamReader(filepath))
                {
                    int numFloor = Int32.Parse(sr.ReadLine());
                    for (int floorId = 1; floorId <= numFloor; floorId++)
                        LoadFloor(sr, result, floorId);
                    LoadExitNodes(sr, result);
                    LoadPeople(sr, result);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Exception occurred while reading file!", e);
            }
        }

        /// <summary>
        /// Cập nhật sự di chuyển của cư dân trong tòa nhà sau một khoảng thời gian. 
        /// Nếu cư dân di chuyển ra khỏi tòa nhà, đối tượng tương ứng sẽ được loại bỏ ra khỏi danh sách <c>Inhabitants</c>.
        /// </summary>
        /// <param name="updatePeriod">Khoảng thời gian di chuyển.</param>
        public void MoveInhabitants(double updatePeriod)
        {
            for (int i = 0; i < inhabitants.Count; i++)
            {
                Person p = inhabitants[i];
                if (p.Evacuate(updatePeriod))
                {
                    inhabitants.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}