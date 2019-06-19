using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Utilities;

namespace EvaFrame.Simulation.Hazards
{
    /// <summary>
    /// Tình trạng thảm họa xảy ra tại các khu vực không trọng yếu (không phải các hành lang nối với Stair Node và Exit Node).
    /// </summary>
    public class RandomNonCriticalHazard : IHazard
    {
        private int[] affectedFloorIds;
        private double affectedRate;
        private int spread;
        private Random random;
        private Dictionary<Indicator, bool> mark;

        /// <summary>
        /// Khởi tạo một đối tượng <c>RandomNonCriticalHazard</c> mới, với thảm họa xảy ra ở một số tầng dưới cùng của tòa nhà.
        /// </summary>
        /// <param name="numberOfLowestFloorsAffected">Số lượng tầng dưới cùng của tòa nhà bị ảng hưởng. Giá trị dương.</param>
        /// <param name="affectedRate">Tỷ lệ số lượng <c>Indicator</c> bị ảnh hưởng bởi thảm họa. Giá trị trong khoảng [0, 1].</param>
        /// <param name="spread">Bán kính của từng cụm thảm họa (số lượng cạnh bị ảnh hưởng, tính từ <c>Indicator</c> trung tâm của cụm thảm họa). Giá trị dương.</param>
        /// <param name="seed">Hạt giống ngẫu nhiên. Thay đổi hạt giống này để thay đổi trạng thái thảm họa.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Throw nếu như <c>numberOfLowestFloorsAffected</c> hoặc <c>spread</c> không phải là số dương, hoặc nếu <c>affectedRate</c> nằm ngoài khoảng [0, 1].
        /// </exception>
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

        /// <summary>
        /// Khởi tạo một đối tượng <c>RandomNonCriticalHazard</c> mới, với thảm họa xảy ra ở một số được chỉ định cụ thể.
        /// </summary>
        /// <param name="affectedFloorIds">Mảng chứa chỉ số các tầng bị ảnh hưởng.</param>
        /// <param name="affectedRate">Tỷ lệ số lượng Stair Node bị ảnh hưởng bởi thảm họa. Giá trị trong khoảng [0, 1].</param>
        /// <param name="spread">Bán kính của từng cụm thảm họa (số lượng cạnh bị ảnh hưởng, tính từ <c>Indicator</c> trung tâm của cụm thảm họa). Giá trị dương.</param>
        /// <param name="seed">Hạt giống ngẫu nhiên. Thay đổi hạt giống này để thay đổi trạng thái thảm họa.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Throw nếu như <c>affectedFloorIds</c> chứa chỉ số âm, <c>spread</c> không phải là số dương, hoặc nếu <c>affectedRate</c> nằm ngoài khoảng [0, 1].
        /// </exception>
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

        /// <exception cref="System.Exception">
        /// Throw nếu như đối tượng này cố gắng thực hiện thảm họa trên một tầng mà <c>target</c> không có.
        /// </exception>
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