using EvaFrame.Models.Building;

namespace EvaFrame.Visualization
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVisualization
    {
        void Initialize(Building target);

        void Update(double timeElapsed);
    }
}