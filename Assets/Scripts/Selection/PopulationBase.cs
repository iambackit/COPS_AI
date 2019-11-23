using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Selection
{
    abstract class PopulationBase : MonoBehaviour, ISelectable
    {
        public GameObject Prefab { get; set; }
        public GameObject Target { get; set; }
        public List<Vector2> Positions { get; set; }
        public Quaternion Rotation { get; set; }
        public int Population { get; set; }
        public List<GameObject> ActualGeneration { get; set; }
        public abstract void CreateNewGeneration();
        public int ActualPopulation;
        public event EventHandler<PopulationEventArgs> PopulationReduced;

        public void CreateFirstGeneration()
        {
            ActualGeneration = new List<GameObject>();
            ActualPopulation = this.Population;

            for (int i = 0; i < Population; i++)
            {
                int randomPosition = UnityEngine.Random.Range(0, Positions.Count);
                GameObject gameObjectCar = Instantiate(Prefab, Positions[randomPosition], Rotation);
                Car car = gameObjectCar.GetComponent<Car>();
                car.Initialize(Target);
                car.CarEvent += ReducePopulation;
                ActualGeneration.Add(gameObjectCar);
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
