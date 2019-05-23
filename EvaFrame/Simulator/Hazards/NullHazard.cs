using System;
using System.Collections.Generic;
using EvaFrame.Models.Building;
using EvaFrame.Simulator;

namespace EvaFrame.Simulator.Hazards
{
    /// <summary>
    /// Class mô phỏng thảm họa, chỉ dùng trong mục đích test
    /// </summary>
    public class NullHazard : IHazard
    {
        void IHazard.Intialize(Building target){
            return;
        }

        void IHazard.Update(double updatePeriod)
        {
            return;
        }
    }
}