using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

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
    #endregion

    void Start()
    {
        for (int i = 0; i < Population; i++)
        {
            GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            gameObjectCar.GetComponent<Car>().Initialize();
            _Cars.Add(gameObjectCar);
        }

        Debug.Log(_Generation++);
    }

    public void DestroyCar(GameObject gameObject)
    {
        if (_Cars.Count == 2) //we set the first and the second place randomly
        {
            _First = _Cars[0].GetComponent<Car>().DNA;
            _Second = _Cars[1].GetComponent<Car>().DNA;
        }
        if (_Cars.Count == 1)
        {
            if (!_First.Equals(_Cars[0].GetComponent<Car>().DNA)) //then we check, if we were right - if not then change
            {
                DNA temp = _First;
                _First = _Second;
                _Second = temp;
            }
            _Cars.Remove(gameObject);
            Destroy(gameObject);
            GenerateNewPopulation();
        }
        else
        {
            _Cars.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void GenerateNewPopulation()
    {
        _Cars.Clear();
        for (int i = 0;i<Population;i++)
        {
            DNA crossOvered = _First.CrossOver(_First, _Second);
            crossOvered.Mutate();
            GenerateCar(crossOvered);
        }
    }


    private void GenerateCar(DNA dna)
    {
        GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
        gameObjectCar.GetComponent<Car>().Initialize(dna);
        _Cars.Add(gameObjectCar);
    }
}
