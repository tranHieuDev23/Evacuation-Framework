using EvaFrame.Models.Building;

namespace EvaFrame.Models
{
    class Person
    {
        private double speedMax;
        public double SpeedMax { get { return speedMax; } }

        private double location;
        public double Location
        {
            get { return location; }
            set { location = value; }
        }

        private double completedPercent;
        public double CompletedPercent
        {
            get { return completedPercent; }
            set { completedPercent = value; }
        }
    }
}