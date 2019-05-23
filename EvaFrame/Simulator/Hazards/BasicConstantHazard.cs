using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Simulator;

namespace EvaFrame.Simulator.Hazards
{
    /// <summary>
    /// Tình trạng thảm họa cơ bản: Một số hành lang ở tầng một sẽ bị cháy, và tình trạng thảm họa lan rộng lần hai sau khi thuật toán chạy được 30s.
    /// </summary>
    public class BasicConstantHazard : IHazard
    {
        private Building target = null;
        private double timeElapsed = 0;
        private bool secondUpdated = false;

        private int[] firstWave = new int[]
            { 20, 21, 22, 23, 24, 25, 26, 27, 28, 59, 60, 61, 62, 63, 64, 65, 66, 67, 82, 83, 84, 85, 86 };
        private int[] secondWave = new int[]
            { 25, 28, 64, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 84, 116, 118, 119, 121, 122, 124, 125, 127, 128, 131, 132 };

        void IHazard.Intialize(Building target)
        {
            this.target = target;
            UpdateEdges(firstWave);
        }

        void IHazard.Update(double updatePeriod)
        {
            if (target == null || secondUpdated)
                return;
            timeElapsed += updatePeriod;
            if (timeElapsed >= 30)
            {
                secondUpdated = true;
                UpdateEdges(secondWave);
            }
        }

        private void UpdateEdges(int[] affectedId)
        {
            List<Indicator> affectedIndicator = new List<Indicator>();
            foreach (int id in affectedId)
                affectedIndicator.Add(this.target.Floors[0].Indicators[id - 1]);

            Random rnd = new Random();
            foreach (Corridor c in target.Floors[0].Corridors)
            {
                if (!affectedIndicator.Contains(c.I1) || !affectedIndicator.Contains(c.I2))
                    continue;
                c.Trustiness = (double)(rnd.Next(2, 4)) / 10;
            }
        }
    }
}