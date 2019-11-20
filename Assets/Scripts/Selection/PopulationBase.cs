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
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int Population { get; set; }
        public List<GameObject> ActualGeneration { get; set; }
        public abstract void CreateNewGeneration();
        public int ActualPopulation;


        //public delegate void EventHandler(object source, EventArgs args);
        //public event System.EventHandler PopulationReduced;
        public event EventHandler<PopulationEventArgs> PopulationReduced;


        public void CreateFirstGeneration()
        {
            ActualGeneration = new List<GameObject>();
            ActualPopulation = this.Population;

            for (int i = 0; i < Population; i++)
            {
                GameObject gameObjectCar = Instantiate(Prefab, Position, Rotation);
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
