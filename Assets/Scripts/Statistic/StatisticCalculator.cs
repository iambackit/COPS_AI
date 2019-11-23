using UnityEngine;
using Assets.Scripts.Interfaces;
using UnityEngine.UI;
using Assets.Scripts.Data;

namespace Assets.Scripts.Statistic
{
    class StatisticCalculator : MonoBehaviour, IStatisticCalculable
    {
        private Text _Current;
        private Text _Best;
        private Text _Basic;

        private int _BestGeneration = 1;
        private int _BestFitness = 0;

        public void SetStatisticTexts(Text current, Text best, Text basic)
        {
            this._Current = current;
            this._Best = best;
            this._Basic = basic;
        }

        public void SetNotChanginStats(int population, float mutationRate)
        {
            _Basic.text = string.Format("{0}{1}\n{2}{3}%", StringContainer.Population, population, StringContainer.MutationRate, mutationRate * 100);
        }

        public void SetChaningStats(int currentGeneration, int carsAlive, int currentFitness)
        {
            _Current.text = string.Format("{0}{1}\n{2}{3}\n{4}{5}", StringContainer.Generation, currentGeneration, StringContainer.CarsAlive, carsAlive,
                StringContainer.Fitness, currentFitness);

            if (currentFitness > _BestFitness)
            {
                _BestFitness = currentFitness;
                _BestGeneration = currentGeneration;

                _Best.text = string.Format("{0}{1}\n{2}{3}", StringContainer.Generation, currentGeneration, StringContainer.Fitness, currentFitness);
            }
        }
    }
}
