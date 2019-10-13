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

    private GUIStyle guiStyle;

    private int _Generation = 0;
    private int _ActiveCars = 0;

    //for some statistic
    private int _DeathByWall = 0;
    private int _DeathByPunishment = 0;
    #endregion

    void Start()
    {
        GenerateFirstPopulation();
        guiStyle = new GUIStyle();
    }


    public void DestroyCar(GameObject gameObject)
    {
        gameObject.SetActive(false);
        if (gameObject.GetComponent<Car>().Punished) _DeathByPunishment++;
        else _DeathByWall++;

        if (_ActiveCars == 1)
        {
            _Cars = _Cars.OrderByDescending(x => x.GetComponent<Car>().Score).ToList();
            GenerateNewPopulation();
        }

        _ActiveCars--;
    }

    private void GenerateFirstPopulation()
    {
        _ActiveCars = Population;
        for (int i = 0; i < Population; i++)
        {
            GameObject gameObjectCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            gameObjectCar.GetComponent<Car>().Initialize();
            _Cars.Add(gameObjectCar);
        }
        _Generation++;
    }

    private void GenerateNewPopulation()
    {
        _NewPopulation.Clear();
        _ActiveCars = Population;

        for (int i = 0;i<Population/2;i++)
        {
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(CarPrefab, new Vector2(0, 2.5f), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
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

    #region GUI
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.red;
        GUI.BeginGroup(new Rect(10, 10, 350, 250));
        GUI.Label(new Rect(0, 0, 100, 100), StringContainer.Generation + _Generation, guiStyle);
        GUI.Label(new Rect(0, 30, 100, 100), StringContainer.Population + Population, guiStyle);
        GUI.Label(new Rect(0, 60, 100, 100), StringContainer.CurrentPopulation + _ActiveCars, guiStyle);
        GUI.Label(new Rect(0, 90, 100, 100), StringContainer.DeathByWall + _DeathByWall, guiStyle);
        GUI.Label(new Rect(0, 120, 100, 100), StringContainer.DeathByPunishment + _DeathByPunishment, guiStyle);
        GUI.EndGroup();
    }
    #endregion
}
