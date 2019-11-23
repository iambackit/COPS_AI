using UnityEngine.UI;

namespace Assets.Scripts.Interfaces
{
    public interface IStatisticCalculable
    {
        void SetStatisticTexts(Text current, Text best, Text basic);
        void SetNotChanginStats(int population, float mutationRate);
        void SetChaningStats(int currentGeneration, int carsAlive, int currentFitness);
    }
}
