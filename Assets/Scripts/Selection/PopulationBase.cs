using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Selection
{
    abstract class PopulationBase : MonoBehaviour, ISelectable
    {
        #region init setup
        public GameObject Prefab { get; set; }
        public Vector2 InitPosition { get; set; }
        public Quaternion Rotation { get; set; }
        public int Population { get; set; }
        #endregion

        public List<GameObject> ActualCars { get; set; }
        public abstract void CreateNewGeneration();
        public int ActualPopulation { get; protected set; }
        public int ActualGeneration { get; protected set; }
        public int BestFitness { get; protected set; }
        public event EventHandler<PopulationEventArgs> PopulationReduced;

        public void CreateFirstGeneration()
        {
            ActualCars = new List<GameObject>();
            ActualGeneration = 1;
            ActualPopulation = this.Population;

            for (int i = 0; i < Population; i++)
            {
                float xRandom = UnityEngine.Random.Range(InitPosition.x - 1f, InitPosition.x + 1f);
                float yRandom = UnityEngine.Random.Range(InitPosition.y - 0.5f, InitPosition.y + 0.5f);
                GameObject gameObjectCar = Instantiate(Prefab, new Vector2(xRandom,yRandom), Rotation);
                Car car = gameObjectCar.GetComponent<Car>();
                car.Initialize();
                car.CarEvent += ReducePopulation;
                ActualCars.Add(gameObjectCar);
            }
        }

        public void ReducePopulation(object source, EventArgs eventArgs)
        {
            --ActualPopulation;
            PopulationEventArgs args = new PopulationEventArgs();
            args.ActualPopulation = this.ActualPopulation;
            OnPopulationReduced(args);
        }

        protected virtual void OnPopulationReduced(PopulationEventArgs e)
        {
            EventHandler<PopulationEventArgs> handler = PopulationReduced;
            if (handler != null)
                handler(this, e);
        }


    }
}
