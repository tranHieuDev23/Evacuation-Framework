using EvaFrame.Models.Building;

namespace EvaFrame.Algorithm
{
    interface IAlgorithm
    {
        void Intialize(Building target);
        void Run();
    }
}