using System;
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
    public class Building
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
        /// <returns>Đối tượng được xây dựng.</returns>
        public static Building LoadFromFile(string filepath)
        {
            try
            {
                Building result = new Building();
                using (StreamReader sr = new StreamReader(filepath))
                {
                    int numFloor = Int32.Parse(sr.ReadLine());
                    for (int floorId = 0; floorId < numFloor; floorId++)
                        LoadFloor(sr, result, floorId);
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
        /// Cập nhật sự di chuyển của cư dân trong tòa nhà sau một khoảng thời gian. Nếu cư dân di 
        /// chuyển ra khỏi tòa nhà, đối tượng tương ứng sẽ được loại bỏ ra khỏi danh sách.
        /// <c>Inhabitants</c>.
        /// </summary>
        /// <param name="updatePeriod">Khoảng thời gian di chuyển.</param>
        public void MoveInhabitants(double updatePeriod)
        {
            for (int i = 0; i < inhabitants.Count; i ++)
            {
                Person p = inhabitants[i];
                if (p.Evacuate(updatePeriod))
                {
                    inhabitants.RemoveAt(i);
                    i --;
                }
            }
        }

        private static void LoadFloor(StreamReader sr, Building target, int floorId)
        {
            List<Indicator> indicatorList = new List<Indicator>();
            int numInd = Int32.Parse(sr.ReadLine());
            sr.ReadLine(); // TODO: This line contains the coordinates of the Indicators, and can be used in graphic representation
            for (int i = 0; i < numInd; i++)
                indicatorList.Add(new Indicator());

            int numCor = Int32.Parse(sr.ReadLine());
            string[] corridorData = sr.ReadLine().Split(',');
            foreach (string data in corridorData)
            {
                string[] values = data.Split(';');
                int fromId = Int32.Parse(values[0]);
                Indicator from = indicatorList[fromId - 1];
                int toId = Int32.Parse(values[1]);
                Indicator to = indicatorList[toId - 1];
                double length = Double.Parse(values[2]) / 10;
                double width = Double.Parse(values[3]);
                double trustiness = Double.Parse(values[4]);

                from.Neighbors.Add(new Corridor(from, to, length, width, 0, trustiness));
            }

            List<Indicator> stairList = new List<Indicator>();
            int numStair = Int32.Parse(sr.ReadLine());
            string[] stairNodeIds = sr.ReadLine().Split(',');
            foreach (string idString in stairNodeIds)
            {
                int id = Int32.Parse(idString) - 1;
                indicatorList[id].IsStairNode = true;
                stairList.Add(indicatorList[id]);
            }

            if (floorId == 0)
            {
                int numExit = Int32.Parse(sr.ReadLine());
                string[] exitNodeIds = sr.ReadLine().Split(',');
                foreach (string idString in exitNodeIds)
                {
                    int id = Int32.Parse(idString) - 1;
                    indicatorList[id].IsExitNode = true;
                    target.exits.Add(indicatorList[id]);
                }
            }
            else
            {
                int numStairCor = Int32.Parse(sr.ReadLine());
                string[] stairData = sr.ReadLine().Split(',');
                foreach (string data in stairData)
                {
                    string[] values = data.Split(';');
                    int fromId = Int32.Parse(values[0]);
                    Indicator from = indicatorList[fromId - 1];
                    int toId = Int32.Parse(values[1]);
                    Indicator to = target.floors[floorId - 1].Indicators[toId - 1];
                    double length = Double.Parse(values[2]) / 10;
                    double width = Double.Parse(values[3]);
                    double trustiness = Double.Parse(values[4]);

                    from.Neighbors.Add(new Corridor(from, to, length, width, 0, trustiness));
                    to.Neighbors.Add(new Corridor(to, from, length, width, 0, trustiness));
                }
            }
            target.floors.Add(new Floor(indicatorList, stairList));
        }

        private static void LoadPeople(StreamReader sr, Building target)
        {
            int numPeople = Int32.Parse(sr.ReadLine());
            for (int i = 0; i < numPeople; i ++)
            {
                string[] peopleData = sr.ReadLine().Split(';');
                int floorId = Int32.Parse(peopleData[0]);
                double speedMax = Double.Parse(peopleData[1]);
                int followingId = Int32.Parse(peopleData[4]);
                Indicator following = target.floors[floorId - 1].Indicators[followingId - 1];

                target.inhabitants.Add(new Person(speedMax, following));
            }
        }
    }
}