using System.Collections.Generic;

namespace EvaFrame.Models.Building
{
    class Floor
    {
        List<Indicator> indicators, stairs;
        public List<Indicator> Indicators { get { return indicators; } }
        public List<Indicator> Stairs { get { return stairs; } }
    }
}