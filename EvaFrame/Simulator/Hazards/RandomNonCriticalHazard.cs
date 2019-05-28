using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Simulator.Hazards
{
    public class RandomNonCriticalHazard : IHazard
    {
        private int[] affectedFloorIds;
        private double affectedRate;
        private int spread;
        private Random random;
        private Dictionary<Indicator, bool> mark;

        public RandomNonCriticalHazard(int numberOfLowestFloorsAffected, double affectedRate, int spread, int seed = 12345)
        {
            if (numberOfLowestFloorsAffected <= 0)
                throw new ArgumentOutOfRangeException("numberOfLowestFloorsAffected", "numberOfLowestFloorsAffected must be positive!");
            if (spread <= 0)
                throw new ArgumentOutOfRangeException("spread", "spread must be positive!");
            this.affectedFloorIds = new int[numberOfLowestFloorsAffected];
            for (int i = 0; i < numberOfLowestFloorsAffected; i++)
                this.affectedFloorIds[i] = i;
            this.affectedRate = affectedRate;            
            this.spread = spread;
            this.random = new Random(seed);
        }

        public RandomNonCriticalHazard(int[] affectedFloorIds, double affectedRate, int spread, int seed = 12345)
        {
            foreach (int id in affectedFloorIds)
                if (id < 0)
                    throw new ArgumentOutOfRangeException("One of the floor ids is negative!");
            if (spread <= 0)
                throw new ArgumentOutOfRangeException("spread", "spread must be positive!");
            this.affectedFloorIds = affectedFloorIds;
            this.affectedRate = affectedRate;
            this.spread = spread;
            this.random = new Random(seed);
        }

        void IHazard.Intialize(Building target)
        {
            foreach (int id in affectedFloorIds)
            {
                if (id >= target.Floors.Count)
                    throw new Exception("Target building has fewer floors than specified in the hazard!");

                mark = new Dictionary<Indicator, bool>();
                Floor floor = target.Floors[id];
                int numAffected = (int) (affectedRate * floor.Indicators.Count);
                List<Indicator> shuffledIndList = ListShuffler.Shuffle<Indicator>(floor.Indicators, random);

                foreach (Indicator ind in shuffledIndList)
                {
                    if (mark.Count >= numAffected)
                        break;
                    if (ind.IsStairNode || ind.IsExitNode)
                        continue;
                    if (mark.ContainsKey(ind))
                        if (mark[ind])
                            continue;
                    double roll = random.NextDouble();
                    if (roll > affectedRate)
                        continue;
                    SpreadHazard(ind, spread, numAffected);
                }
            }
            mark = null;
        }

        void IHazard.Update(double updatePeriod)
        {

        }

        private void SpreadHazard(Indicator current, int stepLeft, int limit)
        {
            if (stepLeft <= 0 || mark.Count >= limit)
                return;
            if (mark.ContainsKey(current))
                if (mark[current])
                    return;
            mark[current] = true;
            foreach (Corridor c in current.Neighbors)
            {
                Indicator to = c.To(current);
                if (mark.ContainsKey(to))
                    if (mark[to])
                        continue;
                if (!c.I1.IsStairNode && !c.I1.IsExitNode && !c.I2.IsStairNode && !c.I2.IsExitNode)                    
                    c.Trustiness = (double) random.Next(2, 4) / 10;
                SpreadHazard(to, stepLeft - 1, limit);
            }
        }
    }
}