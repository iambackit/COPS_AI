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
            List<GameObject> nextGenerationCars = new List<GameObject>();

            PrintScores();

            KillAllCars();
            BestFitness = 1;

            ActualGeneration++;
            ActualPopulation = this.Population;

            DestroyZeroScoreCars();

            int[] indexProbability = CreateIndexProbabilities();
            CreatenextGenerationCars(indexProbability, nextGenerationCars);

            DestroyPreviousGenerationCars();
            ActualCars = nextGenerationCars;
        }

        #region private
        private void KillAllCars()
        {
            for (int i = 0; i < ActualCars.Count; i++)
            {
                Car actualCar = ActualCars[i].GetComponent<Car>();

                if (actualCar.IsAlive)
                {
                    actualCar.Kill();
                }
            }
        }

        private void PrintScores()
        {
            for (int i = 0; i < ActualCars.Count; i++)
            {
                Debug.Log(ActualCars[i].GetComponent<Car>().Score);
            }
        }

        private void DestroyPreviousGenerationCars()
        {
            for (int i = 0; i < ActualCars.Count; i++)
            {
                Destroy(ActualCars[i].gameObject);
            }
        }

        private void CreatenextGenerationCars(int[] indexProbability, List<GameObject> nextGenerationCars)
        {
            int totalScore = ActualCars.Sum(x => x.GetComponent<Car>().Score);
            //PrintScores();

            for (int i = 0; i < this.Population; i++)
            {
                int firstCarIndex = indexProbability[Random.Range(0, totalScore - 1)];
                int secondCarIndex = indexProbability[Random.Range(0, totalScore - 1)];

                DNA first = ActualCars[firstCarIndex].GetComponent<Car>().DNA;
                DNA second = ActualCars[secondCarIndex].GetComponent<Car>().DNA;
                DNA crossOver = first.CrossOver(first, second);
                crossOver.Mutate();

                float xRandom = Random.Range(InitPosition.x - 1f, InitPosition.x + 1f); ;
                float yRandom = Random.Range(InitPosition.y - 0.5f, InitPosition.y + 0.5f);
                GameObject newCar = Instantiate(Prefab, new Vector2(xRandom, yRandom), Rotation);
                Car car = newCar.GetComponent<Car>();
                car.Initialize(crossOver, Target);
                car.CarEvent += ReducePopulation;
                nextGenerationCars.Add(newCar);
            }
        }

        private void DestroyZeroScoreCars()
        {
            for (int i = 0; i < ActualCars.Count; i++)
            {
                if (ActualCars[i].GetComponent<Car>().Score == 0)
                    Destroy(ActualCars[i]);
            }
        }

        /*each car indicies will be in the array, as many times, as many score has
         * e.g. 
         * 0th car with 0 points won't be in the array
         * 1st car with 2 points will be 2 times
         * nth car with xth score will be xth times in the array*/
        private int[] CreateIndexProbabilities()
        {
            int totalScore = ActualCars.Sum(x => x.GetComponent<Car>().Score);
            int[] indexProbability = new int[totalScore];
            int actIndex = 0;

            for (int i = 0; i < ActualCars.Count; i++)
            {
                for (int j = 0; j < ActualCars[i].GetComponent<Car>().Score; j++)
                {
                    indexProbability[actIndex] = i;
                    actIndex++;
                }
            }

            return indexProbability;
        }

        private void FixedUpdate()
        {
            SetBestFitness();
        }


        private void SetBestFitness()
        {
            foreach (GameObject carGo in ActualCars)
            {
                Car car = carGo.GetComponent<Car>();

                if (car.Score > BestFitness)
                    BestFitness = car.Score;
            }
        }
        #endregion
    }

}
