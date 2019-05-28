using EvaFrame.Models.Building;

namespace EvaFrame.Simulator.Hazards
{
    /// <summary>
    /// Class mô phỏng tình trạng không có thảm họa.
    /// </summary>
    public class NoHazard : IHazard
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