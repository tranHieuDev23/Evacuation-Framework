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
        /// Cập nhật sự di chuyển của cư dân trong tòa nhà sau một khoảng thời gian. Nếu cư dân di 
        /// chuyển ra khỏi tòa nhà, đối tượng tương ứng sẽ được loại bỏ ra khỏi danh sách.
        /// <c>Inhabitants</c>.
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

        private static void LoadFloor(StreamReader sr, Building target, int floorId)
        {
            List<Indicator> indicatorList = LoadIndicatorsInFloor(sr, floorId);
            LoadCorridorsInFloor(sr, indicatorList);
            List<Indicator> stairList = LoadStairNodesInFloor(sr, indicatorList);
            LoadStairCorridorsInFloor(sr, target, indicatorList);
            target.floors.Add(new Floor(indicatorList, stairList));
        }

        private static List<Indicator> LoadIndicatorsInFloor(StreamReader sr, int floorId)
        {
            List<Indicator> indicatorList = new List<Indicator>();
            int numInd = Int32.Parse(sr.ReadLine());
            if (numInd == 0)
                return indicatorList;
            sr.ReadLine(); // TODO: This line contains the coordinates of the Indicators, and can be used in graphic representation
            for (int i = 1; i <= numInd; i++)
                indicatorList.Add(new Indicator(i.ToString() + '@' + floorId.ToString()));
            return indicatorList;
        }

        private static void LoadCorridorsInFloor(StreamReader sr, List<Indicator> indicatorList)
        {
            int numCor = Int32.Parse(sr.ReadLine());
            if (numCor == 0)
                return;
            string[] corridorData = sr.ReadLine().Split(',');
            foreach (string data in corridorData)
            {
                string[] values = data.Split(';');
                Indicator from = IdToIndicator(indicatorList, values[0]);
                Indicator to = IdToIndicator(indicatorList, values[1]);
                double length = Double.Parse(values[2]);
                double width = Double.Parse(values[3]);
                double trustiness = Double.Parse(values[4]);
                from.Neighbors.Add(new Corridor(from, to, length, width, 0, trustiness));
            }
        }

        private static List<Indicator> LoadStairNodesInFloor(StreamReader sr, List<Indicator> indicatorList)
        {
            List<Indicator> stairList = new List<Indicator>();
            int numStair = Int32.Parse(sr.ReadLine());
            if (numStair == 0)
                return stairList;
            string[] stairNodeIds = sr.ReadLine().Split(',');
            foreach (string idString in stairNodeIds)
            {
                Indicator stair = IdToIndicator(indicatorList, idString);
                stair.IsStairNode = true;
                stairList.Add(stair);
            }
            return stairList;
        }

        private static void LoadStairCorridorsInFloor(StreamReader sr, Building target, List<Indicator> indicatorList)
        {
            int numStairCor = Int32.Parse(sr.ReadLine());
            if (numStairCor == 0)
                return;
            string[] stairData = sr.ReadLine().Split(',');
            foreach (string data in stairData)
            {
                string[] values = data.Split(';');
                Indicator from = IdToIndicator(indicatorList, values[0]);
                Indicator to = IdToIndicator(target, values[1]);
                double length = Double.Parse(values[2]);
                double width = Double.Parse(values[3]);
                double trustiness = Double.Parse(values[4]);
                from.Neighbors.Add(new Corridor(from, to, length, width, 0, trustiness));
                to.Neighbors.Add(new Corridor(to, from, length, width, 0, trustiness));
            }
        }

        private static void LoadExitNodes(StreamReader sr, Building target)
        {
            int numExit = Int32.Parse(sr.ReadLine());
            string[] exitNodeIds = sr.ReadLine().Split(',');
            foreach (string idString in exitNodeIds)
            {
                Indicator exit = IdToIndicator(target, idString);
                exit.IsExitNode = true;
                target.exits.Add(exit);
            }
        }

        private static void LoadPeople(StreamReader sr, Building target)
        {
            int numPeople = Int32.Parse(sr.ReadLine());
            for (int i = 0; i < numPeople; i++)
            {
                string[] peopleData = sr.ReadLine().Split(';');
                Indicator following = IdToIndicator(target, peopleData[0]);
                double speedMax = Double.Parse(peopleData[1]);
                target.inhabitants.Add(new Person("P-" + i.ToString(), speedMax, following));
            }
        }

        private static Indicator IdToIndicator(Building target, string idString)
        {
            string[] indicatorData = idString.Split('@');
            int indicatorId = Int32.Parse(indicatorData[0]);
            int floorId = Int32.Parse(indicatorData[1]);
            return target.floors[floorId - 1].Indicators[indicatorId - 1];
        }

        private static Indicator IdToIndicator(List<Indicator> indicatorList, string idString)
        {
            string[] indicatorData = idString.Split('@');
            int fromId = Int32.Parse(indicatorData[0]);
            return indicatorList[fromId - 1];
        }
    }
}