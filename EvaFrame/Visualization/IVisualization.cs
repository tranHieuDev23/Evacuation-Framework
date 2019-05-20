using System;
using EvaFrame.Models.Building;

namespace EvaFrame.Visualization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVisualization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        void Initialize(Building target);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationStart"></param>
        /// <param name="simulationLatest"></param>
        void Update(DateTime simulationStart, DateTime simulationLatest);
    }
}