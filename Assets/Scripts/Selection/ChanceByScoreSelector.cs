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

            KillAllCars();

            IgnorePunishedCars();
            int[] indexProbability = CreateIndexProbabilities();
            CreateNextGeneration(indexProbability, nextGeneration);
            DestroyPreviousGenerationCars();
            ActualGeneration = nextGeneration;
            ActualPopulation = this.Population;

        }

        #region private
        private void KillAllCars()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                Car actualCar = ActualGeneration[i].GetComponent<Car>();

                if (actualCar.IsAlive)
                {
                    actualCar.Kill();
                }
            }
        }

        private void PrintScores()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                Debug.Log(ActualGeneration[i].GetComponent<Car>().Score);
            }
        }
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
            PrintScores();

            for (int i = 0; i < this.Population; i++)
            {
                int firstCarIndex = indexProbability[Random.Range(0, totalScore - 1)];
                int secondCarIndex = indexProbability[Random.Range(0, totalScore - 1)];

                DNA first = ActualGeneration[firstCarIndex].GetComponent<Car>().DNA;
                DNA second = ActualGeneration[secondCarIndex].GetComponent<Car>().DNA;
                DNA crossOver = first.CrossOver(first, second);
                crossOver.Mutate();

                int randomPosition = UnityEngine.Random.Range(0, Positions.Count);
                GameObject newCar = Instantiate(Prefab, Positions[randomPosition], Rotation);
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
                if (ActualGeneration[i].GetComponent<Car>().Score == 0)
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
