using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvaFrame.Models;

namespace EvaFrame.Models.Building
{
    // Các hàm phụ trợ cho việc load Building từ file dữ liệu
    public partial class Building
    {
        private static void LoadFloor(StreamReader sr, Building target, int floorId)
        {
            List<IndicatorImpl> indicatorImplList = LoadIndicatorsInFloor(sr, floorId);
            List<Corridor> corridorList = LoadCorridorsInFloor(sr, indicatorImplList);
            List<IndicatorImpl> stairImplList = LoadStairNodesInFloor(sr, indicatorImplList);
            List<Corridor> stairways = LoadStairwaysInFloor(sr, target, indicatorImplList);

            List<Indicator> indicatorList = new List<Indicator>(indicatorImplList);
            List<Indicator> stairList = new List<Indicator>(stairImplList);
            target.floors.Add(new FloorImpl(indicatorList, stairList, corridorList, stairways));
        }

        private static List<IndicatorImpl> LoadIndicatorsInFloor(StreamReader sr, int floorId)
        {
            List<IndicatorImpl> indicatorList = new List<IndicatorImpl>();
            int numInd = int.Parse(sr.ReadLine());
            if (numInd == 0)
                return indicatorList;
            string[] coordinateData = sr.ReadLine().Split(',');
            for (int i = 1; i <= numInd; i++)
            {
                string[] data = coordinateData[i - 1].Split(';');
                string id = i.ToString() + '@' + floorId.ToString();
                int x = int.Parse(data[0]);
                int y = int.Parse(data[1]);
                indicatorList.Add(new IndicatorImpl(id, x, y, floorId));
            }
            return indicatorList;
        }

        private static List<Corridor> LoadCorridorsInFloor(StreamReader sr, List<IndicatorImpl> indicatorList)
        {
            List<Corridor> result = new List<Corridor>();
            int numCor = int.Parse(sr.ReadLine());
            if (numCor == 0)
                return result;

            string[] corridorData = sr.ReadLine().Split(',');
            foreach (string data in corridorData)
            {
                string[] values = data.Split(';');
                IndicatorImpl I1 = IdToIndicator(indicatorList, values[0]);
                IndicatorImpl I2 = IdToIndicator(indicatorList, values[1]);
                double length = double.Parse(values[2]);
                double width = double.Parse(values[3]);
                double trustiness = double.Parse(values[4]);

                Corridor cor = new CorridorImpl(I1, I2, false, length, width, 0, trustiness);
                I1.neighbors.Add(cor);
                I2.neighbors.Add(cor);
                result.Add(cor);
            }

            return result;
        }

        private static List<IndicatorImpl> LoadStairNodesInFloor(StreamReader sr, List<IndicatorImpl> indicatorList)
        {
            List<IndicatorImpl> stairList = new List<IndicatorImpl>();
            int numStair = int.Parse(sr.ReadLine());
            if (numStair == 0)
                return stairList;
            string[] stairNodeIds = sr.ReadLine().Split(',');
            foreach (string idString in stairNodeIds)
            {
                IndicatorImpl stair = IdToIndicator(indicatorList, idString);
                stair.IsStairNode = true;
                stairList.Add(stair);
            }
            return stairList;
        }

        private static List<Corridor> LoadStairwaysInFloor(StreamReader sr, Building target, List<IndicatorImpl> indicatorList)
        {
            List<Corridor> stairways = new List<Corridor>();
            int numStairCor = int.Parse(sr.ReadLine());
            if (numStairCor == 0)
                return stairways;
            string[] stairData = sr.ReadLine().Split(',');
            foreach (string data in stairData)
            {
                string[] values = data.Split(';');
                IndicatorImpl I1 = IdToIndicator(indicatorList, values[0]);
                IndicatorImpl I2 = IdToIndicator(target, values[1]);
                double length = double.Parse(values[2]);
                double width = double.Parse(values[3]);
                double trustiness = double.Parse(values[4]);

                Corridor cor = new CorridorImpl(I1, I2, true, length, width, 0, trustiness);
                I1.neighbors.Add(cor);
                I2.neighbors.Add(cor);
                stairways.Add(cor);
            }
            return stairways;
        }

        private static void LoadExitNodes(StreamReader sr, Building target)
        {
            int numExit = int.Parse(sr.ReadLine());
            string[] exitNodeIds = sr.ReadLine().Split(',');
            foreach (string idString in exitNodeIds)
            {
                IndicatorImpl exit = IdToIndicator(target, idString);
                exit.IsExitNode = true;
                target.exits.Add(exit);
            }
        }

        private static void LoadPeople(StreamReader sr, Building target)
        {
            int numPeople = int.Parse(sr.ReadLine());
            for (int i = 0; i < numPeople; i++)
            {
                string[] peopleData = sr.ReadLine().Split(';');
                Indicator following = IdToIndicator(target, peopleData[0]);
                double speedMax = double.Parse(peopleData[1]);
                target.inhabitants.Add(new Person("P-" + i.ToString(), speedMax, following));
            }
        }

        private static IndicatorImpl IdToIndicator(Building target, string idString)
        {
            string[] indicatorData = idString.Split('@');
            int indicatorId = int.Parse(indicatorData[0]);
            int floorId = int.Parse(indicatorData[1]);
            return target.floors[floorId - 1].Indicators[indicatorId - 1] as IndicatorImpl;
        }

        private static IndicatorImpl IdToIndicator(List<IndicatorImpl> indicatorList, string idString)
        {
            string[] indicatorData = idString.Split('@');
            int fromId = int.Parse(indicatorData[0]);
            return indicatorList[fromId - 1];
        }
    }
}