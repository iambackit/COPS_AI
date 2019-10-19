using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Data;

class PopulationManager : MonoBehaviour
{
    #region private
    PopulationGeneratorName _PopulationGeneratorName;
    List<GameObject> _Cars = new List<GameObject>();
    List<GameObject> _NewPopulation;
    GameObject _CarPrefab;

    int _Population = 0;
    #endregion


    public PopulationManager(PopulationGeneratorName populationGeneratorName, int population, GameObject carPrefab)
    {
        this._PopulationGeneratorName = populationGeneratorName;
        this._Population = population;
        this._CarPrefab = carPrefab;
    }

    public void CreateNewGeneration()
    {
        if (this._Cars.Count == 0)
        {
            GenerateFirstPopulation(_Cars);
            return;
        }

        switch (_PopulationGeneratorName)
        {
            case PopulationGeneratorName.HalfBestHalfCrossovered:
                GenerateNewPopulationHalfBestHalfCrossovered();
                break;
            case PopulationGeneratorName.IgnorePunishedIndividuals:
                GenerateNewPopulationIgnorePunishments();
                break;
            case PopulationGeneratorName.ChanceByScore:
                GenerateNewPopulationChanceByScore();
                break;
            default:
                throw new System.NotImplementedException("Not implemented method");
        }
    }

    private void GenerateFirstPopulation(List<GameObject> cars)
    {
        for (int i = 0; i < _Population; i++)
        {
            GameObject gameObjectCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            gameObjectCar.GetComponent<Car>().Initialize();
            _Cars.Add(gameObjectCar);
        }
    }

    private void GenerateNewPopulationHalfBestHalfCrossovered()
    {
        _NewPopulation = new List<GameObject>();

        _Cars = _Cars.OrderByDescending(x => x.GetComponent<Car>().Score).ToList();

        for (int i = 0; i < _Population / 2; i++)
        {
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Population / 2; i++)
        {
            DNA first = _Cars[i].GetComponent<Car>().DNA;
            DNA second = _Cars[i + 1].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }

    private void GenerateNewPopulationChanceByScore()
    {
        _NewPopulation = new List<GameObject>();
        _Cars = _Cars.Select(x => x).Where(x => x.GetComponent<Car>().Punished == false).ToList();
        int totalScore = _Cars.Sum(x => x.GetComponent<Car>().Score);

        int[] indexProbability = new int[totalScore];
        int actIndex = 0;
        for (int i = 0; i < _Cars.Count; i++)
        {
            for (int j = 0; j < _Cars[i].GetComponent<Car>().Score; j++)
            {
                indexProbability[actIndex] = i;
                actIndex++;
            }
        }

        for (int i = 0; i < _Population; i++)
        {
            int firstCarIndex = indexProbability[Random.Range(0, totalScore - 1)];
            int secondCarIndex = indexProbability[Random.Range(0, totalScore - 1)];

            DNA first = _Cars[firstCarIndex].GetComponent<Car>().DNA;
            DNA second = _Cars[secondCarIndex].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }

    private void GenerateNewPopulationIgnorePunishments()
    {
        _NewPopulation = new List<GameObject>();

        _Cars = _Cars.Select(x => x).Where(x => x.GetComponent<Car>().Punished == false).ToList();
        int notPunishedCarsCount = _Cars.Count;
        int howManyCarsWeNeedToAdd = _Population;

        for (int i = 0; i < notPunishedCarsCount; i++)
        {
            DNA dna = _Cars[i].GetComponent<Car>().DNA;
            dna.Mutate();
            GameObject newCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(dna);
            _NewPopulation.Add(newCar);
        }
        howManyCarsWeNeedToAdd -= notPunishedCarsCount;

        //get 2 random from the not punished set
        for (int i = 0; i < howManyCarsWeNeedToAdd; i++)
        {
            int firstCarIndex = Random.Range(0, notPunishedCarsCount - 1);
            int secondCarIndex = Random.Range(0, notPunishedCarsCount - 1);

            DNA first = _Cars[firstCarIndex].GetComponent<Car>().DNA;
            DNA second = _Cars[secondCarIndex].GetComponent<Car>().DNA;
            DNA crossOver = first.CrossOver(first, second);
            crossOver.Mutate();
            GameObject newCar = Instantiate(_CarPrefab, new Vector2(0, Random.Range(2f, 2.5f)), Quaternion.Euler(0, 0, 90));
            newCar.GetComponent<Car>().Initialize(crossOver);
            _NewPopulation.Add(newCar);
        }

        for (int i = 0; i < _Cars.Count; i++)
        {
            Destroy(_Cars[i].gameObject);
        }

        _Cars = _NewPopulation;
    }
}
