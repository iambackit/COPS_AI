using System;

namespace Assets.Scripts.Interfaces
{
    public interface IGenerationCreatable
    {
        void CreateFirstGeneration();
        void CreateNewGeneration();
        void ReducePopulation(object source, EventArgs eventArgs);
    }
}
