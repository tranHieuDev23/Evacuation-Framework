using System.Collections.Generic;
using EvaFrame.Models;

namespace EvaFrame.Models.Building
{
    class Building
    {
        private List<Floor> floors;
        public List<Floor> Floors { get { return floors; } }

        private List<Indicator> exits;
        public List<Indicator> Exits { get { return exits; } }

        private List<Person> inhabitants;
        public List<Person> Inhabitants { get { return inhabitants; } }

        public Building(string filepath) {}

        public void MovePeople(double updatePeriod)
        {
            
        }
    }
}