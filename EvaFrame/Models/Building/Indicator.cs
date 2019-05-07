using System;
using System.Collections.Generic;

namespace EvaFrame.Models.Building
{
    class Indicator
    {
        private List<Corridor> neighbors;
        public List<Corridor> Neighbors { get { return neighbors; } }

        private Corridor next;
        public Corridor Next
        {
            get { return next; }

            set
            {
                if (value != null && !neighbors.Contains(value))
                    throw new InvalidOperationException("Corridor not found in neighbors.");
                next = value;
            }
        }

        public Indicator()
        {
            neighbors = new List<Corridor>();
            next = null;
        }
    }
}