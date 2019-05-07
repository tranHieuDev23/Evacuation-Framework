using EvaFrame.Models.Building;
using EvaFrame.Algorithm;

namespace EvaFrame.Simulator
{
    class Simulator
    {
        private Building target;
        private IAlgorithm algorithm;
        private IHazard hazard;

        public Simulator(Building target, IAlgorithm algorithm, IHazard hazard)
        {
            this.target = target;
            this.algorithm = algorithm;
            this.hazard = hazard;
        }

        public double RunSimulator(double hazardUpdatePeriod, double algorithmUpdatePeriod)
        {
            hazard.Intialize(target);
            algorithm.Intialize(target);
            double result = 0;
            return result;
        }
    }
}