using EvaFrame.Models.Building;

namespace EvaFrame.Simulator {
    interface IHazard {
        void Intialize(Building target);
        void Update(double updatePeriod);
    }
}