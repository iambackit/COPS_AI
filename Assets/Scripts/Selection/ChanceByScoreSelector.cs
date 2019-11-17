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

            for (int i = 0; i < _ActualGeneration.Count; i++)
            {
                if (_ActualGeneration[i].GetComponent<Car>().Punished)
                    Destroy(_ActualGeneration[i]);
            }
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

            for (int i = 0; i < _ActualGeneration.Count; i++)
            {
                Destroy(_ActualGeneration[i].gameObject);
            }

            _ActualGeneration = nextGeneration;
        }
    }
}
