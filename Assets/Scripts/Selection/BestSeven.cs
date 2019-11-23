using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Data;

namespace Assets.Scripts.Selection
{
    class BestSeven : PopulationBase
    {

        public override void CreateNewGeneration()
        {
            List<GameObject> nextGeneration = new List<GameObject>();

            CreateNextGeneration(nextGeneration);
            DestroyPreviousGenerationCars();
            ActualGeneration = nextGeneration;
            ActualPopulation = this.Population;

        }

        #region private

        private void CreateNextGeneration(List<GameObject> nextGeneration)
        {
            ActualGeneration = ActualGeneration.OrderBy(x => x.GetComponent<Car>().Score).ToList();
            DestroyZeroScores();

            for (int i = 0;i<TimesToUseCar.Length;i++)
            {
                for (int j = 0;j<TimesToUseCar[i];j++)
                {
                    DNA first = ActualGeneration[i].GetComponent<Car>().DNA;
                    DNA crossOver = first.CrossOver(first, first);
                    crossOver.Mutate();
                    GameObject newCar = Instantiate(Prefab, Position, Rotation);
                    Car car = newCar.GetComponent<Car>();
                    car.Initialize(crossOver, Target);
                    car.CarEvent += ReducePopulation;
                    nextGeneration.Add(newCar);
                }
            }
        }

        private void DestroyZeroScores()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                if (ActualGeneration[i].GetComponent<Car>().Score== 0)
                    Destroy(ActualGeneration[i].gameObject);
            }
        }
        private void DestroyPreviousGenerationCars()
        {
            for (int i = 0; i < ActualGeneration.Count; i++)
            {
                Destroy(ActualGeneration[i].gameObject);
            }
        }

        private int[] TimesToUseCar = new int[] { 25, 10, 5, 4, 3, 2, 1 };

        #endregion
    }
}
