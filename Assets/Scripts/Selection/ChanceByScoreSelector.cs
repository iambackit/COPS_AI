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
            ActualGeneration = nextGeneration;
            ActualPopulation = this.Population;
        }

        #region private
        private void DestroyPreviousGenerationCars()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                Destroy(ActualGeneration[i].gameObject);
            }
        }

        private void CreateNextGeneration(int[] indexProbability, List<GameObject> nextGeneration)
        {
            int totalScore = ActualGeneration.Sum(x => x.GetComponent<Car>().Score);

            for (int i = 0; i < this.Population; i++)
            {
                int firstCarIndex = indexProbability[Random.Range(0, totalScore - 1)];
                int secondCarIndex = indexProbability[Random.Range(0, totalScore - 1)];

                DNA first = ActualGeneration[firstCarIndex].GetComponent<Car>().DNA;
                DNA second = ActualGeneration[secondCarIndex].GetComponent<Car>().DNA;
                DNA crossOver = first.CrossOver(first, second);
                crossOver.Mutate();
                GameObject newCar = Instantiate(Prefab, Position, Rotation);
                Car car = newCar.GetComponent<Car>();
                car.Initialize(crossOver, Target);
                car.CarEvent += ReducePopulation;
                nextGeneration.Add(newCar);
            }
        }

        private void IgnorePunishedCars()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                if (ActualGeneration[i].GetComponent<Car>().Punished)
                    Destroy(ActualGeneration[i]);
            }
        }

        /*each car indicies will be in the array, as many times, as many score has
         * e.g. 
         * 0th car with 0 points won't be in the array
         * 1st car with 2 points will be 2 times
         * nth car with xth score will be xth times in the array*/
        private int[] CreateIndexProbabilities()
        {
            int totalScore = ActualGeneration.Sum(x => x.GetComponent<Car>().Score);
            int[] indexProbability = new int[totalScore];
            int actIndex = 0;

            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                for (int j = 0; j < ActualGeneration[i].GetComponent<Car>().Score; j++)
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
