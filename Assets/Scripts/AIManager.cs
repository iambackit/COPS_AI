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
        //for (int i = 0; i < _Cars.Count; i++)
        //    Destroy(_Cars[i].gameObject);
        //_Cars.Clear();

        //_ActiveCars = Population;
        //for (int i = 0;i<Population;i++)
        //{
        //    DNA crossOvered = _First.CrossOver(_First, _Second);
        //    crossOvered.Mutate();
        //    GenerateCar(crossOvered);
        //}
    }


    private void GenerateCar(DNA dna)
    {
        GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
        gameObjectCar.GetComponent<Car>().Initialize(dna);
        _Cars.Add(gameObjectCar);
    }

}
