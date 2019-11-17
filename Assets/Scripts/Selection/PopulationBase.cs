using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Selection
{
    abstract class PopulationBase : MonoBehaviour, ISelectable
    {
        public GameObject Prefab { get; set; }
        public Vector2 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public int Population { get; set; }
        protected List<GameObject> _ActualGeneration = new List<GameObject>();
        public abstract void CreateNewGeneration();

        public void CreateFirstGeneration()
        {
            for (int i = 0; i < Population; i++)
            {
                GameObject gameObjectCar = Instantiate(Prefab, Position, Rotation);
                gameObjectCar.GetComponent<Car>().Initialize();
                _ActualGeneration.Add(gameObjectCar);
            }
        }

    }
}
