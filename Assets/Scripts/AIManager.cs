using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;
using System.Linq;

public class AIManager : MonoBehaviour
{
    #region public
    public GameObject CarPrefab;
    public int Population = 50;
    #endregion

    #region private
    private List<GameObject> _Cars = new List<GameObject>();
    private List<GameObject> _NewPopulation = new List<GameObject>();

    private DNA _First;
    private DNA _Second;

    private int _Generation = 0;
    private int _ActiveCars = 0;
    #endregion

    void Start()
    {
        _ActiveCars = Population;
        for (int i = 0; i < Population; i++)
        {
            GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            gameObjectCar.GetComponent<Car>().Initialize();
            _Cars.Add(gameObjectCar);
        }

    }

    public void DestroyCar(GameObject gameObject)
    {
        gameObject.SetActive(false);

        if (_ActiveCars == 1)
        {
            _Cars = _Cars.OrderByDescending(x => x.GetComponent<Car>().Score).ToList();
            GenerateNewPopulation();
        }

        _ActiveCars--;
    }

    private void GenerateNewPopulation()
    {
        Debug.Log(Population);

        _NewPopulation.Clear();
        _ActiveCars = Population;

        for (int i = 0;i<Population/2;i++)
        {
            //for (int j = 0; j < 2; j++)
            //{
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
            //}
        }

        for (int i = 0;i<Population/2 - 1;i++)
        {
            DNA first = _Cars[i].GetComponent<Car>().DNA;
            DNA second = _Cars[i+1].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0;i<_Cars.Count;i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }
}
