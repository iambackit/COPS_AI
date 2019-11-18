using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Assets.Scripts.Data;

namespace Assets.Scripts.Selection
{
    class ChanceByScoreSelector : PopulationBase
    {
        public override void CreateNewGeneration()
        {
            List<GameObject> nextGeneration = new List<GameObject>();

            IgnorePunishedCars();
            int[] indexProbability = CreateIndexProbabilities();
            CreateNextGeneration(indexProbability, nextGeneration);
            DestroyPreviousGenerationCars();
            _ActualGeneration = nextGeneration;
        }

        #region private
        private void DestroyPreviousGenerationCars()
        {
            for (int i = 0; i < _ActualGeneration.Count; i++)
            {
                Destroy(_ActualGeneration[i].gameObject);
            }
        }

        private void CreateNextGeneration(int[] indexProbability, List<GameObject> nextGeneration)
        {
            int totalScore = _ActualGeneration.Sum(x => x.GetComponent<Car>().Score);

            for (int i = 0; i < this.Population; i++)
            {
                int firstCarIndex = indexProbability[Random.Range(0, totalScore - 1)];
                int secondCarIndex = indexProbability[Random.Range(0, totalScore - 1)];

                DNA first = _ActualGeneration[firstCarIndex].GetComponent<Car>().DNA;
                DNA second = _ActualGeneration[secondCarIndex].GetComponent<Car>().DNA;
                DNA crossOver = first.CrossOver(first, second);
                crossOver.Mutate();
                GameObject newCar = Instantiate(Prefab, Position, Rotation);
                newCar.GetComponent<Car>().Initialize(crossOver);
                nextGeneration.Add(newCar);
            }
        }

        private void IgnorePunishedCars()
        {
            for (int i = 0; i < _ActualGeneration.Count; i++)
            {
                if (_ActualGeneration[i].GetComponent<Car>().Punished)
                    Destroy(_ActualGeneration[i]);
            }
        }

        /*each car indicies will be in the array, as many times, as many score has
         * e.g. 
         * 0th car with 0 points won't be in the array
         * 1st car with 2 points will be 2 times
         * nth car with xth score will be xth times in the array*/
        private int[] CreateIndexProbabilities()
        {
            int totalScore = _ActualGeneration.Sum(x => x.GetComponent<Car>().Score);
            int[] indexProbability = new int[totalScore];
            int actIndex = 0;

            for (int i = 0; i < _ActualGeneration.Count; i++)
            {
                for (int j = 0; j < _ActualGeneration[i].GetComponent<Car>().Score; j++)
                {
                    indexProbability[actIndex] = i;
                    actIndex++;
                }
            }

            return indexProbability;
        }
        #endregion
    }

}
