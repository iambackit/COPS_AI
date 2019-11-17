using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IGenerationCreatable
    {
        void CreateFirstGeneration();
        void CreateNewGeneration();
    }
}
