using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Simulator.Hazards
{
    public class RandomCriticalHazard : IHazard
    {
        private int[] affectedFloorIds;
        private double affectedRate;
        private int spread;
        private Random random;
        private Dictionary<Indicator, bool> mark;

        public RandomCriticalHazard(int numberOfLowestFloorsAffected, double affectedRate, int spread, int seed = 12345)
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

        public RandomCriticalHazard(int[] affectedFloorIds, double affectedRate, int spread, int seed = 12345)
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
                int numAffected = (int) (affectedRate * floor.Stairs.Count);
                List<Indicator> shuffledIndList = ListShuffler.Shuffle<Indicator>(floor.Stairs, random);
                
                foreach (Indicator ind in shuffledIndList)
                {
                    if (mark.Count >= numAffected)
                        break;
                    if (mark.ContainsKey(ind))
                        if (mark[ind])
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
            mark[current] = true;
            foreach (Corridor c in current.Neighbors)
            {
                Indicator to = c.To(current);
                if (mark.ContainsKey(to))
                    if (mark[to])
                        continue;
                c.Trustiness = (double) random.Next(2, 4) / 10;
                SpreadHazard(to, stepLeft - 1, limit);
            }
        }
    }
}